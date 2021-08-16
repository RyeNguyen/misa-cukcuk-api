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
    public class EmployeeService : IEmployeeService
    {
        readonly IEmployeeRepository _employeeRepository;
        readonly ServiceResponse _serviceResponse;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _serviceResponse = new ServiceResponse();
        }

        #region Method
        #region Phương thức lấy tất cả dữ liệu nhân viên
        /// <summary>
        /// Lấy danh sách nhân viên từ DB
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        public List<Employee> GetAll()
        {
            var employees = _employeeRepository.Get();
            return employees;
        }
        #endregion

        #region Phương thức lấy dữ liệu nhân viên theo ID
        public Employee GetById(Guid employeeId)
        {
            var employee = _employeeRepository.GetById(employeeId);
            return employee;
        }
        #endregion

        #region Phương thức thêm dữ liệu nhân viên vào DB
        /// <summary>
        /// Thêm mới nhân viên vào DB
        /// </summary>
        /// <param name="employee">Dữ liệu nhân viên muốn thêm</param>
        /// <returns>Phản hồi tương ứng có thêm thành công hay không</returns>
        /// Author: NQMinh (16/08/2021)
        public ServiceResponse Insert(Employee employee)
        {
            //Validate dữ liệu, nếu dữ liệu chưa hợp lệ thì trả về mô tả lỗi:          
            //Check trường bắt buộc nhập:
            var employeeCode = employee.EmployeeCode;
            if (string.IsNullOrEmpty(employeeCode))
            {
                var msg = new
                {
                    devMsg = new
                    {
                        fieldName = "EmployeeCode",
                        msg = Properties.Resources.messageCheckRequired_Dev
                    },
                    userMsg = Properties.Resources.messageCheckRequired_User,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Data = msg;
                _serviceResponse.Message = Properties.Resources.messageCheckRequired_Dev;
                _serviceResponse.MISACode = MISACode.NotValid;
                return _serviceResponse;
            }

            //Check trùng mã: 
            var employeeToCheck = _employeeRepository.GetByCode(employeeCode);
            if (employeeToCheck != null)
            {
                var msg = new
                {
                    devMsg = new
                    {
                        fieldName = "EmployeeCode",
                        msg = Properties.Resources.messageCheckCodeDuplicate_Dev
                    },
                    userMsg = Properties.Resources.messageCheckCodeDuplicate_User,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Data = msg;
                _serviceResponse.Message = Properties.Resources.messageCheckCodeDuplicate_Dev;
                _serviceResponse.MISACode = MISACode.NotValid;
                return _serviceResponse;
            }

            //Thêm mới khi dữ liệu hợp lệ:
            var rowAffects = _employeeRepository.Insert(employee);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Properties.Resources.messageInsertSuccess;
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }
        #endregion

        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        /// <param name="employeeId">ID của nhân viên cần cập nhật</param>
        /// <param name="employee">Dữ liệu để thay đổi</param>
        /// <returns>Phản hồi tương ứng có cập nhật thành công hay không</returns>
        /// Author: NQMinh (16/08/2021)
        public ServiceResponse Update(Guid employeeId, Employee employee)
        {
            //Validate dữ liệu, nếu dữ liệu chưa hợp lệ thì trả về mô tả lỗi:          
            //Check trùng mã: 
            var employeeCode = employee.EmployeeCode;

            var employeeToCheck = _employeeRepository.GetByCode(employeeCode);
            if (employeeToCheck != null)
            {
                var msg = new
                {
                    devMsg = new
                    {
                        fieldName = "EmployeeCode",
                        msg = Properties.Resources.messageCheckCodeDuplicate_Dev
                    },
                    userMsg = Properties.Resources.messageCheckCodeDuplicate_User,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Data = msg;
                _serviceResponse.Message = Properties.Resources.messageCheckCodeDuplicate_Dev;
                _serviceResponse.MISACode = MISACode.NotValid;
                return _serviceResponse;
            }

            //Sửa thông tin khi dữ liệu hợp lệ:
            var rowAffects = _employeeRepository.Update(employeeId, employee);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Properties.Resources.messageInsertSuccess;
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }

        /// <summary>
        /// Xóa thông tin nhân viên
        /// </summary>
        /// <param name="employeeId">ID của nhân viên cần xóa</param>
        /// <returns>Phản hồi tương ứng có xóa thành công hay không</returns>
        /// Author: NQMinh (16/08/2021)
        public ServiceResponse Delete(List<Guid> employeeIds)
        {
            var rowAffects = _employeeRepository.Delete(employeeIds);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Properties.Resources.messageInsertSuccess;
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }        
        #endregion
    }
}
