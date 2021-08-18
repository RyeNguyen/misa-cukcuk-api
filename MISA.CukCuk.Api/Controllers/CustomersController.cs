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

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : BaseEntityController<Customer>
    {
        #region Declares
        ICustomerService _customerService;
        ICustomerRepository _customerRepository;
        #endregion

        #region Constructor
        public CustomersController(ICustomerService customerService, 
            ICustomerRepository customerRepository) : base(customerService, customerRepository)
        {
            _customerService = customerService;
            _customerRepository = customerRepository;
        }
        #endregion        
    }
}
