using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Domain.Models;

namespace VehicleSurveillance.Services.Interfaces
{
    public interface IAccessFeeTransactionService
    {
        List<AccessFeeTransactionModel> GetAll();
        AccessFeeTransactionModel GetById(Guid id);
        void Add(AccessFeeTransactionModel model);
        void Update(AccessFeeTransactionModel model);
        void Delete(Guid id);
        TransactionReportResponse GetTransactionReport(TransactionReportRequest summary);

    }
}
