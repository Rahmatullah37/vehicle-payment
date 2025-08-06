
using System;
using System.Collections.Generic;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public interface ITarifTypeRepository
    {
        Task<IEnumerable<TarifTypeDataModel>> GetAll();
        Task<TarifTypeDataModel?> GetById(Guid id);
        Task<TarifTypeDataModel?> Create(TarifTypeDataModel tarifType);
        Task<TarifTypeDataModel?> Update(TarifTypeDataModel tarifType);
        Task<bool> Delete(Guid id);
        Task<TarifTypeDataModel?> GetFixedTarifType();
        Task<TarifTypeDataModel?> GetHourlyTarifType();
        Task<TarifTypeEnum> GetTarifType(Guid tarifTypeId);

        Task<decimal> GetAmountByTarif(Guid tarifId, TarifTypeEnum type, int totalHours);
    }
}
