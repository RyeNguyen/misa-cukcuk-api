using Dapper;
using MISA.ApplicationCore.Interfaces.Repositories;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repositories
{
    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity>
    {
        #region Fields
        //Khởi tạo kết nối với DB
        private readonly string _connectionString = "Host = 47.241.69.179;" +
                "Database = MF946_NQMINH_CukCuk;" +
                "User Id = dev;" +
                "Password = 12345678";

        private readonly IDbConnection _dbConnection;
        private readonly string className = typeof(MISAEntity).Name;
        #endregion

        #region Constructor
        public BaseRepository()
        {
            _dbConnection = new MySqlConnection(_connectionString);
        }
        #endregion

        #region Methods
        #region Phương thức lấy tất cả dữ liệu của thực thể
        /// <summary>
        /// Lấy toàn bộ danh sách thực thể từ DB
        /// </summary>
        /// <returns>Danh sách thực thể</returns>
        /// Author: NQMinh(16/08/2021)
        public List<MISAEntity> GetAll()
        {
            var entities = _dbConnection.Query<MISAEntity>($"Proc_{className}GetAll", commandType: CommandType.StoredProcedure);

            //Trả về dữ liệu:
            return entities.ToList();
        }
        #endregion

        #region Phương thức lấy thông tin thực thể qua ID
        /// <summary>
        /// Lấy thông tin thực thể theo ID
        /// </summary>
        /// <param name="entityId">ID thực thể</param>
        /// <returns>Thông tin thực thể cần lấy</returns>
        /// Author: NQMinh(16/08/2021)
        public MISAEntity GetById(Guid entityId)
        {
            var parameters = new DynamicParameters();

            parameters.Add($"@{className}Id", entityId);

            var entity = _dbConnection.QueryFirstOrDefault<MISAEntity>($"Proc_{className}GetById", param: parameters, commandType: CommandType.StoredProcedure);

            return entity;
        }
        #endregion

        #region Phương thức lấy thông tin thực thể qua mã
        /// <summary>
        /// Lấy thực thể theo mã
        /// </summary>
        /// <param name="entityCode">Mã thực thể</param>
        /// <returns>Thực thể đầu tiên lấy được</returns>
        /// Author: NQMinh(16/08/2021)
        public MISAEntity GetByCode(string entityCode)
        {
            var dynamicParams = new DynamicParameters();

            var storeName = $"Proc_{className}GetByCode";

            dynamicParams.Add($"@{className}Code", entityCode);

            var entity = _dbConnection.QueryFirstOrDefault<MISAEntity>(storeName, param: dynamicParams, commandType: CommandType.StoredProcedure);

            return entity;
        }
        #endregion

        #region Phương thức thêm thực thể vào DB
        /// <summary>
        /// Thêm mới thực thể
        /// </summary>
        /// <param name="entity">thông tin thực thể</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh(16/08/2021)
        public int Insert(MISAEntity entity)
        {
            //Khai báo dynamic param:
            var dynamicParams = new DynamicParameters();
            var newId = Guid.NewGuid();

            var columnsName = string.Empty;
            var columnsParam = string.Empty;

            //Đọc từng property của object:
            var properties = entity.GetType().GetProperties();

            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //Lấy tên của prop:
                var propName = prop.Name;

                //Lấy value của prop:
                var propValue = prop.GetValue(entity);

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //Thêm param tương ứng với mỗi property của đối tượng:
                //if (propName == $"{className}Id")
                //{
                //    dynamicParams.Add($"@{propName}", newId);
                //} 
                //else
                //{
                //    dynamicParams.Add($"@{propName}", propValue);
                //}

                dynamicParams.Add($"@{propName}", propValue);

                columnsName += $"{propName},";
                columnsParam += $"@{propName},";
            }

            //Loại ký tự thừa ở cuối khỏi hai cột:
            //columnsName = columnsName.Remove(columnsName.Length - 1, 1);
            //columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);

            var rowAffects = _dbConnection.Execute($"Proc_{className}Insert", param: dynamicParams, commandType: CommandType.StoredProcedure);

            //Trả về số bản ghi thêm mới:
            return rowAffects;
        }
        #endregion

        /// <summary>
        /// Sửa thông tin thực thể
        /// </summary>
        /// <param name="entityId">ID của thực thể cần sửa</param>
        /// <param name="entity">Dữ liệu cần sửa của thực thể</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh (16/08/2021)
        public int Update(Guid entityId, MISAEntity entity)
        {
            //Khai báo dynamic param:
            DynamicParameters dynamicParams = new();

            //var queryString = string.Empty;

            //Đọc từng property của object:
            var properties = entity.GetType().GetProperties();

            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //Lấy tên của prop:
                var propName = prop.Name;

                //Lấy value của prop:
                var propValue = prop.GetValue(entity);

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //Thêm param tương ứng với mỗi property của đối tượng:
                if (propName != $"{className}Id" && propName != $"{className}Code" && propValue != null)
                {
                    dynamicParams.Add($"@{propName}", propValue);

                    //queryString += $"{propName} = @{propName},";
                }
            }

            dynamicParams.Add($"@{className}Id", entityId);

            //queryString = queryString.Remove(queryString.Length - 1, 1);

            var rowAffects = _dbConnection.Execute($"Proc_{className}Update", param: dynamicParams, commandType: CommandType.StoredProcedure);

            return rowAffects;
        }

        /// <summary>
        /// Xóa thực thể khỏi DB
        /// </summary>
        /// <param name="entityIds">Danh sách ID của thực thể cần xóa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Author: NQMinh (16/08/2021)
        public int Delete(List<Guid> entityIds)
        {
            var parameters = new DynamicParameters();

            parameters.Add($"@{className}Ids", entityIds);

            var rowAffects = _dbConnection.Execute($"Proc_{className}Delete", param: parameters, commandType: CommandType.StoredProcedure);

            return rowAffects;
        }              
        #endregion
    }
}
