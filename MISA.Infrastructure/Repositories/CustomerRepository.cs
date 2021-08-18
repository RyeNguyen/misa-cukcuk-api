using Dapper;
using MISA.Infrastructure.Models;
using MySqlConnector;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.Infrastructure.Repositories;

namespace MISA.Infrastructure
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        
    }
}
