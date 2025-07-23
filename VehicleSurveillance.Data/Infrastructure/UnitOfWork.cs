using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VehicleSurveillance.Data.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VehicleSurveillance.Data.Infrastructure
{
    public class UnitOfWork:IUnitOfWork,IDisposable
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IDbConnection connection;
        private IDbTransaction transaction;
        public UnitOfWork(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            connection = _connectionFactory.Connection;
            connection.Open();
        }


        public IDbTransaction Transaction => transaction;
        public IDbConnection Connection => connection;


        private IPackagesRepository _packagesRepository;
        public IPackagesRepository PackagesRepository =>
            _packagesRepository ??= new PackagesRepository(connection, transaction);

        private IAccessFeeTransactionRepository _accessFeeTransactionRepository;
        public IAccessFeeTransactionRepository AccessFeeTransactionRepository =>
            _accessFeeTransactionRepository ??= new AccessFeeTransactionRepository(connection, transaction);

        private IPaymentModeRepository _paymentModeRepository;
        public IPaymentModeRepository PaymentModeRepository =>
            _paymentModeRepository ??= new PaymentModeRepository(connection, transaction);

        private ITarifRepository _tarifRepository;
        public ITarifRepository TarifRepository =>
            _tarifRepository ??= new TarifRepository(connection, transaction);


        private IFixedTarifRepository _fixedTarifRepository;
        public IFixedTarifRepository FixedTarifRepository =>
            _fixedTarifRepository ??= new FixedTarifRepository(connection, transaction);

        private ITarifTypeRepository _tarifTypeRepository;
        public ITarifTypeRepository TarifTypeRepository =>
            _tarifTypeRepository ??= new TarifTypeRepository(connection, transaction);
        private IVehicleTypeRepository _vehicleTypeRepository;
        public IVehicleTypeRepository VehicleTypeRepository =>
            _vehicleTypeRepository ??= new VehicleTypeRepository(connection, transaction);

        private IHourlyTarifRepository _hourlyTarifRepository;
        public IHourlyTarifRepository HourlyTarifRepository =>
            _hourlyTarifRepository ??= new HourlyTarifRepository(connection, transaction);



        public void BeginTransaction()
        {
            transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
                Dispose();
            }
        }

        public void Rollback()
        {
            transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            transaction?.Dispose();
            connection?.Close();
            connection?.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
