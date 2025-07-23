using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Data.Models;
using VehicleSurveillance.Domain.Models;

namespace VehicleSurveillance.Data.Repositories
{
    public interface ITarifTypeRepository  //method signature 
    {

        List<TarifTypeDataModel> GetAll();
        TarifTypeDataModel GetById(Guid id);
        void Create(TarifTypeDataModel tarifType);
        void Update(TarifTypeDataModel tarifType);
        void Delete(Guid id);
        TarifTypeDataModel GetFixedTarifType();
        TarifTypeDataModel GetHourlyTarifType();
        TarifTypeEnum GetTarifType(Guid tarifTypeId);
        decimal GetAmountByTarif(Guid tarifId, TarifTypeEnum type, int totalHours);


    }
}
