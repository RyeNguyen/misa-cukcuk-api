using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using Dapper;
using MISA.ApplicationCore;
using MISA.Infrastructure.Models;
using MISA.Entity;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        #region Lấy toàn bộ dữ liệu khách hàng
        /// <summary>
        /// Phương thức lấy toàn bộ dữ liệu khách hàng
        /// </summary>       
        /// <returns></returns>
        [HttpGet]      
        public IActionResult GetCustomers() 
        {
            var customerService = new CustomerService();
            var customers = customerService.GetCustomers();
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
            var customerService = new CustomerService();
            var customer = customerService.GetCustomerById(customerId);
           
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
            var customerService = new CustomerService();
            var insertResult = customerService.InsertCustomer(customer);

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
            var customerService = new CustomerService();
            var updateResult = customerService.UpdateCustomer(customerId, customer);

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
        public IActionResult DeleteCustomer(Guid customerId)
        {
            var connectionString = "Host = 47.241.69.179;" +
                "Database = MF946_NQMINH_CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

            IDbConnection dbConnection = new MySqlConnection(connectionString: connectionString);

            DynamicParameters parameters = new();
            parameters.Add("@CustomerId", customerId);

            var sqlCommand = $"DELETE FROM Customer WHERE CustomerId = @CustomerId";
            var rowAffects = dbConnection.Execute(sqlCommand, param: parameters);

            return StatusCode(200, rowAffects);
        }
        #endregion
    }
}
