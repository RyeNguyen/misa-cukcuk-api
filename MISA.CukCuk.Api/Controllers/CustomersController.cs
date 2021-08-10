using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using Dapper;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //GET, POST, PUT, DELETE

        #region Lấy toàn bộ dữ liệu khách hàng
        /// <summary>
        /// Phương thức lấy toàn bộ dữ liệu khách hàng
        /// </summary>       
        /// <returns></returns>
        [HttpGet]      
        public IActionResult GetCustomers() 
        {
            //Truy cập vào database
            //1. Khai báo thông tin database:
            var connectionString = "Host = 47.241.69.179;" +
                "Database = MF946_NQMINH_CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

            //2. Khởi tạo đối tượng kết nối với database:
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //3. Lấy dữ liệu:
            var sqlCommand = "SELECT * FROM Customer";           
            var customers = dbConnection.Query<object>(sqlCommand);

            //4. Trả về cho client:
            var response = StatusCode(200, customers);
            return response;
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
            //Truy cập vào database
            //1. Khai báo thông tin database:
            var connectionString = "Host = 47.241.69.179;" +
                "Database = MF946_NQMINH_CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

            //2. Khởi tạo đối tượng kết nối với database:
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //3. Lấy dữ liệu:
            var sqlCommand = $"SELECT * FROM Customer WHERE CustomerId = @CustomerIdParam";

            DynamicParameters parameters = new();
            parameters.Add("@CustomerIdParam", customerId);

            var customer = dbConnection.QueryFirstOrDefault<object>(sqlCommand, param: parameters);

            //4. Trả về cho client:
            var response = StatusCode(200, customer);
            return response;
        }
        #endregion

        #region Thêm khách hàng
        [HttpPost]
        public IActionResult InsertCustomer(Customer customer)
        {
            customer.CustomerId = Guid.NewGuid();

            //Truy cập vào database
            //1. Khai báo thông tin database:
            var connectionString = "Host = 47.241.69.179;" +
                "Database = MF946_NQMINH_CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

            //2. Khởi tạo đối tượng kết nối với database:
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //Khai báo dynamic param:
            var dynamicParams = new DynamicParameters();

            //3. Thêm dữ liệu vào trong db:
            var columnsName = string.Empty;
            var columnsParam = string.Empty;

            //Đọc từng property của object:
            var properties = customer.GetType().GetProperties();

            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //Lấy tên của prop:
                var propName = prop.Name;

                //Lấy value của prop:
                var propValue = prop.GetValue(customer);

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //Thêm param tương ứng với mỗi property của đối tượng:
                dynamicParams.Add($"@{propName}", propValue);

                columnsName += $"{propName},";
                columnsParam += $"@{propName},";
            }

            columnsName = columnsName.Remove(columnsName.Length - 1, 1);
            columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);
            var sqlCommand = $"INSERT INTO Customer({columnsName}) VALUES ({columnsParam})";          

            var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParams);

            //4. Trả về cho client:
            var response = StatusCode(200, rowEffects);
            return response;           
        }
        #endregion

        #region Sửa thông tin khách hàng 
        [HttpPut("{CustomerId}")]
        public IActionResult UpdateCustomer(Guid customerId, Customer customer)
        {
            //Khai báo dynamic param:
            DynamicParameters dynamicParams = new();

            var queryString = string.Empty;

            //Truy cập vào database
            //1. Khai báo thông tin database:
            var connectionString = "Host = 47.241.69.179;" +
                "Database = MF946_NQMINH_CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

            //2. Khởi tạo đối tượng kết nối với database:
            IDbConnection dbConnection = new MySqlConnection(connectionString);          

            //3. Thêm dữ liệu vào trong db:           
            //Đọc từng property của object:
            var properties = customer.GetType().GetProperties();

            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //Lấy tên của prop:
                var propName = prop.Name;

                //Lấy value của prop:
                var propValue = prop.GetValue(customer);

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //Thêm param tương ứng với mỗi property của đối tượng:
                if (propName != "CustomerId" && propName != "CustomerCode" && propValue != null)
                {
                    dynamicParams.Add($"@{propName}", propValue);

                    queryString += $"{propName} = @{propName},";
                }               
            }

            dynamicParams.Add("@ExistingId", customerId);

            queryString = queryString.Remove(queryString.Length - 1, 1);

            var sqlCommand = $"UPDATE Customer SET {queryString} WHERE CustomerId = @ExistingId";

            var rowAffects = dbConnection.Execute(sqlCommand, param: dynamicParams);

            //4. Trả về cho client:
            var response = StatusCode(200, rowAffects);
            return response;
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
