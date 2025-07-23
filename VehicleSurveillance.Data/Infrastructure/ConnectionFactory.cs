using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSurveillance.Data.Infrastructure
{
    public class ConnectionFactory:IConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public string? ConnectionString
        {
            get
            {
                if (_configuration == null)
                {
                    throw new ArgumentNullException(nameof(_configuration));
                }
                else
                {
                    return _configuration.GetSection("ConnectionStrings:DatabaseConnectionString").Value;
                }
            }
        }

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public NpgsqlConnection Connection
        {
            get
            {
                return CreateNewConnection();
            }
        }

        private NpgsqlConnection CreateNewConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }


    }
}
