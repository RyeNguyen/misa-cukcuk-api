using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interfaces.Repositories
{
    public interface IBaseRepository<MISAEntity>
    {
        List<MISAEntity> GetAll();

        MISAEntity GetById(Guid entityId);

        MISAEntity GetByCode(string entityCode);

        int Insert(MISAEntity entity);

        int Update(Guid entityId, MISAEntity entity);

        int Delete(List<Guid> entityIds);
    }
}
