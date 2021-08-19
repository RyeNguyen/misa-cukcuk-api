using Dapper;
using MISA.Infrastructure.Models;
using MySqlConnector;
using MISA.ApplicationCore.Interfaces.Repositories;
using MISA.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace MISA.Infrastructure
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IConfiguration configuration) : base(configuration)
        {

        }
    }
}
