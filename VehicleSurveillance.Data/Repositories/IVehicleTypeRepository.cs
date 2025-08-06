
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public interface IVehicleTypeRepository
    {
        Task<IEnumerable<VehicleTypeDataModel>> GetAll();
        Task<VehicleTypeDataModel?> GetById(Guid id);
        Task<VehicleTypeDataModel?> GetByName(string name);
        Task<VehicleTypeDataModel?> Create(VehicleTypeDataModel vehicleType);
        Task<VehicleTypeDataModel?> Update(VehicleTypeDataModel vehicleType);
        Task<bool> Delete(Guid id);
    }
}
