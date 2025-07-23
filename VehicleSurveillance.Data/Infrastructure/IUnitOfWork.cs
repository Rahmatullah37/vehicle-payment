using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Data.Repositories;

namespace VehicleSurveillance.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        IPackagesRepository PackagesRepository { get; }
        IAccessFeeTransactionRepository AccessFeeTransactionRepository { get; }
        IPaymentModeRepository PaymentModeRepository { get; }
        ITarifRepository TarifRepository { get; }
        IFixedTarifRepository FixedTarifRepository { get; }
        ITarifTypeRepository TarifTypeRepository { get; }
        IVehicleTypeRepository VehicleTypeRepository { get; }
        IHourlyTarifRepository HourlyTarifRepository { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
