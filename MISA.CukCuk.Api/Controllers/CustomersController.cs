using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using Dapper;
using MISA.Infrastructure.Models;
using MISA.Entity;
using MISA.ApplicationCore.Interfaces.Services;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        #region Fields
        readonly ICustomerService _customerService;
        #endregion

        #region Constructor
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        #endregion

        #region Lấy toàn bộ dữ liệu khách hàng
        /// <summary>
        /// Phương thức lấy toàn bộ dữ liệu khách hàng
        /// </summary>       
        /// <returns></returns>
        [HttpGet]      
        public IActionResult GetCustomers() 
        {
            var customers = _customerService.GetCustomers();
            try
            {
                if (customers.Any())
                {
                    return StatusCode(200, customers);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception)
            {
                var errorObj = new
                {
                    devMsg = Properties.Resources.messageGetCustomers_Dev,
                    userMsg = Properties.Resources.messageGetCustomers_User,
                    Code = MISACode.NotValid
                };
                return BadRequest(errorObj);
            }
        }
        #endregion

        #region Lấy dữ liệu khách hàng theo id
        /// <summary>
        /// Phương thức lấy dữ liệu nhân viên bằng id
        /// </summary>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns></returns>
        [HttpGet("{customerId}")]
        public IActionResult GetById(Guid customerId)
        {                        
            var customer = _customerService.GetCustomerById(customerId);
           
            try
            {
                if (customer != null)
                {
                    return StatusCode(200, customer);
                }
                else
                {                    
                    return NoContent();
                }
            } 
            catch (Exception)
            {
                var errorObj = new
                {
                    devMsg = Properties.Resources.messageGetCustomerById_Dev,
                    userMsg = Properties.Resources.messageGetCustomerById_User,
                    Code = MISACode.NotValid
                };
                return BadRequest(errorObj);
            }
        }
        #endregion

        #region Thêm khách hàng
        [HttpPost]
        public IActionResult InsertCustomer(Customer customer)
        {            
            var insertResult = _customerService.InsertCustomer(customer);

            if (insertResult.MISACode == MISACode.NotValid)
            {
                return BadRequest(insertResult.Data);
            }

            if (insertResult.MISACode == MISACode.isValid && (int)insertResult.Data > 0)
            {
                return Created(Properties.Resources.messageInsertSuccess, customer);
            } 
            else
            {
                return NoContent();
            }
        }
        #endregion

        #region Sửa thông tin khách hàng 
        [HttpPut("{CustomerId}")]
        public IActionResult UpdateCustomer(Guid customerId, Customer customer)
        {            
            var updateResult = _customerService.UpdateCustomer(customerId, customer);

            if (updateResult.MISACode == MISACode.NotValid)
            {
                return BadRequest(updateResult.Data);
            }

            if (updateResult.MISACode == MISACode.isValid && (int)updateResult.Data > 0)
            {
                return Created(Properties.Resources.messageInsertSuccess, customer);
            }
            else
            {
                return NoContent();
            }
        }
        #endregion

        #region Xóa khách hàng
        [HttpDelete("{CustomerId}")]
        public IActionResult DeleteCustomer(List<Guid> customerIds)
        {           
            var deleteResult = _customerService.DeleteCustomer(customerIds);

            if (deleteResult.MISACode == MISACode.NotValid)
            {
                return BadRequest(deleteResult.Data);
            }

            if (deleteResult.MISACode == MISACode.isValid && (int)deleteResult.Data > 0)
            {
                return StatusCode(200, Properties.Resources.messageInsertSuccess);
            }
            else
            {
                return NoContent();
            }
        }
        #endregion
    }
}
