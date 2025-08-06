using OneOf;
using System;
using System.Collections.Generic;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Domain.Utils;

namespace VisualSoft.Surveillance.Payment.Services.Interfaces
{
    public interface IPaymentModeService
    {
        Task<List<PaymentModeModel>> GetAllAsync();
        Task<OneOf<PaymentModeModel, ValidationResult>> GetByIdAsync(Guid id);
        Task<OneOf<PaymentModeModel, ValidationResult>> AddAsync(PaymentModeModel mode);
        Task<OneOf<PaymentModeModel, ValidationResult>> UpdateAsync(PaymentModeModel mode);
        Task DeleteAsync(Guid id);
    }
}
