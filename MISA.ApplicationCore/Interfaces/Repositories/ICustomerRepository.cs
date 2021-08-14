using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Lất tất cả khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// Author: NQMinh (14/08/2021)
        List<Customer> GetCustomers();

        /// <summary>
        /// Lấy thông tin khách hàng qua ID
        /// </summary>
        /// <param name="customerId">ID của khách hàng (khóa chính)</param>
        /// <returns>Thông tin khách hàng có ID đầu vào</returns>
        /// Author: NQMinh (14/08/2021)
        Customer GetCustomerById(Guid customerId);

        /// <summary>
        /// Thêm thông tin khách hàng
        /// </summary>
        /// <param name="customer">Thông tin khách hàng cần thêm</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh (14/08/2021)
        int InsertCustomer(Customer customer);

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="customerId">ID khách hàng (khóa chính)</param>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh (14/08/2021)
        int UpdateCustomer(Guid customerId, Customer customer);

        /// <summary>
        /// Xóa khách hàng
        /// </summary>
        /// <param name="customerIds">Danh sách ID các khách hàng cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh (14/08/2021)
        int DeleteCustomer(List<Guid> customerIds);

        /// <summary>
        /// Lấy thông tin khách hàng bằng mã khách hàng
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>Thông tin khách hàng cần lấy</returns>
        /// Author: NQMinh (14/08/2021)
        Customer GetCustomerByCode(string customerCode);
    }
}
