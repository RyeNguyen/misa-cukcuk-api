using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Entity;
using MISA.Infrastructure.Models;
using System;

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
        /// <summary>
        /// Hàm xử lý phân trang nhân viên
        /// </summary>
        /// <param name="employeeFilter">Dữ liệu cần lọc</param>
        /// <param name="departmentId">ID phòng ban</param>
        /// <param name="positionId">ID vị trí</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi một trang</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (19/08/2021)
        public ServiceResponse Pagination(string employeeFilter, Guid? departmentId, Guid? positionId, int pageIndex, int pageSize)
        {
            _serviceResponse.Data = _employeeRepository.Pagination(employeeFilter, departmentId, positionId, pageIndex, pageSize);

            return _serviceResponse;
        }

        /// <summary>
        /// Hàm validate thông tin nhân viên
        /// </summary>
        /// <param name="employee">Thông tin nhân viên</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (19/08/2021)
        protected override ServiceResponse ValidateCustom(Employee employee)
        {
            var checkedEmployee = _employeeRepository.GetById(employee.EmployeeId);

            //Check trùng số CMND
            var checkIdentity = _employeeRepository.CheckDuplicateIdentity(employee.IdentityNumber);
            if (checkIdentity == true && checkedEmployee == null)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.Resources.messageErrorDuplicateIdentityEm,
                    userMsg = Entity.Properties.Resources.messageErrorDuplicateIdentityEm,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Data = errorObj;
                _serviceResponse.MISACode = MISACode.NotValid;
                _serviceResponse.Message = Entity.Properties.Resources.messageErrorDuplicateIdentityEm;
                return _serviceResponse;
            }

            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }
        #endregion
    }
}
