using OneOf;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Domain.Utils;

namespace VisualSoft.Surveillance.Payment.Services.Interfaces
{
    public interface ITarifTypeService
    {
        Task<List<TarifTypeModel>> GetAllAsync();
        Task<OneOf<TarifTypeModel, ValidationResult>> GetByIdAsync(Guid id);
        Task<OneOf<TarifTypeModel, ValidationResult>> AddAsync(TarifTypeModel model);
        Task<OneOf<TarifTypeModel, ValidationResult>> UpdateAsync(TarifTypeModel model);
        Task DeleteAsync(Guid id);
    }
}
