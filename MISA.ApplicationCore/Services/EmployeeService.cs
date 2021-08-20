using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Entity;
using MISA.Infrastructure.Models;
using System;
using System.Text.RegularExpressions;

namespace MISA.ApplicationCore.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ServiceResponse _serviceResponse;

        public EmployeeService(IBaseRepository<Employee> baseRepository,
            IEmployeeRepository employeeRepository) : base(baseRepository)
        {
            _employeeRepository = employeeRepository;
            _serviceResponse = new ServiceResponse();
        }

        #region Methods
        public ServiceResponse Pagination(string employeeFilter, Guid? departmentId, Guid? positionId, int pageIndex, int pageSize)
        {
            _serviceResponse.Data = _employeeRepository.Pagination(employeeFilter, departmentId, positionId, pageIndex, pageSize);

            return _serviceResponse;
        }

        protected override bool ValidateCustom(Employee employee)
        {
            var isValid = false;

            //Check trùng mã:
            var checkCode = _employeeRepository.CheckDuplicateCode(employee.EmployeeCode);
            if (checkCode == false)
            {
                var errorObj = new
                {
                    userMsg = "Trùng mã",
                };
                isValid = false;
                _serviceResponse.MISACode = MISACode.NotValid;
                _serviceResponse.Data = errorObj;
                return isValid;
            }

            //Check Email:
            var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

            var isMatch = Regex.IsMatch(employee.Email, emailFormat, RegexOptions.IgnoreCase);

            if (isMatch == false)
            {
                var errorObj = new
                {
                    userMsg = "Lỗi email",
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = ""
                };
                isValid = false;
                _serviceResponse.MISACode = MISACode.NotValid;
                _serviceResponse.Data = errorObj;
                return isValid;
            }

            return isValid;

        }
        #endregion
    }
}
