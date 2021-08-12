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
        #region Phương thức lấy tất cả dữ liệu khách hàng
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
        #endregion

        #region Phương thức lấy dữ liệu khách hàng theo ID
        public Customer GetCustomerById(Guid customerId)
        {
            var customerContext = new CustomerContext();
            var customer = customerContext.GetCustomerById(customerId);          
            return customer;
        }
        #endregion

        #region Phương thức thêm dữ liệu khách hàng vào DB
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
                        msg = Properties.Resources.messageCheckRequired_Dev
                    },
                    userMsg = Properties.Resources.messageCheckRequired_User,
                    Code = MISACode.NotValid
                };
                serviceResponse.Data = msg;
                serviceResponse.Message = Properties.Resources.messageCheckRequired_Dev;                
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
                        msg = Properties.Resources.messageCheckCodeDuplicate_Dev
                    },
                    userMsg = Properties.Resources.messageCheckCodeDuplicate_User,
                    Code = MISACode.NotValid
                };
                serviceResponse.Data = msg;
                serviceResponse.Message = Properties.Resources.messageCheckCodeDuplicate_Dev;
                serviceResponse.MISACode = MISACode.NotValid;
                return serviceResponse;
            }

            //Thêm mới khi dữ liệu hợp lệ:
            var rowAffects = customerContext.InsertCustomer(customer);
            serviceResponse.Data = rowAffects;
            serviceResponse.Message = Properties.Resources.messageInsertSuccess;
            serviceResponse.MISACode = MISACode.isValid;
            return serviceResponse;
        }
        #endregion

        //Sửa thông tin khách hàng
        public ServiceResponse UpdateCustomer(Guid customerId, Customer customer)
        {
            var serviceResponse = new ServiceResponse();
            var customerContext = new CustomerContext();

            //Validate dữ liệu, nếu dữ liệu chưa hợp lệ thì trả về mô tả lỗi:          
            //Check trường bắt buộc nhập:
            var customerCode = customer.CustomerCode;
            //if (string.IsNullOrEmpty(customerCode))
            //{
            //    var msg = new
            //    {
            //        devMsg = new
            //        {
            //            fieldName = "CustomerCode",
            //            msg = Properties.Resources.messageCheckRequired_Dev
            //        },
            //        userMsg = Properties.Resources.messageCheckRequired_User,
            //        Code = MISACode.NotValid
            //    };
            //    serviceResponse.Data = msg;
            //    serviceResponse.Message = Properties.Resources.messageCheckRequired_Dev;
            //    serviceResponse.MISACode = MISACode.NotValid;
            //    return serviceResponse;
            //}

            //Check trùng mã: 
            var customerToCheck = customerContext.GetCustomerbyCode(customerCode);
            if (customerToCheck != null)
            {
                var msg = new
                {
                    devMsg = new
                    {
                        fieldName = "CustomerCode",
                        msg = Properties.Resources.messageCheckCodeDuplicate_Dev
                    },
                    userMsg = Properties.Resources.messageCheckCodeDuplicate_User,
                    Code = MISACode.NotValid
                };
                serviceResponse.Data = msg;
                serviceResponse.Message = Properties.Resources.messageCheckCodeDuplicate_Dev;
                serviceResponse.MISACode = MISACode.NotValid;
                return serviceResponse;
            }

            //Sửa thông tin khi dữ liệu hợp lệ:
            var rowAffects = customerContext.UpdateCustomer(customerId, customer);
            serviceResponse.Data = rowAffects;
            serviceResponse.Message = Properties.Resources.messageInsertSuccess;
            serviceResponse.MISACode = MISACode.isValid;
            return serviceResponse;
        }

        //Xóa khách hàng
        #endregion
    }
}
