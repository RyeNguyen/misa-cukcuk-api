using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Api.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerGroupsController : ControllerBase
    {
        #region Lấy toàn bộ dữ liệu nhóm khách hàng
        [HttpGet]
        public IActionResult GetCustomersGroup()
        {
            var connectionString = "Host = 47.241.69.179;" +
                "Database = MF946_NQMINH_CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

            IDbConnection dbConnection = new MySqlConnection(connectionString);

            var sqlCommand = "SELECT * FROM CustomerGroup";

            try
            {
                var customersGroup = dbConnection.Query<CustomersGroup>(sqlCommand);
                return StatusCode(200, customersGroup);
            } 
            catch (Exception)
            {
                return StatusCode(500, "Không tìm thấy nhóm khách hàng.");
            }
        }
        #endregion

        #region Lấy dữ liệu nhóm khách hàng theo id
        [HttpGet("{CustomerGroupId}")]
        public IActionResult GetCustomersGroupById(Guid customerGroupId)
        {
            var connectionString = "Host = 47.241.69.179;" +
                "Database = MF946_NQMINH_CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

            var sqlCommand = $"SELECT * FROM CustomerGroup WHERE CustomerGroupId = @dynamicCustomerGroupId";

            DynamicParameters parameters = new();
            parameters.Add("@dynamicCustomerGroupId", customerGroupId);          

            IDbConnection dbConnection = new MySqlConnection(connectionString);

            try
            {
                var customersGroup = dbConnection.QueryFirstOrDefault<CustomersGroup>(sqlCommand, param: parameters);
                return StatusCode(200, customersGroup);
            }
            catch
            {
                return StatusCode(500, $"Không tìm thấy nhóm khách hàng có ID là {customerGroupId}");
            }
        }
        #endregion

        #region Thêm nhóm khách hàng
        #endregion
    }
}
