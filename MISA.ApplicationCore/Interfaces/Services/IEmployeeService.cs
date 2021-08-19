using MISA.ApplicationCore.Entities;
using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Services
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        /// <summary>
        /// Phương thức xử lý phân trang cho nhân viên
        /// </summary>
        /// <param name="employeeFilter">Dữ liệu cần lọc</param>
        /// <param name="departmentId">ID phòng ban</param>
        /// <param name="positionId">ID vị trí</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi một trang</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (19/08/2021)
        ServiceResponse Pagination(string employeeFilter, Guid? departmentId, Guid? positionId, int pageIndex, int pageSize);
    }
}
