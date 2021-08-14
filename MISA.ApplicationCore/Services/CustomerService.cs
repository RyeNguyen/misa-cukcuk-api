using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Entity;
using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace MISA.ApplicationCore
{
    public class CustomerService : ICustomerService
    {
        readonly ICustomerRepository _customerRepository;
        readonly ServiceResponse _serviceResponse;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _serviceResponse = new ServiceResponse();
        }

        #region Method
        #region Phương thức lấy tất cả dữ liệu khách hàng
        /// <summary>
        /// Lấy danh sách khách hàng từ DB
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        public List<Customer> GetCustomers()
        {            
            var customers = _customerRepository.GetCustomers();
            return customers;
        }
        #endregion

        #region Phương thức lấy dữ liệu khách hàng theo ID
        public Customer GetCustomerById(Guid customerId)
        {            
            var customer = _customerRepository.GetCustomerById(customerId);          
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
                _serviceResponse.Data = msg;
                _serviceResponse.Message = Properties.Resources.messageCheckRequired_Dev;
                _serviceResponse.MISACode = MISACode.NotValid;
                return _serviceResponse;
            }

            //Check trùng mã: 
            var customerToCheck = _customerRepository.GetCustomerByCode(customerCode);
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
                _serviceResponse.Data = msg;
                _serviceResponse.Message = Properties.Resources.messageCheckCodeDuplicate_Dev;
                _serviceResponse.MISACode = MISACode.NotValid;
                return _serviceResponse;
            }

            //Thêm mới khi dữ liệu hợp lệ:
            var rowAffects = _customerRepository.InsertCustomer(customer);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Properties.Resources.messageInsertSuccess;
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }
        #endregion

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="customerId">ID của khách hàng cần cập nhật</param>
        /// <param name="customer">Dữ liệu để thay đổi</param>
        /// <returns>Phản hồi tương ứng có cập nhật thành công hay không</returns>
        /// Author: NQMinh (12/08/2021)
        public ServiceResponse UpdateCustomer(Guid customerId, Customer customer)
        {                      
            //Validate dữ liệu, nếu dữ liệu chưa hợp lệ thì trả về mô tả lỗi:          
            //Check trùng mã: 
            var customerCode = customer.CustomerCode;
            
            var customerToCheck = _customerRepository.GetCustomerByCode(customerCode);
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
                _serviceResponse.Data = msg;
                _serviceResponse.Message = Properties.Resources.messageCheckCodeDuplicate_Dev;
                _serviceResponse.MISACode = MISACode.NotValid;
                return _serviceResponse;
            }

            //Sửa thông tin khi dữ liệu hợp lệ:
            var rowAffects = _customerRepository.UpdateCustomer(customerId, customer);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Properties.Resources.messageInsertSuccess;
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }

        /// <summary>
        /// Xóa thông tin khách hàng
        /// </summary>
        /// <param name="customerId">ID của khách hàng cần xóa</param>
        /// <returns>Phản hồi tương ứng có xóa thành công hay không</returns>
        /// Author: NQMinh (12/08/2021)
        public ServiceResponse DeleteCustomer(List<Guid> customerId)
        {                        
            var rowAffects = _customerRepository.DeleteCustomer(customerId);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Properties.Resources.messageInsertSuccess;
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }
        #endregion
    }
}
