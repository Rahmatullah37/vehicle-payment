using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Data.Models;

namespace VehicleSurveillance.Data.Repositories
{
    public interface IHourlyTarifRepository
    {
       
            List<HourlyTarifDataModel> GetAll();
            HourlyTarifDataModel GetById(Guid id);
            void Create(HourlyTarifDataModel hourlyTarif);
            void Delete(Guid id);
            void Update(HourlyTarifDataModel hourlyTarif);


        
    }
}
