using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Domain.Utils;

namespace VisualSoft.Surveillance.Payment.Services.Interfaces
{
    public interface IHourlyTarifService
    {
        public interface IHourlyTarifService
        {
            Task<List<HourlyTarifModel>> GetAllAsync();
            Task<OneOf<HourlyTarifModel, ValidationResult>> GetByIdAsync(Guid id);

            Task<OneOf<HourlyTarifModel, ValidationResult>> AddAsync(HourlyTarifModel model);
            Task<OneOf<HourlyTarifModel, ValidationResult>> UpdateAsync(HourlyTarifModel model);
            Task DeleteAsync(Guid id);
        }
    }
}
