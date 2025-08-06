using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Domain.Configurations;
using VisualSoft.Surveillance.Payment.Domain.Configurations;

namespace VisualSoft.Surveillance.Payment.Data.Infrastructure
{
    public class ConnectionFactory:IConnectionFactory
    {
        private readonly IServiceConfiguration _serviceConfiguration;
        //public ConnectionFactory(IServiceConfiguration serviceConfiguration)
        //{
        //    _serviceConfiguration = serviceConfiguration;
        //}
        public ConnectionFactory(IServiceConfiguration serviceConfiguration)
        {
            if (serviceConfiguration == null)
                throw new ArgumentNullException(nameof(serviceConfiguration), "ServiceConfiguration not injected");

            _serviceConfiguration = serviceConfiguration;
        }
        public string ConnectionString
        {
            get
            {
                if (_serviceConfiguration.DatabaseConnectionString == null)
                {
                    throw new ArgumentNullException(nameof(_serviceConfiguration));
                }
                else
                {
                    //return _configuration.GetSection("ConnectionStrings:DatabaseConnectionString").Value;
                    return _serviceConfiguration.DatabaseConnectionString;
                }
            }
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
