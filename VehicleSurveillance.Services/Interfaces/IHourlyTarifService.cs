using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Domain.Models;

namespace VehicleSurveillance.Services.Interfaces
{
    public interface IHourlyTarifService
    {
        List<HourlyTarifModel> GetAll();
        HourlyTarifModel GetById(Guid id);
        void Add(HourlyTarifModel model);
        void Update(HourlyTarifModel model);
        void Delete(Guid id);
    }
}
