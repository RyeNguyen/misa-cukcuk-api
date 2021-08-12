using MISA.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    public class ServiceResponse
    {
        public object Data { get; set; }

        public string Message { get; set; }

        public MISACode MISACode { get; set; }
    }
}
