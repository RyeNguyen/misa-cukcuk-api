using Dapper;
using MISA.Infrastructure.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure
{
    public class CustomerContext
    {
        #region Method
        /// <summary>
        /// Lấy toàn bộ danh sách khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// Author: NQMinh (10/08/2021)
        public static IEnumerable<Customer> GetCustomers()
        {
            //Kết nối tới CSDL:           
            var connectionString = "Host = 47.241.69.179;" +
                "Database = MF946_NQMINH_CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";
            
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            //Khởi tạo các commandText:
            var sqlCommand = "SELECT * FROM Customer";
            var customers = dbConnection.Query<Customer>(sqlCommand);

            //Trả về dữ liệu:
            return customers;
        }

        //Lấy thông tin khách hàng theo ID khách hàng:

        //Thêm mới khách hàng:
        public int InsertCustomer()
        {
            return 1;
        }

        //Sửa thông tin khách hàng:

        //Xóa thông tin khách hàng:

        #endregion
    }
}
