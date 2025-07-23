using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Data.Repositories;

namespace VehicleSurveillance.Data.Infrastructure
{
    public static class Registory
    {
        public static void AddServicesToContainer(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<IPackagesRepository>(provider =>
            {
                var uow = provider.GetRequiredService<IUnitOfWork>();
                return new PackagesRepository(uow.Connection, uow.Transaction);
            });
            services.AddScoped<IAccessFeeTransactionRepository>(provider =>
            {
                var uow = provider.GetRequiredService<IUnitOfWork>();
                return new AccessFeeTransactionRepository(uow.Connection, uow.Transaction);
            });
            services.AddScoped<IPaymentModeRepository>(provider =>
            {
                var uow = provider.GetRequiredService<IUnitOfWork>();
                return new PaymentModeRepository(uow.Connection, uow.Transaction);
            });
            services.AddScoped<ITarifRepository>(provider =>
            {
                var uow = provider.GetRequiredService<IUnitOfWork>();
                return new TarifRepository(uow.Connection, uow.Transaction);
            });
            services.AddScoped<IFixedTarifRepository>(provider =>
            {
                var uow = provider.GetRequiredService<IUnitOfWork>();
                return new FixedTarifRepository(uow.Connection, uow.Transaction);
            });
            services.AddScoped<ITarifTypeRepository>(provider =>
            {
                var uow = provider.GetRequiredService<IUnitOfWork>();
                return new TarifTypeRepository(uow.Connection, uow.Transaction);
            });
            services.AddScoped<IVehicleTypeRepository>(provider =>
            {
                var uow = provider.GetRequiredService<IUnitOfWork>();
                return new VehicleTypeRepository(uow.Connection, uow.Transaction);
            });
            services.AddScoped<IHourlyTarifRepository>(provider =>
            {
                var uow = provider.GetRequiredService<IUnitOfWork>();
                return new HourlyTarifRepository(uow.Connection, uow.Transaction);
            });


        }
    }
}
