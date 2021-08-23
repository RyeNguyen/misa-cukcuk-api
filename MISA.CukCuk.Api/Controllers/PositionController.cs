using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.ApplicationCore.Interfaces.Services;
using MISA.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PositionsController : BaseEntityController<Position>
    {
        #region Declares
        private readonly IPositionService _positionService;
        private readonly IPositionRepository _positionRepository;
        #endregion

        #region Constructor
        public PositionsController(IPositionService positionService,
            IPositionRepository positionRepository) : base(positionService, positionRepository)
        {
            _positionService = positionService;
            _positionRepository = positionRepository;
        }
        #endregion
    }
}
