using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
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
        IBaseRepository<MISAEntity> _baseRepository;
        ServiceResponse _serviceResponse;
        private string className = typeof(MISAEntity).Name;

        public BaseService(IBaseRepository<MISAEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResponse = new ServiceResponse();
        }

        public ServiceResponse Delete(List<Guid> entityIds)
        {
            throw new NotImplementedException();
        }

        public List<MISAEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public MISAEntity GetById(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse Insert(MISAEntity entity)
        {
            var className = typeof(MISAEntity).Name;

            //Validate dữ liệu, nếu dữ liệu chưa hợp lệ thì trả về mô tả lỗi:          
            //Check trường bắt buộc nhập:
            
            //if (string.IsNullOrEmpty(entityCode))
            //{
            //    var msg = new
            //    {
            //        devMsg = new
            //        {
            //            fieldName = $"{className}Code",
            //            msg = Properties.Resources.messageCheckRequired_Dev
            //        },
            //        userMsg = Properties.Resources.messageCheckRequired_User,
            //        Code = MISACode.NotValid
            //    };
            //    _serviceResponse.Data = msg;
            //    _serviceResponse.Message = Properties.Resources.messageCheckRequired_Dev;
            //    _serviceResponse.MISACode = MISACode.NotValid;
            //    return _serviceResponse;
            //}

            //Check trùng mã: 
            //var entityToCheck = _baseRepository.GetByCode(entityCode);
            //if (entityToCheck != null)
            //{
            //    var msg = new
            //    {
            //        devMsg = new
            //        {
            //            fieldName = $"{className}Code",
            //            msg = Properties.Resources.messageCheckCodeDuplicate_Dev
            //        },
            //        userMsg = Properties.Resources.messageCheckCodeDuplicate_User,
            //        Code = MISACode.NotValid
            //    };
            //    _serviceResponse.Data = msg;
            //    _serviceResponse.Message = Properties.Resources.messageCheckCodeDuplicate_Dev;
            //    _serviceResponse.MISACode = MISACode.NotValid;
            //    return _serviceResponse;
            //}

            //Thêm mới khi dữ liệu hợp lệ:
            var rowAffects = _baseRepository.Insert(entity);
            _serviceResponse.Data = rowAffects;
            _serviceResponse.Message = Properties.Resources.messageInsertSuccess;
            _serviceResponse.MISACode = MISACode.isValid;
            return _serviceResponse;           
        }

        public ServiceResponse Update(Guid entityId, MISAEntity entity)
        {
            _serviceResponse.Data = _baseRepository.Update(entityId, entity);
            return _serviceResponse; 
        }
    }
}
