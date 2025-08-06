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
   public interface ITarifService
        {
        Task<List<TarifModel>> GetAllAsync();
        Task<OneOf<TarifModel, ValidationResult>?> GetByIdAsync(Guid id);
        Task<OneOf<TarifModel, ValidationResult>?> AddAsync(TarifModel model);
        Task<OneOf<TarifModel, ValidationResult>?> UpdateAsync(TarifModel model);
        Task DeleteAsync(Guid id);
    }
    
}
