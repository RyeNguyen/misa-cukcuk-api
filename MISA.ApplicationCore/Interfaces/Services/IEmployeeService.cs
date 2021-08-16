using MISA.ApplicationCore.Entities;
using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Services
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Lấy toàn bộ danh sách nhân viên
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        /// Author: NQMinh (16/08/2021)
        List<Employee> GetAll();

        /// <summary>
        /// Lấy thông tin nhân viên qua ID
        /// </summary>
        /// <param name="employeeId">ID nhân viên (khóa chính)</param>
        /// <returns>Thông tin nhân viên</returns>
        /// Author: NQMinh (16/08/2021)
        Employee GetById(Guid employeeId);

        /// <summary>
        /// Thêm thông tin nhân viên vào DB
        /// </summary>
        /// <param name="employee">Thông tin nhân viên cần thêm</param>
        /// <returns>Kết quả xử lý qua nghiệp vụ</returns>
        /// Author: NQMinh (16/08/2021)
        ServiceResponse Insert(Employee employee);

        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        /// <param name="employeeId">ID nhân viên cần sửa</param>
        /// <param name="employee">Thông tin nhân viên mới</param>
        /// <returns>Kết quả xử lý qua nghiệp vụ</returns>
        /// Author: NQMinh (16/08/2021)
        ServiceResponse Update(Guid employeeId, Employee employee);

        /// <summary>
        /// Xóa thông tin nhân viên khỏi DB
        /// </summary>
        /// <param name="employeeIds">Danh sách ID nhân viên cần xóa</param>
        /// <returns>Kết quả xử lý qua nghiệp vụ</returns>
        /// Author: NQMinh (16/08/2021)
        ServiceResponse Delete(List<Guid> employeeIds);
    }
}
