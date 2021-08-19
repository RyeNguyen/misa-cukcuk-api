using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.ApplicationCore.MISAAttribute;
using MISA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Services
{
    public class BaseService<MISAEntity> : IBaseService<MISAEntity>
    {
        #region Fields
        private readonly IBaseRepository<MISAEntity> _baseRepository;
        private readonly ServiceResponse _serviceResponse;
        private readonly string _className;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<MISAEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResponse = new ServiceResponse();
            _className = typeof(MISAEntity).Name;
        }
        #endregion

        #region Method
        #region Phương thức lấy tất cả dữ liệu thực thể
        /// <summary>
        /// Lấy danh sách thực thể từ DB
        /// </summary>
        /// <returns>Danh sách thực thể</returns>
        /// Author: NQMinh (16/08/2021)
        public ServiceResponse GetAll()
        {
            _serviceResponse.Data = _baseRepository.GetAll();

            if (_serviceResponse.Data != null)
            {
                _serviceResponse.MISACode = MISACode.isValid;
            }
            else
            {
                _serviceResponse.MISACode = MISACode.NotValid;
            }
            return _serviceResponse;
        }
        #endregion

        #region Phương thức lấy thông tin thực thể qua ID
        /// <summary>
        /// Phương thức lấy thông tin thực thể qua ID
        /// </summary>
        /// <param name="entityId">ID thực thể (khóa chính)</param>
        /// <returns>Thông tin thực thể cần lấy</returns>
        /// Author: NQMinh (16/08/2021)
        public ServiceResponse GetById(Guid entityId)
        {
            _serviceResponse.Data = _baseRepository.GetById(entityId);

            if (_serviceResponse.Data != null)
            {
                _serviceResponse.MISACode = MISACode.isValid;              
            }
            else
            {
                _serviceResponse.MISACode = MISACode.NotValid;
                _serviceResponse.Message = Entity.Properties.Resources.messageErrorCustomerGetById_Dev;
            }

            return _serviceResponse;
        }
        #endregion

        public ServiceResponse Insert(MISAEntity entity)
        {
            var check = ValidateCommon(entity);
            if (check == false)
            {
                var msg = new
                {
                    devMsg = new
                    {
                        fieldName = $"{_className}Code",
                        msg = Properties.Resources.messageCheckRequired_Dev
                    },
                    userMsg = Properties.Resources.messageCheckRequired_User,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Data = msg;
                _serviceResponse.Message = Properties.Resources.messageCheckCodeDuplicate_Dev;
                _serviceResponse.MISACode = MISACode.NotValid;
                return _serviceResponse;
            }

            //Thêm mới khi dữ liệu hợp lệ:
            var rowAffects = _baseRepository.Insert(entity);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Properties.Resources.messageInsertSuccess;
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }

        public ServiceResponse Update(Guid entityId, MISAEntity entity)
        {
            //    //Lấy mã thực thể để thực hiện validate dữ liệu
            //    var entityCode = typeof(MISAEntity).GetProperty($"{_className}Code").ToString();

            //    //Validate dữ liệu, nếu dữ liệu chưa hợp lệ thì trả về mô tả lỗi:          
            //    //Check trùng mã:             
            //    var entityToCheck = _baseRepository.GetByCode(entityCode);
            //    if (entityToCheck != null)
            //    {
            //        var msg = new
            //        {
            //            devMsg = new
            //            {
            //                fieldName = $"{_className}Code",
            //                msg = Properties.Resources.messageCheckCodeDuplicate_Dev
            //            },
            //            userMsg = Properties.Resources.messageCheckCodeDuplicate_User,
            //            Code = MISACode.NotValid
            //        };
            //        _serviceResponse.Data = msg;
            //        _serviceResponse.Message = Properties.Resources.messageCheckCodeDuplicate_Dev;
            //        _serviceResponse.MISACode = MISACode.NotValid;
            //        return _serviceResponse;
            //    }

            //Sửa thông tin khi dữ liệu hợp lệ:
            var rowAffects = _baseRepository.Update(entityId, entity);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Properties.Resources.messageInsertSuccess;
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }
        #endregion

        #region Phương thức xóa thông tin thực thể
        /// <summary>
        /// Phương thức xóa thông tin thực thể
        /// </summary>
        /// <param name="entityIds">Danh sách ID thực thể cần xóa</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (16/08/2021)
        public ServiceResponse Delete(List<Guid> entityIds)
        {
            var rowAffects = _baseRepository.Delete(entityIds);
            _serviceResponse.Data = rowAffects;          
            _serviceResponse.Message = Entity.Properties.Resources.messageSuccessCustomerDelete;
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }

        private bool ValidateCommon(MISAEntity entity)
        {
            var isValid = true;
            //Thực hiện validate:

            //Bắt buộc nhập:
            //1. Lấy thông tin các property:
            var properties = typeof(MISAEntity).GetProperties();

            //2. Xác định việc validate dựa trên attribute: (MISARequired - check thông tin không được phép null hoặc trống)
            foreach(var prop in properties)
            {
                var propValue = prop.GetValue(entity);

                var propName = prop.Name;

                //Kiểm tra prop hiện tại có bắt buộc nhập hay không
                var propMISARequired = prop.GetCustomAttributes(typeof(MISARequired), true);
                if (propMISARequired.Length > 0)
                {
                    if (prop.PropertyType == typeof(string) && (propValue == null || propValue.ToString() == string.Empty))
                    {
                        isValid = false;
                        _serviceResponse.Message = "Thông tin này không được phép để trống.";
                        return isValid;
                    }
                }
            }

            return isValid;
        }

        protected virtual bool ValidateCustom(MISAEntity entity)
        {
            return true;
        }
        #endregion
    }
}
