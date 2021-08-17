using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Repositories
{
    public interface IEmployeeRepository<MISAEntity> : IBaseRepository<MISAEntity>
    {
        ///// <summary>
        ///// Lất tất cả nhân viên
        ///// </summary>
        ///// <returns>Danh sách nhân viên</returns>
        ///// Author: NQMinh (16/08/2021)
        //List<Employee> GetAll();

        ///// <summary>
        ///// Lấy thông tin nhân viên qua ID
        ///// </summary>
        ///// <param name="employeeId">ID của nhân viên (khóa chính)</param>
        ///// <returns>Thông tin nhân viên có ID đầu vào</returns>
        ///// Author: NQMinh (16/08/2021)
        //Employee GetById(Guid employeeId);

        ///// <summary>
        ///// Thêm thông tin nhân viên
        ///// </summary>
        ///// <param name="employee">Thông tin nhân viên cần thêm</param>
        ///// <returns>Số bản ghi bị ảnh hưởng</returns>
        ///// Author: NQMinh (16/08/2021)
        //int Insert(Employee employee);

        ///// <summary>
        ///// Cập nhật thông tin nhân viên
        ///// </summary>
        ///// <param name="employeeId">ID nhân viên (khóa chính)</param>
        ///// <param name="employee">Thông tin nhân viên</param>
        ///// <returns>Số bản ghi bị ảnh hưởng</returns>
        ///// Author: NQMinh (16/08/2021)
        //int Update(Guid employeeId, Employee employee);

        ///// <summary>
        ///// Xóa nhân viên
        ///// </summary>
        ///// <param name="employeeIds">Danh sách ID các nhân viên cần xóa</param>
        ///// <returns>Số bản ghi bị ảnh hưởng</returns>
        ///// Author: NQMinh (14/08/2021)
        //int Delete(List<Guid> employeeIds);

        ///// <summary>
        ///// Lấy thông tin nhân viên bằng mã nhân viên
        ///// </summary>
        ///// <param name="employeeCode">Mã nhân viên</param>
        ///// <returns>Thông tin nhân viên cần lấy</returns>
        ///// Author: NQMinh (14/08/2021)
        //Employee GetByCode(string employeeCode);
    }
}
