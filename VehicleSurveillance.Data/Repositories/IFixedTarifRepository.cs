
using VisualSoft.Surveillance.Payment.Data.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public interface IFixedTarifRepository
    {
        Task<IEnumerable<FixedTarifDataModel>?> GetAll();
        Task<FixedTarifDataModel?> GetById(Guid id);
        Task<FixedTarifDataModel?> Create(FixedTarifDataModel fixedTarif);
        Task<FixedTarifDataModel?> Update(FixedTarifDataModel fixedTarif);
        Task<bool> Delete(Guid id);
        Task<FixedTarifDataModel?> GetByTarifId(Guid tarifId);
    }
}
