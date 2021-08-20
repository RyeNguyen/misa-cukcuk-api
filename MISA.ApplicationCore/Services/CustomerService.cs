using Microsoft.AspNetCore.Http;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.ApplicationCore.Services;
using MISA.Entity;
using MISA.Infrastructure.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace MISA.ApplicationCore
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        readonly ICustomerRepository _customerRepository;
        readonly ServiceResponse _serviceResponse;

        public CustomerService(IBaseRepository<Customer> baseRepository,
            ICustomerRepository customerRepository) : base(baseRepository)
        {
            _customerRepository = customerRepository;
            _serviceResponse = new ServiceResponse();
        }

        /// <summary>
        /// Phương thức xử lý chức năng nhập khẩu dữ liệu Excel
        /// </summary>
        /// <param name="formFile">File nhập khẩu</param>
        /// <param name="cancellationToken">Token</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (20/08/2021)
        public ServiceResponse Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            var customers = new List<Customer>();

            //Check file hợp lệ (file phải có định dạng xls hoặc xlsx):
            if (formFile == null)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.Resources.messageErrorNullFile,
                    userMsg = Entity.Properties.Resources.messageErrorNullFile,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Data = errorObj;
                _serviceResponse.Message = Entity.Properties.Resources.messageErrorNullFile;
                _serviceResponse.MISACode = MISACode.NotValid;
                return _serviceResponse;
            }

            //Check độ lớn của file (Giới hạn 150Mb):

            //Thực hiện đọc dữ liệu trong tệp Excel:
            using (var stream = new MemoryStream())
            {
                formFile.CopyToAsync(stream, cancellationToken);

                using var package = new ExcelPackage(stream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;
                var columnCount = worksheet.Dimension.Columns;

                for (int row = 3; row <= rowCount; row++)
                {
                    var customer = new Customer();

                    for (int column = 1; column <= columnCount; column++)
                    {
                        var itemToCheck = worksheet.Cells[row, column].Value;

                        switch (worksheet.Cells[2, column].Value.ToString().Trim())
                        {
                            case "Mã khách hàng (*)":
                                
                                if (itemToCheck is null or "")
                                {
                                    var errorMsg = "Mã khách hàng không được phép để trống.";
                                    customer.ImportErrors.Add(errorMsg);
                                }
                                else if (CheckDuplicateCode(itemToCheck.ToString().Trim()))
                                {
                                    var errorMsg = "Mã khách hàng đã tồn tại trong hệ thống.";
                                    customer.ImportErrors.Add(errorMsg);
                                }
                                else
                                {
                                    customer.CustomerCode = itemToCheck.ToString().Trim();
                                }
                                break;

                            case "Tên khách hàng (*)":
                                if (itemToCheck is null or "")
                                {
                                    var errorMsg = "Tên khách hàng không được phép để trống.";
                                    customer.ImportErrors.Add(errorMsg);
                                }
                                else
                                {
                                    customer.FullName = itemToCheck.ToString().Trim();
                                }
                                break;

                            case "Mã thẻ thành viên":
                                customer.MemberCard = itemToCheck.ToString().Trim();
                                break;

                            case "Nhóm khách hàng":
                                customer.CustomerGroupName = itemToCheck.ToString().Trim();
                                break;

                            case "Số điện thoại":
                                if (itemToCheck is null or "")
                                {
                                    var errorMsg = "Số điện thoại không được phép để trống.";
                                    customer.ImportErrors.Add(errorMsg);
                                }
                                else
                                {
                                    customer.PhoneNumber = itemToCheck.ToString().Trim().Replace(".", "");
                                }
                                break;

                            case "Ngày sinh":

                                break;

                            case "Tên công ty":
                                customer.CompanyName = itemToCheck.ToString().Trim();
                                break;

                            case "Mã số thuế":
                                customer.CompanyTaxCode = itemToCheck.ToString().Trim();
                                break;

                            case "Email":
                                if (itemToCheck is null or "")
                                {
                                    var errorMsg = "Email không được phép để trống.";
                                    customer.ImportErrors.Add(errorMsg);
                                }
                                else if (CheckEmailFormat(itemToCheck.ToString().Trim()) == false)
                                {
                                    var errorMsg = "Email không đúng định dạng.";
                                    customer.ImportErrors.Add(errorMsg);
                                }
                                else
                                {
                                    customer.Email = itemToCheck.ToString().Trim();
                                }
                                break;

                            case "Địa chỉ":
                                customer.Address = itemToCheck.ToString().Trim();
                                break;
                        }
                    }

                    Console.WriteLine(customer); 
                    customers.Add(customer);
                }
            }
            _serviceResponse.Data = customers;
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }

        public ServiceResponse Pagination(string customerFilter, Guid? customerGroupId, int pageIndex, int pageSize)
        {
            _serviceResponse.Data = _customerRepository.Pagination(customerFilter, customerGroupId, pageIndex, pageSize);

            return _serviceResponse;
        }

        #region Kiểm tra dữ liệu hợp lệ
        /// <summary>
        /// Hàm validate thông tin nhân viên
        /// </summary>
        /// <param name="employee">Thông tin nhân viên</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (19/08/2021)
        protected override ServiceResponse ValidateCustom(Customer customer)
        {
            //Check trùng mã:
            var checkCode = CheckDuplicateCode(customer.CustomerCode);
            if (checkCode == false)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.Resources.messageErrorDuplicateCodeCus,
                    userMsg = Entity.Properties.Resources.messageErrorDuplicateCodeCus,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Data = errorObj;
                _serviceResponse.MISACode = MISACode.NotValid;
                _serviceResponse.Message = Entity.Properties.Resources.messageErrorDuplicateCodeCus;
                return _serviceResponse;
            }

            //Check Email:
            var isMatch = CheckEmailFormat(customer.Email);
            if (isMatch == false)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.Resources.messageErrorEmailFormat,
                    userMsg = Entity.Properties.Resources.messageErrorEmailFormat,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Message = Entity.Properties.Resources.messageErrorEmailFormat;
                _serviceResponse.MISACode = MISACode.NotValid;
                _serviceResponse.Data = errorObj;
                return _serviceResponse;
            }

            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }

        private bool CheckDuplicateCode(string code)
        {
            return _customerRepository.CheckDuplicateCode(code);
        }

        private bool CheckEmailFormat(string email)
        {
            var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

            return Regex.IsMatch(email, emailFormat, RegexOptions.IgnoreCase);
        }
        #endregion
    }
}
