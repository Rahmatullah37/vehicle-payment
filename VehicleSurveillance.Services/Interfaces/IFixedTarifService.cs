using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Domain.Models;

namespace VehicleSurveillance.Services.Interfaces
{
    public interface IFixedTarifService
    {
        List<FixedTarifModel> GetAll();
        FixedTarifModel GetById(Guid id);
        void Add(FixedTarifModel model);
        void Update(FixedTarifModel model);
        void Delete(Guid id);
    }
}
