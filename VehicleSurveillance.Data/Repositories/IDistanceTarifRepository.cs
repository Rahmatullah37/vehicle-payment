
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public interface IDistanceTarifRepository
    {
        
       Task<IEnumerable<DistanceTarifDataModel>?> GetAll();
        Task<DistanceTarifDataModel?> GetById(Guid id);
        Task<DistanceTarifDataModel?> Create(DistanceTarifDataModel distanceTarif);
        Task<DistanceTarifDataModel?> Update(DistanceTarifDataModel distanceTarif);
        Task<bool> Delete(Guid id);
        Task<List<DistanceTarifDataModel>> GetSegmentsForTariffAsync(Guid tarifId);
    }
}
