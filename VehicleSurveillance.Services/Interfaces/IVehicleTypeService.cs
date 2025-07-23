using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Domain.Models;

namespace VehicleSurveillance.Services.Interfaces
{
    public interface IVehicleTypeService
    {

        List<VehicleTypeModel> GetAll();
        VehicleTypeModel GetById(Guid id);
        void Add(VehicleTypeModel model);
        void Update(VehicleTypeModel model);
        void Delete(Guid id);

    }
}
