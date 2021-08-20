using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Repositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        /// <summary>
        /// Hàm xử lý phân trang cho khách hàng
        /// </summary>
        /// <param name="customerFilter">Dữ liệu cần lọc</param>
        /// <param name="customerGroupId">ID nhóm khách hàng</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi một trang</param>
        /// <returns>Dữ liệu phân trang</returns>
        /// Author: NQMinh (20/08/2021)
        public object Pagination(string customerFilter, Guid? customerGroupId, int pageIndex, int pageSize);
    }
}
