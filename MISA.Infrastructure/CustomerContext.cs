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
        private string _connectionString = "Host = 47.241.69.179;" +
                "Database = MF946_NQMINH_CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

        private readonly IDbConnection _dbConnection;

        public CustomerContext()
        {
            _dbConnection = new MySqlConnection(_connectionString);
        }

        #region Method
        /// <summary>
        /// Lấy toàn bộ danh sách khách hàng từ DB
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// Author: NQMinh(10/08/2021)
        public IEnumerable<Customer> GetCustomers()
        {
            //Khởi tạo các commandText:
            var sqlCommand = "SELECT * FROM Customer";
            var customers = _dbConnection.Query<Customer>(sqlCommand);

            //Trả về dữ liệu:
            return customers;
        }

        //Lấy thông tin khách hàng theo ID khách hàng:

        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="customer">Object khách hàng</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh(12/08/2021)
        public int InsertCustomer(Customer customer)
        {                                        
            //Khai báo dynamic param:
            var dynamicParams = new DynamicParameters();            

            customer.CustomerId = Guid.NewGuid();
            
            var columnsName = string.Empty;
            var columnsParam = string.Empty;

            //Đọc từng property của object:
            var properties = customer.GetType().GetProperties();

            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //Lấy tên của prop:
                var propName = prop.Name;

                //Lấy value của prop:
                var propValue = prop.GetValue(customer);

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //Thêm param tương ứng với mỗi property của đối tượng:
                dynamicParams.Add($"@{propName}", propValue);

                columnsName += $"{propName},";
                columnsParam += $"@{propName},";
            }
            
            //Loại ký tự thừa ở cuối khỏi hai cột:
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);
            columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);

            var sqlCommand = $"INSERT INTO Customer({columnsName}) VALUES ({columnsParam})";

            var rowAffects = _dbConnection.Execute(sqlCommand, param: dynamicParams);

            //Trả về số bản ghi thêm mới:
            return rowAffects;
        }    

        //Sửa thông tin khách hàng:

        //Xóa thông tin khách hàng:

        /// <summary>
        /// Lấy khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>Object khách hàng đầu tiên lấy được</returns>
        /// Author: NQMinh(12/08/2021)
        public object GetCustomerbyCode(string customerCode)
        {
            //Khai báo dynamic param:
            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("@dynamicCustomerCode", customerCode);

            var sqlCommand = $"SELECT * FROM Customer WHERE CustomerCode = @dynamicCustomerCode LIMIT 1";

            var customer = _dbConnection.Query(sqlCommand, param: dynamicParams).FirstOrDefault();

            return customer;
        }
        #endregion
    }
}
