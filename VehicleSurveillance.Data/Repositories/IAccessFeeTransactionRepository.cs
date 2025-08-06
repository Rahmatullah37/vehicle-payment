
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Data.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public interface IAccessFeeTransactionRepository
    {
        Task<IEnumerable<AccessFeeTransactionDataModel>?> GetAll();
        Task<AccessFeeTransactionDataModel?> GetById(Guid id);
        Task<AccessFeeTransactionDataModel?> Create(AccessFeeTransactionDataModel transaction);
        Task<AccessFeeTransactionDataModel?> Update(AccessFeeTransactionDataModel transaction);
        Task<bool> Delete(Guid id);
        Task<TransactionReportResponse?> GetTransactionReport(TransactionReportRequest request);
    }
}
