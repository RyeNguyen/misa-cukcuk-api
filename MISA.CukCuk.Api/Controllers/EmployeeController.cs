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
    public class EmployeesController : ControllerBase
    {
        #region Fields
        readonly IEmployeeRepository _employeeRepository;
        readonly IEmployeeService _employeeService;
        #endregion

        #region Constructor
        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeService employeeService)
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
        }
        #endregion

        #region Lấy toàn bộ dữ liệu nhân viên
        /// <summary>
        /// Phương thức lấy toàn bộ dữ liệu nhân viên
        /// </summary>       
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetEmployees()
        {            
            try
            {
                var employees = _employeeService.GetAll();

                if (employees.MISACode == MISACode.isValid)
                {
                    return StatusCode(200, employees.Data);
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

        #region Lấy dữ liệu nhân viên theo id
        /// <summary>
        /// Phương thức lấy dữ liệu nhân viên bằng id
        /// </summary>
        /// <param name="employeeId">Id của nhân viên cần lấy thông tin</param>
        /// <returns></returns>
        [HttpGet("{employeeId}")]
        public IActionResult GetEmployeeById(Guid employeeId)
        {
            try
            {
                var employee = _employeeService.GetById(employeeId);

                if (employee != null)
                {
                    return StatusCode(200, employee);
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

        #region Thêm nhân viên vào DB
        /// <summary>
        /// Phương thức thêm nhân viên vào DB
        /// </summary>
        /// <param name="employee">Dữ liệu nhân viên cần thêm</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InsertEmployee(Employee employee)
        {
            var insertResult = _employeeService.Insert(employee);

            if (insertResult.MISACode == MISACode.NotValid)
            {
                return BadRequest(insertResult.Data);
            }

            if (insertResult.MISACode == MISACode.isValid && (int)insertResult.Data > 0)
            {
                return Created(Properties.Resources.messageInsertSuccess, employee);
            }
            else
            {
                return NoContent();
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
            var updateResult = _employeeService.Update(employeeId, employee);

            if (updateResult.MISACode == MISACode.NotValid)
            {
                return BadRequest(updateResult.Data);
            }

            if (updateResult.MISACode == MISACode.isValid && (int)updateResult.Data > 0)
            {
                return Created(Properties.Resources.messageInsertSuccess, employee);
            }
            else
            {
                return NoContent();
            }
        }
        #endregion

        #region Xóa nhân viên
        /// <summary>
        /// Phương thức xóa thông tin nhân viên
        /// </summary>
        /// <param name="employeeId">ID của nhân viên</param>
        /// <returns></rethurns>
        [HttpDelete("{EmployeeId}")]
        public IActionResult DeleteCustomer(List<Guid> employeeIds)
        {
            var deleteResult = _employeeService.Delete(employeeIds);

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
