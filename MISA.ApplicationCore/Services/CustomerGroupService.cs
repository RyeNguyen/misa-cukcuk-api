using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Services
{
    public class CustomerGroupService : BaseService<CustomerGroup>, ICustomerGroupService
    {
        private readonly ICustomerGroupRepository _customerGroupRepository;
        private readonly ServiceResponse _serviceResponse;

        public CustomerGroupService(IBaseRepository<CustomerGroup> baseRepository,
            ICustomerGroupRepository customerGroupRepository) : base(baseRepository)
        {
            _customerGroupRepository = customerGroupRepository;
            _serviceResponse = new ServiceResponse();
        }
    }
}
