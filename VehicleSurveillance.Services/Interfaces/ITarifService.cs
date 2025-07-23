using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Domain.Models;

namespace VehicleSurveillance.Services.Interfaces
{
   public interface ITarifService
        {
            List<TarifModel> GetAll();
            TarifModel GetById(Guid id);
            void Add(TarifModel model);
            void Update(TarifModel model);
            void Delete(Guid id);
        }
    
}
