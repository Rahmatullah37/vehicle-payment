using OneOf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Domain.Utils;

namespace VisualSoft.Surveillance.Payment.Services.Interfaces
{
    public interface IFixedTarifService
    {
        Task<List<FixedTarifModel>> GetAllAsync();
        Task<OneOf<FixedTarifModel, ValidationResult>> GetByIdAsync(Guid id);      
        Task<OneOf<FixedTarifModel, ValidationResult>> AddAsync(FixedTarifModel model);
        Task<OneOf<FixedTarifModel, ValidationResult>> UpdateAsync(FixedTarifModel model);       
        Task DeleteAsync(Guid id);
    }
}

