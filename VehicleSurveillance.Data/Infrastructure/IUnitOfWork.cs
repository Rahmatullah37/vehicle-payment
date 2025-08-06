using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Repositories;

namespace VisualSoft.Surveillance.Payment.Data.Infrastructure
{
    //public interface IUnitOfWork
    //{
    //    IDbConnection Connection { get; }
    //    IDbTransaction Transaction { get; }
    //    IPackagesRepository PackagesRepository { get; }
    //    IAccessFeeTransactionRepository AccessFeeTransactionRepository { get; }
    //    IPaymentModeRepository PaymentModeRepository { get; }
    //    ITarifRepository TarifRepository { get; }
    //    IFixedTarifRepository FixedTarifRepository { get; }
    //    ITarifTypeRepository TarifTypeRepository { get; }
    //    IVehicleTypeRepository VehicleTypeRepository { get; }
    //    IHourlyTarifRepository HourlyTarifRepository { get; }
    //    void BeginTransaction();
    //    void Commit();
    //    void Rollback();
    //}

    public interface IUnitOfWork
    {
        IDbTransaction Transaction { get; }
        IDbConnection Connection { get; }

        IPackagesRepository PackagesRepository { get; }
        IAccessFeeTransactionRepository AccessFeeTransactionRepository { get; }
        IPaymentModeRepository PaymentModeRepository { get; }
        ITarifRepository TarifRepository { get; }
        IFixedTarifRepository FixedTarifRepository { get; }
        ITarifTypeRepository TarifTypeRepository { get; }
        IVehicleTypeRepository VehicleTypeRepository { get; }
        IHourlyTarifRepository HourlyTarifRepository { get; }
        ITollBoothRepository TollBoothRepository { get; }
        IDistanceTarifRepository DistanceTarifRepository { get; }
        ITimeBasedRepository TimeBasedRepository { get; }
        IVehicleAccountsRepository VehicleAccountsRepository { get; }

        IVehicalPackageRepository VehicalPackageRepository { get; }
        void BeginTransaction();
        void Commit();
        Task CommitAsync();
        void Rollback();
        Task RollbackAsync();
    }
}
