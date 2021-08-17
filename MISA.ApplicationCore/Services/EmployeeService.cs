using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Entity;
using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        //readonly IEmployeeRepository _employeeRepository;
        private readonly ServiceResponse _serviceResponse;

        public EmployeeService(IBaseRepository<Employee> baseRepository) : base(baseRepository)
        {
            //_employeeRepository = employeeRepository;
            _serviceResponse = new ServiceResponse();
        }
    }
}
