using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Entity;
using MISA.Infrastructure.Models;
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
    public class EmployeesController : BaseEntityController<Employee>
    {
        #region Declares
        readonly IEmployeeService _employeeService;
        readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Constructor
        public EmployeesController(IEmployeeService employeeService, 
            IEmployeeRepository employeeRepository) : base(employeeService, employeeRepository)
        {
            _employeeService = employeeService;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm xử lý phân trang cho nhân viên
        /// </summary>
        /// <param name="employeeFilter">Dữ liệu cần lọc</param>
        /// <param name="departmentId">ID phòng ban</param>
        /// <param name="positionId">ID vị trí</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi một trang</param>
        /// <returns>Dữ liệu phân trang</returns>
        /// Author: NQMinh (19/08/2021)
        [HttpGet("paging")]
        public IActionResult EmployeePagination([FromQuery] string employeeFilter, [FromQuery] Guid? departmentId, [FromQuery] Guid? positionId, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            try
            {
                var serviceResponse = _employeeService.Pagination(employeeFilter, departmentId, positionId, pageIndex, pageSize);

                if ((int)serviceResponse.Data.GetType().GetProperty("totalRecord").GetValue(serviceResponse.Data) != 0)
                {
                    return StatusCode(200, serviceResponse.Data);
                }
                else
                {
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    devMsg = ex.Message,
                    userMsg = "Phân trang lỗi",
                };
                return StatusCode(500, errorObj);
            }

        }
        #endregion
    }
}
