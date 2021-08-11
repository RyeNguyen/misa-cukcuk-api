using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class EmployeeController : ControllerBase
    {
        #region Fields
        private readonly IConfiguration _configuration;

        private readonly string _connectionString;

        private readonly IDbConnection _dbConnection;
        #endregion

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("MisaCukCuk");

            _dbConnection = new MySqlConnection(_connectionString);
        }

        #region Lấy toàn bộ dữ liệu nhân viên
        /// <summary>
        /// Phương thức lấy toàn bộ dữ liệu nhân viên
        /// </summary>       
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetEmployees()
        {           
            var sqlCommand = "SELECT * FROM Employee";           

            //4. Trả về cho client:
            try
            {
                var employees = _dbConnection.Query<object>(sqlCommand);                
                return StatusCode(200, employees);
            } 
            catch (Exception)
            {
                return StatusCode(500, "Không tìm thấy dữ liệu nhân viên.");
            }
        }
        #endregion

        #region Lấy dữ liệu nhân viên theo id
        /// <summary>
        /// Phương thức lấy dữ liệu nhân viên bằng id
        /// </summary>
        /// <param name="employeeId">Id của nhân viên cần lấy thông tin</param>
        /// <returns></returns>
        [HttpGet("{employeeId}")]
        public IActionResult GetEmployeeById(Guid employeeId)
        {            
            var sqlCommand = $"SELECT * FROM Employee WHERE EmployeeId = @dynamicEmployeeId";

            DynamicParameters parameters = new();
            parameters.Add("@dynamicEmployeeId", employeeId);           

            //4. Trả về cho client:
            try
            {
                var employee = _dbConnection.QueryFirstOrDefault<object>(sqlCommand, param: parameters);
                return StatusCode(200, employee);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Không tìm thấy dữ liệu nhân viên có ID là {employeeId}");
            }
        }
        #endregion

        #region Thêm nhân viên vào DB
        /// <summary>
        /// Phương thức thêm nhân viên vào DB
        /// </summary>
        /// <param name="employee">Dữ liệu nhân viên cần thêm</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertEmployee(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid();         

            //Khai báo dynamic param:
            var dynamicParams = new DynamicParameters();

            //3. Thêm dữ liệu vào trong db:
            var columnsName = string.Empty;
            var columnsParam = string.Empty;

            //Đọc từng property của object:
            var properties = employee.GetType().GetProperties();

            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //Lấy tên của prop:
                var propName = prop.Name;

                //Lấy value của prop:
                var propValue = prop.GetValue(employee);

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //Thêm param tương ứng với mỗi property của đối tượng:
                dynamicParams.Add($"@{propName}", propValue);

                columnsName += $"{propName},";
                columnsParam += $"@{propName},";
            }

            columnsName = columnsName.Remove(columnsName.Length - 1, 1);

            columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);

            var sqlCommand = $"INSERT INTO Employee({columnsName}) VALUES ({columnsParam})";          

            //4. Trả về cho client:
            try
            {
                var employeeToInsert = _dbConnection.Execute(sqlCommand, param: dynamicParams);
                return StatusCode(200, employeeToInsert);
            }
            catch (Exception)
            {
                return StatusCode(500, "Không thể thêm dữ liệu nhân viên.");
            }          
        }
        #endregion

        #region Sửa thông tin nhân viên
        /// <summary>
        /// Phương thức sửa thông tin nhân viên
        /// </summary>
        /// <param name="employeeId">ID của nhân viên</param>
        /// <param name="employee">Dữ liệu mới để sửa</param>
        /// <returns></returns>
        [HttpPut("{EmployeeId}")]
        public IActionResult UpdateCustomer(Guid employeeId, Employee employee)
        {
            //Khai báo dynamic param:
            DynamicParameters dynamicParams = new();

            var queryString = string.Empty;            

            //3. Thêm dữ liệu vào trong db:           
            //Đọc từng property của object:
            var properties = employee.GetType().GetProperties();

            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //Lấy tên của prop:
                var propName = prop.Name;

                //Lấy value của prop:
                var propValue = prop.GetValue(employee);

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //Thêm param tương ứng với mỗi property của đối tượng:
                if (propName != "EmployeeId" && propValue != null)
                {
                    dynamicParams.Add($"@{propName}", propValue);

                    queryString += $"{propName} = @{propName},";
                }
            }

            dynamicParams.Add("@ExistingId", employeeId);

            queryString = queryString.Remove(queryString.Length - 1, 1);

            var sqlCommand = $"UPDATE Employee SET {queryString} WHERE EmployeeId = @ExistingId";

            //4. Trả về cho client:
            try
            {
                var employeeToUpdate = _dbConnection.Execute(sqlCommand, param: dynamicParams);
                return StatusCode(200, employeeToUpdate);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Không thể cập nhật dữ liệu nhân viên có ID = {employeeId}.");
            }
        }
        #endregion

        #region Xóa nhân viên
        /// <summary>
        /// Phương thức xóa thông tin nhân viên
        /// </summary>
        /// <param name="employeeId">ID của nhân viên</param>
        /// <returns></returns>
        [HttpDelete("{EmployeeId}")]
        public IActionResult DeleteCustomer(Guid employeeId)
        {
            DynamicParameters parameters = new();
            parameters.Add("@dynamicEmployeeId", employeeId);

            var sqlCommand = $"DELETE FROM Employee WHERE EmployeeId = @dynamicEmployeeId";
            try
            {
                var employeeToDelete = _dbConnection.Execute(sqlCommand, param: parameters);
                return StatusCode(200, employeeToDelete);
            }
            catch (Exception)
            {
                return StatusCode(500, $"Không thể xóa nhân viên có ID = {employeeId}.");
            }
        }
        #endregion

        #region Lọc nhân viên qua mã nv, tên, sđt, phòng ban, vị trí
        //[HttpGet("filter")]
        //public IActionResult FilterEmployee([FromQuery] string keyword, [FromQuery] Guid? departmentId, [FromQuery] Guid? positionId)
        //{
        //    var connectionString = "Host = 47.241.69.179;" +
        //        "Database = MF946_NQMINH_CukCuk;" +
        //        "User Id = dev;" +
        //        "Password = 12345678";

        //    IDbConnection dbConnection = new MySqlConnection(connectionString: connectionString);

        //    DynamicParameters parameters = new();


        //}
        #endregion
    }
}
