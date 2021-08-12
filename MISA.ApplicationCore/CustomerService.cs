using MISA.ApplicationCore.Entities;
using MISA.Entity;
using MISA.Infrastructure;
using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore
{
    public class CustomerService
    {
        #region Method
        /// <summary>
        /// Lấy danh sách khách hàng từ DB
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        public IEnumerable<Customer> GetCustomers()
        {
            var customerContext = new CustomerContext();
            var customers = customerContext.GetCustomers();
            return customers;
        }

        /// <summary>
        /// Thêm mới khách hàng vào DB
        /// </summary>
        /// <param name="customer">Dữ liệu khách hàng muốn thêm</param>
        /// <returns>Phản hồi tương ứng có thêm thành công hay không</returns>
        /// Author: NQMinh (12/08/2021)
        public ServiceResponse InsertCustomer(Customer customer)
        {
            var serviceResponse = new ServiceResponse();
            var customerContext = new CustomerContext();
            //Validate dữ liệu, nếu dữ liệu chưa hợp lệ thì trả về mô tả lỗi:          
            //Check trường bắt buộc nhập:
            var customerCode = customer.CustomerCode;
            if (string.IsNullOrEmpty(customerCode))
            {
                var msg = new
                {
                    devMsg = new
                    {
                        fieldName = "CustomerCode",
                        msg = "Mã khách hàng không được phép để trống."
                    },
                    userMsg = "Mã khách hàng không được phép để trống.",
                    Code = MISACode.NotValid
                };
                serviceResponse.Data = msg;
                serviceResponse.Message = "Mã khách hàng không được phép để trống.";                
                serviceResponse.MISACode = MISACode.NotValid;
                return serviceResponse;
            }

            //Check trùng mã: 
            var customerToCheck = customerContext.GetCustomerbyCode(customerCode);
            if (customerToCheck != null)
            {
                var msg = new
                {
                    devMsg = new
                    {
                        fieldName = "CustomerCode",
                        msg = "Mã khách hàng đã tồn tại."
                    },
                    userMsg = "Mã khách hàng đã tồn tại, xin vui lòng kiểm tra lại.",
                    Code = MISACode.NotValid
                };
                serviceResponse.Data = msg;
                serviceResponse.Message = "Mã khách hàng đã tồn tại";
                serviceResponse.MISACode = MISACode.NotValid;
                return serviceResponse;
            }

            //Thêm mới khi dữ liệu hợp lệ:
            var rowAffects = customerContext.InsertCustomer(customer);
            serviceResponse.Data = rowAffects;
            serviceResponse.Message = "Thêm dữ liệu khách hàng thành công";
            serviceResponse.MISACode = MISACode.isValid;
            return serviceResponse;
        }

        //Sửa thông tin khách hàng

        //Xóa khách hàng
        #endregion
    }
}
