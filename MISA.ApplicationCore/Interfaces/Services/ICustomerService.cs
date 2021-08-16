using MISA.ApplicationCore.Entities;
using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Services
{
    public interface ICustomerService
    {
        /// <summary>
        /// Lấy toàn bộ danh sách khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// Author: NQMinh (14/08/2021)
        List<Customer> GetAll();

        /// <summary>
        /// Lấy thông tin khách hàng qua ID
        /// </summary>
        /// <param name="customerId">ID khách hàng (khóa chính)</param>
        /// <returns>Thông tin khách hàng</returns>
        /// Author: NQMinh (14/08/2021)
        Customer GetById(Guid customerId);

        /// <summary>
        /// Thêm mơi khách hàng
        /// </summary>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns>Kết quả xử lý qua nghiệp vụ</returns>
        /// Author: NQMinh (14/08/2021)
        ServiceResponse Insert(Customer customer);

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="customerId">ID khách hàng (khóa chính)</param>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns>Kết quả xử lý qua nghiệp vụ</returns>
        /// Author: NQMinh (14/08/2021)
        ServiceResponse Update(Guid customerId, Customer customer);

        /// <summary>
        /// Xóa thông tin khách hàng
        /// </summary>
        /// <param name="customerIds">Danh sách ID khách hàng cần xóa</param>
        /// <returns>Kết quả xử lý qua nghiệp vụ</returns>
        /// Author: NQMinh (14/08/2021)
        ServiceResponse Delete(List<Guid> customerIds);
    }
}
