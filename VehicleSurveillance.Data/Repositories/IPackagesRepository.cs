
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public interface IPackagesRepository
    {
        Task<IEnumerable<PackageDataModel>> GetAllAsync();
        Task<PackageDataModel?> GetByIdAsync(Guid id);
        Task<PackageDataModel?> CreateAsync(PackageDataModel package);
        Task<PackageDataModel?> UpdateAsync(PackageDataModel package);
        Task<bool> DeleteAsync(Guid id);
        Task<VehiclePackageDataModel?> SubscribeVehicleToPackageAsync(Guid vehicleId, Guid packageId);
        Task<VehiclePackageDataModel?> SubscribeVehicleToPackageByNameAsync(Guid vehicleId, string packageName);
        Task<IEnumerable<PackageDataModel>> GetActivePackagesAsync();
        Task<IEnumerable<PackageDataModel>> GetPackagesByTypeAsync(string packageType);
        Task<IEnumerable<PackageDataModel>> GetPackagesByCostRangeAsync(decimal minCost, decimal maxCost);
        Task<PackageDataModel?> GetCheapestPackageAsync();
        Task<PackageDataModel?> GetMostExpensivePackageAsync();
        Task<int> GetPackageCountAsync();
        Task<int> GetPackageCountByTypeAsync(string packageType);
        Task<bool> ActivatePackageAsync(Guid id);
        Task<bool> DeactivatePackageAsync(Guid id);
        Task<bool> UpdatePackageCostAsync(Guid id, decimal newCost);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExtendVehiclePackageAsync(Guid vehiclePackageId, DateTime newExpireDate);
        Task<IEnumerable<VehiclePackageDataModel>> GetExpiredPackagesAsync();
        Task<int> GetSubscribedVehicleCountByPackageAsync(Guid packageId);
        Task<bool> HasActiveSubscriptionAsync(Guid vehicleId, Guid packageId);
        Task<IEnumerable<PackagePopularityDataModel>> GetPackagePopularityStatsAsync();
    }
}
