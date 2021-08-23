using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
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
    public class CustomerGroupsController : BaseEntityController<CustomerGroup>
    {
        #region Declares
        private readonly ICustomerGroupService _customerGroupService;
        private readonly ICustomerGroupRepository _customerGroupRepository;
        #endregion

        #region Constructor
        public CustomerGroupsController(ICustomerGroupService customerGroupService,
            ICustomerGroupRepository customerGroupRepository) : base(customerGroupService, customerGroupRepository)
        {
            _customerGroupService = customerGroupService;
            _customerGroupRepository = customerGroupRepository;
        }
        #endregion
    }
}
