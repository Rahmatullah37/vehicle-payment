using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Domain.Utils;

namespace VisualSoft.Surveillance.Payment.Services.Interfaces
{
    public interface IAccessFeeTransactionService
    {
        Task<List<AccessFeeTransactionModel>> GetAllAsync();
        Task<OneOf<AccessFeeTransactionModel, ValidationResult>> GetByIdAsync(Guid id);


        Task<OneOf<AccessFeeTransactionModel, ValidationResult>> AddAsync(AccessFeeTransactionModel model);

        Task<OneOf<AccessFeeTransactionModel, ValidationResult>> UpdateAsync(AccessFeeTransactionModel model);
       
        Task DeleteAsync(Guid id);
       // Task<TransactionReportResponse?> GetTransactionReportAsync(TransactionReportRequest summary);
 }
}
