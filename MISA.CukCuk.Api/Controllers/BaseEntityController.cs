using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseEntityController<MISAEntity> : ControllerBase
    {
        #region Declares
        readonly private IBaseService<MISAEntity> _baseService;
        readonly private IBaseRepository<MISAEntity> _baseRepository;
        #endregion

        #region Constructor
        public BaseEntityController(IBaseService<MISAEntity> baseService, 
            IBaseRepository<MISAEntity> baseRepository)
        {
            _baseService = baseService;
            _baseRepository = baseRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// <returns>Danh sách dữ liệu thực thể</returns>
        /// Author: NQMinh (18/08/2021)
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var entities = _baseService.GetAll();
                if (entities.MISACode == MISACode.isValid)
                {
                    return Ok(entities.Data);
                }
                else
                {
                    return BadRequest(entities.Data);
                }
            }
            catch (Exception)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.Resources.messageErrorGetAll_Dev,
                    userMsg = Entity.Properties.Resources.messageErrorGetAll_User,
                    Code = MISACode.NotValid
                };
                return BadRequest(errorObj);
            }
        }

        /// <summary>
        /// Lấy dữ liệu qua ID
        /// </summary>
        /// <param name="entityId">ID của thực thể (khóa chính)</param>
        /// <returns>Dữ liệu thực thể</returns>
        /// Author: NQMinh (18/08/2021)
        [HttpGet("{entityId}")]
        public IActionResult GetById(Guid entityId)
        {
            try
            {
                var entity = _baseService.GetById(entityId);

                if (entity.MISACode == MISACode.isValid)
                {
                    return Ok(entity.Data);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception)
            {
                var errorObj = new
                {
                    devMsg = Entity.Properties.Resources.messageErrorGetById_Dev,
                    userMsg = Entity.Properties.Resources.messageErrorGetById_User,
                    Code = MISACode.NotValid
                };
                return BadRequest(errorObj);
            }
        }

        /// <summary>
        /// Thêm thông tin vào DB
        /// </summary>
        /// <param name="entity">Thông tin cần thêm mới</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (18/08/2021)
        [HttpPost]
        public IActionResult Insert(MISAEntity entity)
        {
            var insertResult = _baseService.Insert(entity);

            if (insertResult.MISACode == MISACode.NotValid)
            {
                return BadRequest(insertResult.Data);
            }

            if (insertResult.MISACode == MISACode.isValid && (int)insertResult.Data > 0)
            {
                return Created(Entity.Properties.Resources.messageSuccessInsert, insertResult.Data);
            }
            else
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="entityId">ID thực thể</param>
        /// <param name="entity">Thông tin mới</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (18/08/2021)
        [HttpPatch("{entityId}")]
        public IActionResult Update(Guid entityId, MISAEntity entity)
        {
            var updateResult = _baseService.Update(entityId, entity);

            if (updateResult.MISACode == MISACode.NotValid)
            {
                return BadRequest(updateResult.Data);
            }

            if (updateResult.MISACode == MISACode.isValid && (int)updateResult.Data > 0)
            {
                return Created(Entity.Properties.Resources.messageSuccessUpdate, updateResult.Data);
            }
            else
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Xóa thông tin 
        /// </summary>
        /// <param name="entityIds">Danh sách ID các thực thể cần xóa</param>
        /// <returns>Phản hồi tương ứng</returns>
        /// Author: NQMinh (18/08/2021)
        [HttpPost("delete")]
        public IActionResult Delete([FromBody]List<Guid> entityIds)
        {
            var deleteResult = _baseService.Delete(entityIds);

            if (deleteResult.MISACode == MISACode.NotValid)
            {
                return BadRequest(deleteResult.Data);
            }

            if (deleteResult.MISACode == MISACode.isValid && (int)deleteResult.Data > 0)
            {
                return StatusCode(200, Entity.Properties.Resources.messageSuccessDelete);
            }
            else
            {
                return NoContent();
            }
        }
        #endregion
    }
}
