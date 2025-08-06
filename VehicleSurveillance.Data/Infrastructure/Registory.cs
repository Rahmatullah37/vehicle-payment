using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Repositories;
using VisualSoft.Surveillance.Payment.Data.Infrastructure;

namespace VisualSoft.Surveillance.Payment.Data.Infrastructure
{
    public static class Registory
    {
        public static void AddServicesToContainer(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<IQueryExecuter, QueryExecuter>();

           


        }
    }
}
