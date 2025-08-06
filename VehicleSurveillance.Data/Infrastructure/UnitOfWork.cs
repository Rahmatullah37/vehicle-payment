
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VisualSoft.Surveillance.Payment.Data.Repositories;
using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<UnitOfWork> _logger;

        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;

        private IUserIdentificationModel? logedinUser = null;

        public UnitOfWork(IConnectionFactory connectionFactory, ILogger<UnitOfWork> logger)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            _connectionFactory = connectionFactory;
            _logger = logger;

            _connection = _connectionFactory.Connection;
            _connection.Open();
        }
        public IUserIdentificationModel? LogedinUser
        {
            get
            {
                return logedinUser;
            }
            set
            {
                logedinUser = value;
            }
        }

        public IDbTransaction Transaction => _transaction;
        public IDbConnection Connection => _connection;
        public IPackagesRepository PackagesRepository => new PackagesRepository(Connection, LogedinUser);

        public IAccessFeeTransactionRepository AccessFeeTransactionRepository => new AccessFeeTransactionRepository(Connection, LogedinUser);

        public IPaymentModeRepository PaymentModeRepository => new PaymentModeRepository(Connection, LogedinUser);
        public ITarifRepository TarifRepository => new TarifRepository(Connection, LogedinUser);
        public IFixedTarifRepository FixedTarifRepository => new FixedTarifRepository(Connection, LogedinUser);
        public ITarifTypeRepository TarifTypeRepository => new TarifTypeRepository(Connection, LogedinUser);
        public IVehicleTypeRepository VehicleTypeRepository => new VehicleTypeRepository(Connection, LogedinUser);
        public IHourlyTarifRepository HourlyTarifRepository => new HourlyTarifRepository(Connection, LogedinUser);

        public ITollBoothRepository TollBoothRepository => new TollBoothRepository(Connection, LogedinUser);

        public IDistanceTarifRepository DistanceTarifRepository => new DistanceTarifRepository(Connection, LogedinUser);
        public ITimeBasedRepository TimeBasedRepository => new TimeBasedRepository(Connection, LogedinUser);
        public IVehicleAccountsRepository VehicleAccountsRepository => new VehicleAccountsRepository(Connection, LogedinUser);
        public IVehicalPackageRepository VehicalPackageRepository => new VehicalPackageRepository(Connection, LogedinUser);

        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
            _logger.LogInformation("Transaction started.");
        }
        public void Commit()
        {
            try
            {
                _transaction?.Commit();
                _logger.LogInformation("Transaction committed.");
            }
            catch (Exception ex)
            {
                _transaction?.Rollback();
                _logger.LogError(ex, "Transaction commit failed. Rolled back.");
                throw;
            }
            finally
            {
                _connection?.Close();
            }
        }
        public async Task CommitAsync()
        {
            await Task.Run(() => Commit());
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _logger.LogWarning("Transaction rolled back.");
            _connection?.Close();
        }

        public async Task RollbackAsync()
        {
            await Task.Run(() => Rollback());
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
            GC.SuppressFinalize(this);
        }


    }
}
