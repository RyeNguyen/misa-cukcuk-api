﻿using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.Infrastructure.Models;
using MySqlConnector;
using System;
using System.Data;

namespace MISA.Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {

        }

        /// <summary>
        /// Hàm xử lý phân trang cho nhân viên
        /// </summary>
        /// <param name="employeeFilter">Dữ liệu cần lọc</param>
        /// <param name="departmentId">ID phòng ban</param>
        /// <param name="positionId">ID vị trí</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Số bản ghi một trang</param>
        /// <returns>Dữ liệu phân trang</returns>
        /// Author: NQMinh (19/08/2021)
        public object Pagination(string employeeFilter, Guid? departmentId, Guid? positionId, int pageIndex, int pageSize)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();

                parameters.Add("@EmployeeFilter", employeeFilter ?? string.Empty);
                parameters.Add("@DepartmentId", departmentId);
                parameters.Add("@PositionId", positionId);
                parameters.Add("@PageIndex", pageIndex);
                parameters.Add("@PageSize", pageSize);

                parameters.Add("@TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@TotalPage", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var data = _dbConnection.Query<Employee>($"Proc_EmployeePagingAndFilter", param: parameters, commandType: CommandType.StoredProcedure);

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

        /// <summary>
        /// Hàm kiểm tra nhân viên có trùng số CMND hay không
        /// </summary>
        /// <param name="identityNumber">Số CMND</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (24/08/2021)
        public bool CheckDuplicateIdentity(string identityNumber)
        {
            using (_dbConnection = new MySqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();

                parameters.Add($"@IdentityNumber", identityNumber);

                parameters.Add("@AlreadyExist", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                _dbConnection.Execute($"Proc_EmployeeCheckDuplicateIdentity", param: parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<bool>("@AlreadyExist");
            }
        }
    }
}
