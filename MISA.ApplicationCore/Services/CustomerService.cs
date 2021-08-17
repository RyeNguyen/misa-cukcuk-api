using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.ApplicationCore.Services;
using MISA.Entity;
using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace MISA.ApplicationCore
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        //readonly ICustomerRepository _customerRepository;
        readonly ServiceResponse _serviceResponse;

        public CustomerService(IBaseRepository<Customer> baseRepository) : base(baseRepository)
        {
            //_customerRepository = customerRepository;
            _serviceResponse = new ServiceResponse();
        }
    }
}
