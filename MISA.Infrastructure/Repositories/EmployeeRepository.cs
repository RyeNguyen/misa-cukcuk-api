using Dapper;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.Infrastructure.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        //#region Fields
        ////Khởi tạo kết nối với DB
        //private readonly string _connectionString = "Host = 47.241.69.179;" +
        //        "Database = MF946_NQMINH_CukCuk;" +
        //        "User Id = dev;" +
        //        "Password = 12345678";

        //private readonly IDbConnection _dbConnection;
        //#endregion

        //#region Constructor
        //public EmployeeRepository()
        //{
        //    _dbConnection = new MySqlConnection(_connectionString);
        //}
        //#endregion

        //#region Methods
        //#region Phương thức lấy tất cả dữ liệu nhân viên
        ///// <summary>
        ///// Lấy toàn bộ danh sách nhân viên từ DB
        ///// </summary>
        ///// <returns>Danh sách nhân viên</returns>
        ///// Author: NQMinh(10/08/2021)
        //public List<Employee> GetAll()
        //{
        //    var employees = _dbConnection.Query<Employee>("Proc_EmployeesGetAll", commandType: CommandType.StoredProcedure);

        //    //Trả về dữ liệu:
        //    return employees.ToList();
        //}
        //#endregion

        //#region Phương thức lấy thông tin nhân viên qua ID
        ///// <summary>
        ///// Lấy thông tin nhân viên theo ID
        ///// </summary>
        ///// <returns>Thông tin nhân viên cần lấy</returns>
        //public Employee GetById(Guid employeeId)
        //{
        //    var parameters = new DynamicParameters();

        //    parameters.Add("@dynamicEmployeeId", employeeId);

        //    var sqlCommand = $"SELECT * FROM Employee WHERE EmployeeId = @dynamicEmployeeId";

        //    var employee = _dbConnection.QueryFirstOrDefault<Employee>(sqlCommand, param: parameters);

        //    return employee;
        //}
        //#endregion

        //#region Phương thức lấy thông tin nhân viên qua mã nhân viên
        ///// <summary>
        ///// Lấy nhân viên theo mã nhân viên
        ///// </summary>
        ///// <param name="employeeCode">Mã nhân viên</param>
        ///// <returns>Object nhân viên đầu tiên lấy được</returns>
        ///// Author: NQMinh(16/08/2021)
        //public Employee GetByCode(string employeeCode)
        //{
        //    //Khai báo dynamic param:
        //    var dynamicParams = new DynamicParameters();

        //    dynamicParams.Add("@dynamicEmployeeCode", employeeCode);

        //    var sqlCommand = $"SELECT * FROM Employee WHERE EmployeeCode = @dynamicEmployeeCode LIMIT 1";

        //    var employee = _dbConnection.QueryFirstOrDefault<Employee>(sqlCommand, param: dynamicParams);

        //    return employee;
        //}
        //#endregion

        //#region Phương thức thêm nhân viên vào DB
        ///// <summary>
        ///// Thêm mới nhân viên
        ///// </summary>
        ///// <param name="employee">Object nhân viên</param>
        ///// <returns>Số bản ghi bị ảnh hưởng</returns>
        ///// Author: NQMinh(16/08/2021)
        //public int Insert(Employee employee)
        //{
        //    //Khai báo dynamic param:
        //    var dynamicParams = new DynamicParameters();

        //    employee.EmployeeId = Guid.NewGuid();

        //    var columnsName = string.Empty;
        //    var columnsParam = string.Empty;

        //    //Đọc từng property của object:
        //    var properties = employee.GetType().GetProperties();

        //    //Duyệt từng property:
        //    foreach (var prop in properties)
        //    {
        //        //Lấy tên của prop:
        //        var propName = prop.Name;

        //        //Lấy value của prop:
        //        var propValue = prop.GetValue(employee);

        //        //Lấy kiểu dữ liệu của prop:
        //        var propType = prop.PropertyType;

        //        //Thêm param tương ứng với mỗi property của đối tượng:
        //        dynamicParams.Add($"@{propName}", propValue);

        //        columnsName += $"{propName},";
        //        columnsParam += $"@{propName},";
        //    }

        //    //Loại ký tự thừa ở cuối khỏi hai cột:
        //    columnsName = columnsName.Remove(columnsName.Length - 1, 1);
        //    columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);

        //    var sqlCommand = $"INSERT INTO Employee({columnsName}) VALUES ({columnsParam})";

        //    var rowAffects = _dbConnection.Execute(sqlCommand, param: dynamicParams);

        //    //Trả về số bản ghi thêm mới:
        //    return rowAffects;
        //}
        //#endregion

        ///// <summary>
        ///// Sửa thông tin nhân viên
        ///// </summary>
        ///// <param name="employeeId">ID của nhân viên cần sửa</param>
        ///// <param name="employee">Dữ liệu cần sửa của nhân viên</param>
        ///// <returns>Số bản ghi bị ảnh hưởng</returns>
        ///// Author: NQMinh (16/08/2021)
        //public int Update(Guid employeeId, Employee employee)
        //{
        //    //Khai báo dynamic param:
        //    DynamicParameters dynamicParams = new();

        //    var queryString = string.Empty;

        //    //Đọc từng property của object:
        //    var properties = employee.GetType().GetProperties();

        //    //Duyệt từng property:
        //    foreach (var prop in properties)
        //    {
        //        //Lấy tên của prop:
        //        var propName = prop.Name;

        //        //Lấy value của prop:
        //        var propValue = prop.GetValue(employee);

        //        //Lấy kiểu dữ liệu của prop:
        //        var propType = prop.PropertyType;

        //        //Thêm param tương ứng với mỗi property của đối tượng:
        //        if (propName != "EmployeeId" && propName != "EmployeeCode" && propValue != null)
        //        {
        //            dynamicParams.Add($"@{propName}", propValue);

        //            queryString += $"{propName} = @{propName},";
        //        }
        //    }

        //    dynamicParams.Add("@ExistingId", employeeId);

        //    queryString = queryString.Remove(queryString.Length - 1, 1);

        //    var sqlCommand = $"UPDATE Employee SET {queryString} WHERE EmployeeId = @ExistingId";

        //    var rowAffects = _dbConnection.Execute(sqlCommand, param: dynamicParams);

        //    return rowAffects;
        //}

        ///// <summary>
        ///// Xóa nhân viên khỏi DB
        ///// </summary>
        ///// <param name="employeeId">ID của nhân viên cần xóa</param>
        ///// <returns>Số bản ghi bị ảnh hưởng</returns>
        ///// Author: NQMinh (16/08/2021)
        //public int Delete(List<Guid> employeeIds)
        //{
        //    var parameters = new DynamicParameters();

        //    parameters.Add("@dynamicEmployeeId", employeeIds);

        //    var sqlCommand = $"DELETE FROM Employee WHERE EmployeeId IN @dynamicEmployeeId";

        //    var rowAffects = _dbConnection.Execute(sqlCommand, param: parameters);

        //    return rowAffects;
        //}       
        //#endregion
    }
}
