using MISA.Infrastructure;
using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore
{
    public class CustomerService
    {
        #region Method
        //Lấy danh sách khách hàng
        public IEnumerable<Customer> GetCustomers()
        {
            var customerContext = new CustomerContext();
            var customers = CustomerContext.GetCustomers();
            return customers;
        }

        //Thêm mới khách hàng

        //Sửa thông tin khách hàng

        //Xóa khách hàng
        #endregion
    }
}
