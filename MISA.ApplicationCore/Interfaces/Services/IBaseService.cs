using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Services
{
    public interface IBaseService<MISAEntity>
    {
        List<MISAEntity> GetAll();

        MISAEntity GetById(Guid entityId);

        ServiceResponse Insert(MISAEntity entity);

        ServiceResponse Update(Guid entityId, MISAEntity entity);

        ServiceResponse Delete(List<Guid> entityIds);
    }
}
