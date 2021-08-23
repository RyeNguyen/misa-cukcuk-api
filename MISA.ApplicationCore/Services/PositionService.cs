using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Services
{
    public class PositionService : BaseService<Position>, IPositionService
    {
        public PositionService(IBaseRepository<Position> baseRepository) : base(baseRepository)
        {

        }
    }
}
