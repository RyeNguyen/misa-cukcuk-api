using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.ApplicationCore.MISAAttribute;
using MISA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                var errorMsg = new
                {
                    devMsg = Entity.Properties.Resources.messageErrorGetAll_Dev,
                    userMsg = Entity.Properties.Resources.messageErrorGetAll_User,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Data = errorMsg;
                _serviceResponse.Message = Entity.Properties.Resources.messageErrorGetAll_Dev;
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
                var errorMsg = new
                {
                    devMsg = Entity.Properties.Resources.messageErrorGetById_Dev,
                    userMsg = Entity.Properties.Resources.messageErrorGetById_User,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Data = errorMsg;
                _serviceResponse.MISACode = MISACode.NotValid;
                _serviceResponse.Message = Entity.Properties.Resources.messageErrorGetById_Dev;
            }

            return _serviceResponse;
        }
        #endregion

        /// <summary>
        /// Phương thức thêm thông tin thực thể vào DB
        /// </summary>
        /// <param name="entity">Thông tin cần thêm</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (17/08/2021)
        public ServiceResponse Insert(MISAEntity entity)
        {
            var commonValidate = ValidateCommon(entity);
            if (commonValidate.MISACode == MISACode.NotValid)
            {
                return commonValidate;
            }

            var customValidate = ValidateCustom(entity);
            if (customValidate.MISACode == MISACode.NotValid)
            {
                return customValidate;
            }

            //Thêm mới khi dữ liệu hợp lệ:
            var rowAffects = _baseRepository.Insert(entity);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Entity.Properties.Resources.messageSuccessInsert;
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }

        /// <summary>
        /// Phương thức cập nhật dữ liệu thực thể
        /// </summary>
        /// <param name="entityId">ID thực thể cần sửa</param>
        /// <param name="entity">Dữ liệu mới</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (17/08/2021)
        public ServiceResponse Update(Guid entityId, MISAEntity entity)
        {
            var commonValidate = ValidateCommon(entity);
            if (commonValidate.MISACode == MISACode.NotValid) 
            {
                return commonValidate;
            }

            var customValidate = ValidateCustom(entity);
            if (customValidate.MISACode == MISACode.NotValid)
            {
                return customValidate;
            }

            //Sửa thông tin khi dữ liệu hợp lệ:
            var rowAffects = _baseRepository.Update(entityId, entity);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Entity.Properties.Resources.messageSuccessUpdate;
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
            _serviceResponse.Message = Entity.Properties.Resources.messageSuccessDelete;
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }

        /// <summary>
        /// Phương thức kiểm tra dữ liệu chung
        /// </summary>
        /// <param name="entity">Dữ liệu thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (18/08/2021)
        private ServiceResponse ValidateCommon(MISAEntity entity)
        {
            //Thực hiện validate:
            //1. Bắt buộc nhập:
            var checkRequired = CheckRequired(entity);
            if (checkRequired.MISACode == MISACode.NotValid)
            {
                return checkRequired;
            }

            //2. Check trùng mã:
            var checkDuplicateCode = CheckDuplicateCode(entity);
            if (checkDuplicateCode.MISACode == MISACode.NotValid)
            {
                return checkDuplicateCode;
            }

            //3. Check định dạng email:
            var checkEmailFormat = CheckEmailFormat(entity);
            if (checkEmailFormat.MISACode == MISACode.NotValid)
            {
                return checkEmailFormat;
            }

            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }

        /// <summary>
        /// Phương thức kiểm tra dữ liệu riêng
        /// </summary>
        /// <param name="entity">Dữ liệu thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (18/08/2021)
        protected virtual ServiceResponse ValidateCustom(MISAEntity entity)
        {
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }

        /// <summary>
        /// Phương thức kiểm tra các trường bắt buộc
        /// </summary>
        /// <param name="entity">Thông tin thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (24/08/2021)
        private ServiceResponse CheckRequired(MISAEntity entity)
        {
            //1. Lấy thông tin các property:
            var properties = typeof(MISAEntity).GetProperties();

            //2. Xác định việc validate dựa trên attribute: (MISARequired - check thông tin không được phép null hoặc trống)
            foreach (var prop in properties)
            {
                var propValue = prop.GetValue(entity);

                //Kiểm tra prop hiện tại có bắt buộc nhập hay không
                var propMISARequired = prop.GetCustomAttributes(typeof(MISARequired), true);
                if (propMISARequired.Length > 0)
                {
                    var errorMessage = (propMISARequired[0] as MISARequired)._message;
                    if (prop.PropertyType == typeof(string) && (propValue == null || propValue.ToString() == string.Empty))
                    {
                        _serviceResponse.MISACode = MISACode.NotValid;
                        _serviceResponse.Message = errorMessage;
                        _serviceResponse.Data = errorMessage;
                        return _serviceResponse;
                    }
                }
            }
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }

        /// <summary>
        /// Phương thức kiểm tra mã trùng
        /// </summary>
        /// <param name="entity">Thông tin thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (24/08/2021)
        private ServiceResponse CheckDuplicateCode(MISAEntity entity)
        {
            var entityId = (Guid)entity.GetType().GetProperty($"{_className}Id").GetValue(entity);
            var checkedEntity = _baseRepository.GetById(entityId);

            var checkCode = _baseRepository.CheckDuplicateCode(entity.GetType().GetProperty($"{_className}Code").GetValue(entity).ToString());
            if (checkCode == true && checkedEntity == null)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.Resources.messageErrorDuplicateCode,
                    userMsg = Entity.Properties.Resources.messageErrorDuplicateCode,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Data = errorObj;
                _serviceResponse.MISACode = MISACode.NotValid;
                _serviceResponse.Message = Entity.Properties.Resources.messageErrorDuplicateCode;
                return _serviceResponse;
            }
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }

        /// <summary>
        /// Phương thức kiểm tra định dạng email
        /// </summary>
        /// <param name="entity">Thông tin thực thể</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (24/08/2021)
        private ServiceResponse CheckEmailFormat(MISAEntity entity)
        {
            var emailFormat = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

            var isMatch = Regex.IsMatch(entity.GetType().GetProperty("Email").GetValue(entity).ToString(), emailFormat, RegexOptions.IgnoreCase);

            if (isMatch == false)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.Resources.messageErrorEmailFormat,
                    userMsg = Entity.Properties.Resources.messageErrorEmailFormat,
                    Code = MISACode.NotValid
                };
                _serviceResponse.Message = Entity.Properties.Resources.messageErrorEmailFormat;
                _serviceResponse.MISACode = MISACode.NotValid;
                _serviceResponse.Data = errorObj;
                return _serviceResponse;
            }
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;
        }
        #endregion
    }
}
