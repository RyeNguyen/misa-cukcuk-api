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
                        var columnHeader = worksheet.Cells[2, column].Value.ToString().Trim();

                        if (columnHeader == Entity.Properties.ImportScenarioVN.fieldNameCode)
                        {
                            if (itemToCheck is null or "")
                            {
                                var errorMsg = Entity.Properties.ImportScenarioVN.fieldNameCode.Replace("(*)", "") + 
                                    Entity.Properties.ImportScenarioVN.messageErrorRequired;
                                customer.ImportErrors.Add(errorMsg);
                            }
                            else if (CheckDuplicateCode(itemToCheck.ToString().Trim()))
                            {
                                var errorMsg = Entity.Properties.ImportScenarioVN.fieldNameCode + Entity.Properties.ImportScenarioVN.messageErrorDuplicateSystem;
                                customer.ImportErrors.Add(errorMsg);
                            }
                            else
                            {
                                customer.CustomerCode = itemToCheck.ToString().Trim();
                            }
                        }

                        if (columnHeader == Entity.Properties.ImportScenarioVN.fieldNameName)
                        {
                            if (itemToCheck is null or "")
                            {
                                var errorMsg = Entity.Properties.ImportScenarioVN.fieldNameName.Replace("(*)", "") +
                                    Entity.Properties.ImportScenarioVN.messageErrorRequired;
                                customer.ImportErrors.Add(errorMsg);
                            }
                            else
                            {
                                customer.FullName = itemToCheck.ToString().Trim();
                            }
                        }

                        if (columnHeader == Entity.Properties.ImportScenarioVN.fieldNameCard)
                        {
                            customer.MemberCard = itemToCheck?.ToString().Trim();
                        }

                        if (columnHeader == Entity.Properties.ImportScenarioVN.fieldNameGroup)
                        {
                            customer.CustomerGroupName = itemToCheck?.ToString().Trim();
                        }

                        if (columnHeader == Entity.Properties.ImportScenarioVN.fieldNamePhone)
                        {
                            if (itemToCheck is null or "")
                            {
                                var errorMsg = Entity.Properties.ImportScenarioVN.fieldNamePhone +
                                    Entity.Properties.ImportScenarioVN.messageErrorRequired;
                                customer.ImportErrors.Add(errorMsg);
                            }
                            else
                            {
                                customer.PhoneNumber = itemToCheck.ToString().Trim().Replace(".", "");
                            }
                        }

                        if (columnHeader == Entity.Properties.ImportScenarioVN.fieldNameDob)
                        {
                            customer.DateOfBirth = FormatDate(itemToCheck);
                        }

                        if (columnHeader == Entity.Properties.ImportScenarioVN.fieldNameCompany)
                        {
                            customer.CompanyName = itemToCheck?.ToString().Trim();
                        }

                        if (columnHeader == Entity.Properties.ImportScenarioVN.fieldNameTax)
                        {
                            customer.CompanyTaxCode = itemToCheck?.ToString().Trim();
                        }

                        if (columnHeader == Entity.Properties.ImportScenarioVN.fieldNameEmail)
                        {
                            if (itemToCheck is null or "")
                            {
                                var errorMsg = Entity.Properties.ImportScenarioVN.fieldNameEmail +
                                    Entity.Properties.ImportScenarioVN.messageErrorRequired;
                                customer.ImportErrors.Add(errorMsg);
                            }
                            else if (CheckEmailFormat(itemToCheck.ToString().Trim()) == false)
                            {
                                var errorMsg = Entity.Properties.ImportScenarioVN.fieldNameEmail +
                                    Entity.Properties.ImportScenarioVN.messageErrorFormat;
                                customer.ImportErrors.Add(errorMsg);
                            }
                            else
                            {
                                customer.Email = itemToCheck.ToString().Trim();
                            }
                        }

                        if (columnHeader == Entity.Properties.ImportScenarioVN.fieldNameAddress)
                        {
                            customer.Address = itemToCheck?.ToString().Trim();
                        }
                    }

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

        private static DateTime? FormatDate(object date)
        {
            if (date != null)
            {
                string dateString = date.ToString().Trim();
                if (Regex.IsMatch(dateString, @"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$"))
                {
                    return DateTime.ParseExact(dateString, "dd/MM/yyyy", null);
                }

                if (Regex.IsMatch(dateString, @"^(((0)[0-9])|((1)[0-2]))(\/)\d{4}$"))
                {
                    return DateTime.ParseExact("01/" + dateString, "dd/MM/yyyy", null);
                }

                if (Regex.IsMatch(dateString, @"^\d{4}$"))
                {
                    return DateTime.ParseExact("01/01/" + dateString, "dd/MM/yyyy", null);
                }
            }

            return null;
        }

        private static bool CheckEmailFormat(string email)
        {
            var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

            return Regex.IsMatch(email, emailFormat, RegexOptions.IgnoreCase);
        }
        #endregion
    }
}
