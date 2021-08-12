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
        #region Fields
        //Khởi tạo kết nối với DB
        private readonly string _connectionString = "Host = 47.241.69.179;" +
                "Database = MF946_NQMINH_CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

        private readonly IDbConnection _dbConnection;
        #endregion

        #region Constructor
        public CustomerContext()
        {
            _dbConnection = new MySqlConnection(_connectionString);
        }
        #endregion

        #region Methods
        #region Phương thức lấy tất cả dữ liệu khách hàng
        /// <summary>
        /// Lấy toàn bộ danh sách khách hàng từ DB
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// Author: NQMinh(10/08/2021)
        public IEnumerable<Customer> GetCustomers()
        {                       
            var customers = _dbConnection.Query<Customer>("Proc_CustomersGetAll", commandType: CommandType.StoredProcedure);

            //Trả về dữ liệu:
            return customers;
        }
        #endregion

        #region Phương thức lấy thông tin khách hàng qua ID
        /// <summary>
        /// Lấy thông tin khách hàng theo ID
        /// </summary>
        /// <returns>Thông tin khách hàng cần lấy</returns>
        public Customer GetCustomerById(Guid customerId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@dynamicCustomerId", customerId);

            var sqlCommand = $"SELECT * FROM Customer WHERE CustomerId = @dynamicCustomerId";

            var customer = _dbConnection.QueryFirstOrDefault<Customer>(sqlCommand, param: parameters);

            return customer;
        }
        #endregion

        #region Phương thức thêm khách hàng vào DB
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
        #endregion

        /// <summary>
        /// Sửa thông tin khách hàng
        /// </summary>
        /// <param name="customerId">ID của khách hàng cần sửa</param>
        /// <param name="customer">Dữ liệu cần sửa của khách hàng</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh (12/08/2021)
        public int UpdateCustomer(Guid customerId, Customer customer)
        {
            //Khai báo dynamic param:
            DynamicParameters dynamicParams = new();

            var queryString = string.Empty;
   
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
                if (propName != "CustomerId" && propName != "CustomerCode" && propValue != null)
                {
                    dynamicParams.Add($"@{propName}", propValue);

                    queryString += $"{propName} = @{propName},";
                }
            }

            dynamicParams.Add("@ExistingId", customerId);

            queryString = queryString.Remove(queryString.Length - 1, 1);

            var sqlCommand = $"UPDATE Customer SET {queryString} WHERE CustomerId = @ExistingId";

            var rowAffects = _dbConnection.Execute(sqlCommand, param: dynamicParams);

            return rowAffects;
        }

        /// <summary>
        /// Xóa khách hàng khỏi DB
        /// </summary>
        /// <param name="customerId">ID của khách hàng cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh (12/08/2021)
        public int DeleteCustomer(Guid customerId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@dynamicCustomerId", customerId);

            var sqlCommand = @"DELETE FROM Customer WHERE CustomerId = @dynamicCustomerId";

            var rowAffects = _dbConnection.Execute(sqlCommand, param: parameters);

            return rowAffects;
        }

        #region Phương thức lấy thông tin khách hàng qua mã khách hàng
        /// <summary>
        /// Lấy khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>Object khách hàng đầu tiên lấy được</returns>
        /// Author: NQMinh(12/08/2021)
        public Customer GetCustomerbyCode(string customerCode)
        {
            //Khai báo dynamic param:
            var dynamicParams = new DynamicParameters();

            dynamicParams.Add("@dynamicCustomerCode", customerCode);

            var sqlCommand = $"SELECT * FROM Customer WHERE CustomerCode = @dynamicCustomerCode LIMIT 1";

            var customer = _dbConnection.QueryFirstOrDefault<Customer>(sqlCommand, param: dynamicParams);

            return customer;
        }
        #endregion
        #endregion
    }
}
