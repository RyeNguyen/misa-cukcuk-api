using MISA.ApplicationCore.MISAAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Models
{
    public class Employee : BaseEntity
    {
        #region Properties
        /// <summary>
        /// Id của nhân viên
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [MISARequired]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Đệm và tên nhân viên
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Họ nhân viên
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Họ và tên nhân viên
        /// </summary>
        [MISARequired]
        public string FullName { get; set; }

        /// <summary>
        /// Giới tính nhân viên
        /// </summary>
        public int? Gender { get; set; }        

        /// <summary>
        /// Ngày tháng năm sinh nhân viên
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Số điện thoại nhân viên
        /// </summary>
        [MISARequired]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email nhân viên
        /// </summary>
        [MISARequired]
        public string Email { get; set; }

        /// <summary>
        /// Địa chỉ nhân viên
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Số cmnd/căn cước
        /// </summary>
        [MISARequired]
        public string IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp cmnd
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp cmnd
        /// </summary>
        public string IdentityPlace { get; set; }

        /// <summary>
        /// Ngày gia nhập công ty
        /// </summary>
        public DateTime? JoinDate { get; set; }

        /// <summary>
        /// Trạng thái hôn nhân
        /// </summary>
        public int? MartialStatus { get; set; }

        /// <summary>
        /// Trình độ học vấn
        /// </summary>
        public int? EducationalBackground { get; set; }

        /// <summary>
        /// ID chuyên môn
        /// </summary>
        public Guid? QualificationId { get; set; }

        /// <summary>
        /// Id Phòng ban
        /// </summary>
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// id vị trí
        /// </summary>
        public Guid? PositionId { get; set; }

        /// <summary>
        /// Trạng thái công việc
        /// </summary>
        public int? WorkStatus { get; set; }


        /// <summary>
        /// Mã số thuế cá nhân
        /// </summary>
        public string PersonalTaxCode { get; set; }

        /// <summary>
        /// Lương nhân viên
        /// </summary>
        public double? Salary { get; set; }
        #endregion
    }
}
