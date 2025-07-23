using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Data.Models;

namespace VehicleSurveillance.Data.Repositories
{
    public interface IVehicleTypeRepository
    {
        List<VehicleTypeDataModel> GetAll();
        VehicleTypeDataModel GetById(Guid id);
        void Create(VehicleTypeDataModel vehicleType);
        void Update(VehicleTypeDataModel vehicleType);
        void Delete(Guid id);
        VehicleTypeDataModel GetByName(string name);

    }
}
