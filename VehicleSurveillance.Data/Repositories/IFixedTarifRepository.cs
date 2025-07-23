using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Data.Models;
using VehicleSurveillance.Domain.Models;

namespace VehicleSurveillance.Data.Repositories
{
    public interface IFixedTarifRepository
    {
       
            List<FixedTarifDataModel> GetAll();
            FixedTarifDataModel GetById(Guid id);
            void Create(FixedTarifDataModel fixedTarif);
            void Update(FixedTarifDataModel fixedTarif);
            void Delete(Guid id);
            FixedTarifDataModel GetByTarifId(Guid tarifId);


    }
}
