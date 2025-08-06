
using OneOf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Domain.Utils;

namespace VisualSoft.Surveillance.Payment.Services.Interfaces
{
    public interface IPackagesService
    {
        Task<List<PackageModel>> GetAllAsync();
        Task<OneOf<PackageModel, ValidationResult>> GetByIdAsync(Guid id);
        Task<OneOf<PackageModel, ValidationResult>> AddAsync(PackageModel package);
        Task<OneOf<PackageModel, ValidationResult>> UpdateAsync(PackageModel package);
        Task DeleteAsync(Guid id);
        Task<List<PackageModel>> GetActivePackagesAsync();
        Task<List<PackageModel>> GetPackagesByTypeAsync(string packageType);
        Task<List<PackageModel>> GetPackagesByCostRangeAsync(decimal minCost, decimal maxCost);
        Task<PackageModel?> GetCheapestPackageAsync();
        Task<PackageModel?> GetMostExpensivePackageAsync();
        Task<int> GetPackageCountAsync();
        Task<int> GetPackageCountByTypeAsync(string packageType);
        Task<OneOf<bool, ValidationResult>> ActivatePackageAsync(Guid id);
        Task<OneOf<bool, ValidationResult>> DeactivatePackageAsync(Guid id);
        Task<OneOf<bool, ValidationResult>> UpdatePackageCostAsync(Guid id, decimal newCost);
        Task<bool> ExistsAsync(Guid id);
        Task<OneOf<bool, ValidationResult>> ExtendVehiclePackageAsync(Guid vehiclePackageId, DateTime newExpireDate);
        Task<List<VehiclePackageModel>> GetExpiredPackagesAsync();
        Task<int> GetSubscribedVehicleCountByPackageAsync(Guid packageId);
        Task<bool> HasActiveSubscriptionAsync(Guid vehicleId, Guid packageId);
        Task<List<PackagePopularityDomainModel>> GetPackagePopularityStatsAsync();


    }
}
