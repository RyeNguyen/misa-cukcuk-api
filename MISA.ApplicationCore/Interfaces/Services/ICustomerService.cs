using Microsoft.AspNetCore.Http;
using MISA.ApplicationCore.Entities;
using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Services
{
    public interface ICustomerService : IBaseService<Customer>
    {
        /// <summary>
        /// Phương thức xử lý chức năng nhập khẩu dữ liệu Excel
        /// </summary>
        /// <param name="formFile">File nhập khẩu</param>
        /// <param name="cancellationToken">Token</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (20/08/2021)
        ServiceResponse Import(IFormFile formFile, CancellationToken cancellationToken);

        /// <summary>
        /// Hàm xử lý phân trang cho khách hàng
        /// </summary>
        /// <param name="customerFilter">Dữ liệu cần lọc</param>
        /// <param name="customerGroupId">ID nhóm khách hàng</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi một trang</param>
        /// <returns>Dữ liệu phân trang</returns>
        /// Author: NQMinh (20/08/2021)
        ServiceResponse Pagination(string customerFilter, Guid? customerGroupId, int pageIndex, int pageSize);
    }
}
