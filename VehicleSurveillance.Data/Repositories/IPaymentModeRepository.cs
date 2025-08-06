
using System;
using System.Collections.Generic;
using VisualSoft.Surveillance.Payment.Data.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public interface IPaymentModeRepository
    {
        Task<IEnumerable<PaymentModeDataModel>> GetAll();
        Task<PaymentModeDataModel?> GetById(Guid id);
        Task<PaymentModeDataModel?> Create(PaymentModeDataModel mode);
        Task<PaymentModeDataModel?> Update(PaymentModeDataModel mode);
        Task<bool> Delete(Guid id);
        Task<PaymentModeDataModel?> GetPaymentByName(string name);
    }
}
