
using System;
using System.Collections.Generic;
using VisualSoft.Surveillance.Payment.Data.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public interface ITarifRepository
    {
      //  List<TarifDataModel> GetAll();
        Task<IEnumerable<TarifDataModel>?> GetAll();
        Task<TarifDataModel?> GetById(Guid id);
       Task<TarifDataModel?> Create(TarifDataModel tarif);
       Task< TarifDataModel?> Update(TarifDataModel tarif);
        Task<bool> Delete(Guid id);
       Task< TarifDataModel? >GetTarif(Guid vehicleTypeId, Guid tarifTypeId);
    }
}
