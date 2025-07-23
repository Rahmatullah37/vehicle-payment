
using VehicleSurveillance.Data.Models;

namespace VehicleSurveillance.Data.Repositories
{
    public interface IDistanceTarifRepository
    {
        List<DistanceTarifDataModel> GetAll();
        DistanceTarifDataModel GetById(Guid id);
        void Create(DistanceTarifDataModel distanceTarif);
        void Update(DistanceTarifDataModel distanceTarif);
        void Delete(Guid id);
        DistanceTarifDataModel GetByTarifId(Guid tarifId);
    }
}
