using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Data.Models;

namespace VehicleSurveillance.Data.Repositories
{
    public interface ITarifRepository
    {
        List<TarifDataModel> GetAll();
        TarifDataModel GetById(Guid id);
        void Create(TarifDataModel tarif);
        void Update(TarifDataModel tarif);
        void Delete(Guid id);
        TarifDataModel GetTarif(Guid vehicleTypeId, Guid tarifTypeId);
    }
}
