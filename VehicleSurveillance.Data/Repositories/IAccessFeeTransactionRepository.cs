using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Domain.Models;

using VehicleSurveillance.Data.Models;
namespace VehicleSurveillance.Data.Repositories
{
    public interface IAccessFeeTransactionRepository
    {
        List<AccessFeeTransactionDataModel> GetAll();
        AccessFeeTransactionDataModel GetById(Guid id);
        void Create(AccessFeeTransactionDataModel accessFeeTransaction);
        void Delete(Guid id);
        void Update(AccessFeeTransactionDataModel accessFeeTransaction);
        TransactionReportResponse GetTransactionReport(TransactionReportRequest request);

    }
}
