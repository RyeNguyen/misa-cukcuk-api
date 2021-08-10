using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Models
{
    public class Position : BaseEntity
    {
        /// <summary>
        /// Id vị trí
        /// </summary>
        public Guid PositionId { get; set; }

        /// <summary>
        /// Mã vị trí
        /// </summary>
        public string PositionCode { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        public string PositionName { get; set; }        

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
    }
}
