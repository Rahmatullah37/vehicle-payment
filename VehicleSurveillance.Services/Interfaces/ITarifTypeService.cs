using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Domain.Models;

namespace VehicleSurveillance.Services.Interfaces
{
    public interface ITarifTypeService
    {

        List<TarifTypeModel> GetAll();
        TarifTypeModel GetById(Guid id);
        void Add(TarifTypeModel model);
        void Update(TarifTypeModel model);
        void Delete(Guid id);

    }
}
