using Dapper;
using MISA.Infrastructure.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.Infrastructure.Repositories;

namespace MISA.Infrastructure
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        
    }
}
