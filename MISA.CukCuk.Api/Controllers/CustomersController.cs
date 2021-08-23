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
using MISA.ApplicationCore.Interfaces.Repositories;
using System.Threading;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : BaseEntityController<Customer>
    {
        #region Declares
        private readonly ICustomerService _customerService;
        private readonly ICustomerRepository _customerRepository;
        #endregion

        #region Constructor
        public CustomersController(ICustomerService customerService, 
            ICustomerRepository customerRepository) : base(customerService, customerRepository)
        {
            _customerService = customerService;
            _customerRepository = customerRepository;
        }

        [HttpPost("import")]
        public IActionResult Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            try
            {
                var importTest = _customerService.Import(formFile, cancellationToken);
                if (importTest.MISACode == MISACode.isValid)
                {
                    return StatusCode(200, importTest.Data);
                }
                else
                {
                    return BadRequest(importTest.Data);
                }
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    userMsg = ex.Message,
                };
                return StatusCode(500, errorObj);
            }
        }

        [HttpGet("paging")]
        public IActionResult CustomerPagination([FromQuery] string customerFilter, [FromQuery] Guid? customerGroupId, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            try
            {
                var serviceResponse = _customerService.Pagination(customerFilter, customerGroupId, pageIndex, pageSize);

                if ((int)serviceResponse.Data.GetType().GetProperty("totalRecord").GetValue(serviceResponse.Data) != 0)
                {
                    return StatusCode(200, serviceResponse.Data);
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
                    devMsg = Entity.Properties.Resources.messageErrorPagingFilterCus,
                    userMsg = Entity.Properties.Resources.messageErrorPagingFilterCus
                };
                return StatusCode(500, errorObj);
            }

        }
        #endregion        
    }
}
