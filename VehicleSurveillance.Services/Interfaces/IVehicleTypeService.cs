using OneOf;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Domain.Utils;

namespace VisualSoft.Surveillance.Payment.Services.Interfaces
{
    public interface IVehicleTypeService
    {
        Task<List<VehicleTypeModel>> GetAllAsync();
        Task<OneOf<VehicleTypeModel, ValidationResult>> GetByIdAsync(Guid id);
        Task<OneOf<VehicleTypeModel, ValidationResult>> AddAsync(VehicleTypeModel model);
        Task<OneOf<VehicleTypeModel, ValidationResult>> UpdateAsync(VehicleTypeModel model);
        Task DeleteAsync(Guid id);
    }
}
