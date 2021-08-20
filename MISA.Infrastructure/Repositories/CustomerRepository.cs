using Dapper;
using MISA.Infrastructure.Models;
using MySqlConnector;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace MISA.Infrastructure
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IConfiguration configuration) : base(configuration)
        {

        }

        /// <summary>
        /// Hàm xử lý phân trang cho khách hàng
        /// </summary>
        /// <param name="customerFilter">Dữ liệu cần lọc</param>
        /// <param name="customerGroupId">ID nhóm khách hàng</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi một trang</param>
        /// <returns>Dữ liệu phân trang</returns>
        /// Author: NQMinh (20/08/2021)
        public object Pagination(string customerFilter, Guid? customerGroupId,  int pageIndex, int pageSize)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@CustomerFilter", customerFilter ?? string.Empty);
                parameters.Add("@CustomerGroupId", customerGroupId);
                parameters.Add("@PageIndex", pageIndex);
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@TotalPage", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var data = _dbConnection.Query<Customer>($"Proc_CustomerPagingAndFilter", param: parameters, commandType: CommandType.StoredProcedure);

                var totalPage = parameters.Get<int>("@TotalPage");
                var totalRecord = parameters.Get<int>("@TotalRecord");


                var pagingData = new
                {
                    totalPage,
                    totalRecord,
                    data
                };

                return pagingData;
            }
        }
    }
}
