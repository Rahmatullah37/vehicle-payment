
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public interface IHourlyTarifRepository
    {
        Task<IEnumerable<HourlyTarifDataModel>> GetAll();
        Task<HourlyTarifDataModel?> GetById(Guid id);
        Task<HourlyTarifDataModel?> Create(HourlyTarifDataModel hourlyTarif);
        Task<HourlyTarifDataModel?> Update(HourlyTarifDataModel hourlyTarif);
        Task<bool> Delete(Guid id);
    }
}
