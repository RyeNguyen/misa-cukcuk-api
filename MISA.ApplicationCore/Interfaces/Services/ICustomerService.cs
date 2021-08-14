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
        List<Customer> GetCustomers();

        Customer GetCustomerById(Guid customerId);

        /// <summary>
        /// Thêm mơi khách hàng
        /// </summary>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns>Kết quả xử lý qua nghiệp vụ</returns>
        /// Author: NQMinh (14/08/2021)
        ServiceResponse InsertCustomer(Customer customer);

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="customerId">ID khách hàng (khóa chính)</param>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns>Kết quả xử lý qua nghiệp vụ</returns>
        /// Author: NQMinh (14/08/2021)
        ServiceResponse UpdateCustomer(Guid customerId, Customer customer);

        ServiceResponse DeleteCustomer(List<Guid> customerIds);
    }
}
