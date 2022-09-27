using Microsoft.Extensions.Configuration;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql; 
namespace Services.DateContext
{
    public class Context
    {
        private readonly IConfiguration _configuration;
        public Context(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public NpgsqlConnection CreateConnection()
        {
            var connection = _configuration.GetConnectionString("NpgSqlConnection");
            return new NpgsqlConnection (connection); 
        }

    }
}
