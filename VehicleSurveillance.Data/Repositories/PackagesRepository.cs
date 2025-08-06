
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Infrastructure;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public class PackagesRepository : IPackagesRepository
    {
        private readonly IDbConnection _connection;
        private readonly IUserIdentificationModel _logedinUser;

        public PackagesRepository(IDbConnection connection,IUserIdentificationModel logedinUser)
        {
            _connection = connection;
            _logedinUser = logedinUser;
        }

        public async Task<IEnumerable<PackageDataModel>> GetAllAsync()
        {

            var sql = @"SELECT * FROM Packages;";
            return await _connection.QueryAsync<PackageDataModel>(sql);
        }

        public async Task<PackageDataModel?> GetByIdAsync(Guid id)
        {
             var sql = @"SELECT * FROM Packages WHERE id = @Id;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return await _connection.QueryFirstOrDefaultAsync<PackageDataModel>(sql, parameters);
        }

        public async Task<PackageDataModel?> CreateAsync(PackageDataModel package)
        {
            var sql = @"
                INSERT INTO Packages (
                    package_type,
                    package_cost,
                    is_active,
                    created_by,
                    created_date,
                    updated_by,
                    updated_date
                )
                VALUES (
                    @Package_Type,
                    @Package_Cost,
                    @Is_Active,
                    @Created_By,
                    @Created_Date,
                    @Updated_By,
                    @Updated_Date
                )";

            var parameters = new DynamicParameters();
            parameters.Add("@Package_Type", package.Package_Type);
            parameters.Add("@Package_Cost", package.Package_Cost);
            parameters.Add("@Is_Active", package.IsActive);
            parameters.Add("@Created_By", package.CreatedBy);
            parameters.Add("@Created_Date", package.CreatedDate);
            parameters.Add("@Updated_By", package.UpdatedBy);
            parameters.Add("@Updated_Date", package.UpdatedDate);
            return await _connection.QueryFirstOrDefaultAsync<PackageDataModel>(sql, parameters);
        }

        public async Task<PackageDataModel?> UpdateAsync(PackageDataModel package)
        {
            var sql = @"
                UPDATE Packages SET
                    package_type  = @Package_Type,
                    package_cost  = @Package_Cost,
                    is_active     = @Is_Active,
                    updated_by    = @Updated_By,
                    updated_date  = @Updated_Date
                WHERE id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", package.Id);
            parameters.Add("@Package_Type", package.Package_Type);
            parameters.Add("@Package_Cost", package.Package_Cost);
            parameters.Add("@Is_Active", package.IsActive);
            parameters.Add("@Updated_By", package.UpdatedBy);
            parameters.Add("@Updated_Date", package.UpdatedDate);



            return await _connection.QueryFirstOrDefaultAsync<PackageDataModel>(sql, parameters);
        }
        
        public async Task<bool> DeleteAsync(Guid id)
        {
            var sql = @"
        DELETE FROM Packages
        WHERE       id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var affectedRows = await _connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public async Task<VehiclePackageDataModel?> SubscribeVehicleToPackageAsync(Guid vehicleId, Guid packageId)
        {
            // First, get the package details to validate it exists and is active
            var package = await GetByIdAsync(packageId);
            if (package == null)
            {
                throw new ArgumentException($"Package with ID {packageId} not found");
            }

            if ((bool)!package.IsActive)
            {
                throw new InvalidOperationException($"Package {package.Package_Type} is not active");
            }

            // Check if vehicle already has an active subscription for this package
            var hasActiveSubscription = await HasActiveSubscriptionAsync(vehicleId, packageId);
            if (hasActiveSubscription)
            {
                throw new InvalidOperationException($"Vehicle already has an active subscription for package {package.Package_Type}");
            }

            var subscribeDate = DateTime.UtcNow;
            // Calculate expire date based on package's own duration
            var expireDate = CalculateExpireDateFromPackage(subscribeDate, package);

            var sql = @"
                INSERT INTO vehiclepackages (
                    id,
                    vehicle_id,
                    package_id,
                    subscribed_date,
                    expire_date,
                    created_by,
                    created_date,
                    updated_by,
                    updated_date
                )
                VALUES (
                    @Id,
                    @VehicleId,
                    @PackageId,
                    @SubscribedDate,
                    @ExpireDate,
                    @CreatedBy,
                    @CreatedDate,
                    @UpdatedBy,
                    @UpdatedDate
                )
                RETURNING *;";

            var vehiclePackage = new VehiclePackageDataModel
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicleId,
                PackageId = packageId,
                SubscribedDate = subscribeDate,
                ExpireDate = expireDate,
                // CreatedBy = _logedinUser?.UserId ?? "System",
                CreatedBy = "system",
                CreatedDate = DateTime.UtcNow,
               // UpdatedBy = _logedinUser?.UserId ?? "System",
                UpdatedBy = "System",
                UpdatedDate = DateTime.UtcNow
            };

            return await _connection.QueryFirstOrDefaultAsync<VehiclePackageDataModel>(sql, vehiclePackage);
        }

        // Add method to subscribe by package name
        public async Task<VehiclePackageDataModel?> SubscribeVehicleToPackageByNameAsync(Guid vehicleId, string packageName)
        {
            // First, find the package by name
            var sql = @"SELECT * FROM packages WHERE LOWER(package_type) = LOWER(@PackageName) AND is_active = true;";
            var parameters = new DynamicParameters();
            parameters.Add("@PackageName", packageName);

            var package = await _connection.QueryFirstOrDefaultAsync<PackageDataModel>(sql, parameters);

            if (package == null)
            {
                throw new ArgumentException($"Active package with name '{packageName}' not found");
            }

            // Use the existing method with the found package ID
            return await SubscribeVehicleToPackageAsync(vehicleId, package.Id);
        }

         private DateTime CalculateExpireDateFromPackage(DateTime subscribeDate, PackageDataModel package)
        {
            
            return package.Package_Type.ToLower() switch
            {
                "daily" => subscribeDate.AddDays(1),
                "weekly" => subscribeDate.AddDays(7),
                "monthly" => subscribeDate.AddMonths(1),
                "quarterly" => subscribeDate.AddMonths(3),
                "semi-annual" => subscribeDate.AddMonths(6),
                "annual" or "yearly" => subscribeDate.AddYears(1),
                "basic" => subscribeDate.AddDays(30),
                "premium" => subscribeDate.AddDays(90),
                "enterprise" => subscribeDate.AddYears(1),
                _ => subscribeDate.AddDays(30) // Default to 30 days
            };
        }

        public async Task<IEnumerable<PackageDataModel>> GetActivePackagesAsync()
        {
            var sql = @"
                SELECT * FROM Packages 
                WHERE is_active = true 
                ORDER BY package_cost ASC;";

            return await _connection.QueryAsync<PackageDataModel>(sql);
        }

        public async Task<IEnumerable<PackageDataModel>> GetPackagesByTypeAsync(string packageType)
        {
            var sql = @"
                SELECT * FROM Packages 
                WHERE package_type = @PackageType AND is_active = true
                ORDER BY package_cost ASC;";

            var parameters = new DynamicParameters();
            parameters.Add("@PackageType", packageType);

            return await _connection.QueryAsync<PackageDataModel>(sql, parameters);
        }

        public async Task<IEnumerable<PackageDataModel>> GetPackagesByCostRangeAsync(decimal minCost, decimal maxCost)
        {
            var sql = @"
                SELECT * FROM Packages 
                WHERE package_cost BETWEEN @MinCost AND @MaxCost 
                AND is_active = true
                ORDER BY package_cost ASC;";

            var parameters = new DynamicParameters();
            parameters.Add("@MinCost", minCost);
            parameters.Add("@MaxCost", maxCost);

            return await _connection.QueryAsync<PackageDataModel>(sql, parameters);
        }

        public async Task<PackageDataModel?> GetCheapestPackageAsync()
        {
            var sql = @"
                SELECT * FROM Packages 
                WHERE is_active = true 
                ORDER BY package_cost ASC 
                LIMIT 1;";

            return await _connection.QueryFirstOrDefaultAsync<PackageDataModel>(sql);
        }

        public async Task<PackageDataModel?> GetMostExpensivePackageAsync()
        {
            var sql = @"
                SELECT * FROM Packages 
                WHERE is_active = true 
                ORDER BY package_cost DESC 
                LIMIT 1;";

            return await _connection.QueryFirstOrDefaultAsync<PackageDataModel>(sql);
        }

        public async Task<int> GetPackageCountAsync()
        {
            var sql = @"SELECT COUNT(*) FROM Packages WHERE is_active = true;";
            return await _connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<int> GetPackageCountByTypeAsync(string packageType)
        {
            var sql = @"
                SELECT COUNT(*) FROM Packages 
                WHERE package_type = @PackageType AND is_active = true;";

            var parameters = new DynamicParameters();
            parameters.Add("@PackageType", packageType);

            return await _connection.ExecuteScalarAsync<int>(sql, parameters);
        }

        public async Task<bool> ActivatePackageAsync(Guid id)
        {
            var sql = @"
                UPDATE Packages SET 
                    is_active = true, 
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate 
                WHERE id = @Id;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
           // parameters.Add("@UpdatedBy", _loggedInUser.UserId);
            parameters.Add("@UpdatedBy", "system");
            parameters.Add("@UpdatedDate", DateTime.UtcNow);

            var affectedRows = await _connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public async Task<bool> DeactivatePackageAsync(Guid id)
        {
            var sql = @"
                UPDATE Packages SET 
                    is_active = false, 
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate 
                WHERE id = @Id;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
           // parameters.Add("@UpdatedBy", _loggedInUser.UserId);
            parameters.Add("@UpdatedBy", "system");
            parameters.Add("@UpdatedDate", DateTime.UtcNow);

            var affectedRows = await _connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public async Task<bool> UpdatePackageCostAsync(Guid id, decimal newCost)
        {
            var sql = @"
                UPDATE Packages SET 
                    package_cost = @NewCost, 
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate 
                WHERE id = @Id;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@NewCost", newCost);
            // parameters.Add("@UpdatedBy", _loggedInUser.UserId);
            parameters.Add("@UpdatedBy", "system");
            parameters.Add("@UpdatedDate", DateTime.UtcNow);

            var affectedRows = await _connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var sql = @"SELECT COUNT(1) FROM Packages WHERE id = @Id;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var count = await _connection.ExecuteScalarAsync<int>(sql, parameters);
            return count > 0;
        }

        public async Task<bool> ExtendVehiclePackageAsync(Guid vehiclePackageId, DateTime newExpireDate)
        {
            var sql = @"
                UPDATE VehiclePackages SET 
                    expire_date = @NewExpireDate,
                    updated_by = @UpdatedBy,
                    updated_date = @UpdatedDate
                WHERE id = @Id;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", vehiclePackageId);
            parameters.Add("@NewExpireDate", newExpireDate);
            parameters.Add("UpdatedBy", "system");
           // parameters.Add("@UpdatedBy", _loggedInUser.UserId);
            parameters.Add("@UpdatedDate", DateTime.UtcNow);

            var affectedRows = await _connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public async Task<IEnumerable<VehiclePackageDataModel>> GetExpiredPackagesAsync()
        {
            var sql = @"
                SELECT vp.*, p.package_type, p.package_cost, v.vehicle_number
                FROM VehiclePackages vp
                INNER JOIN Packages p ON vp.package_id = p.id
                INNER JOIN Vehicles v ON vp.vehicle_id = v.id
                WHERE vp.expire_date < @CurrentDate
                ORDER BY vp.expire_date DESC;";

            var parameters = new DynamicParameters();
            parameters.Add("@CurrentDate", DateTime.UtcNow);

            return await _connection.QueryAsync<VehiclePackageDataModel>(sql, parameters);
        }

        public async Task<int> GetSubscribedVehicleCountByPackageAsync(Guid packageId)
        {
            var sql = @"
                SELECT COUNT(DISTINCT vehicle_id) 
                FROM VehiclePackages 
                WHERE package_id = @PackageId 
                AND expire_date > @CurrentDate;";

            var parameters = new DynamicParameters();
            parameters.Add("@PackageId", packageId);
            parameters.Add("@CurrentDate", DateTime.UtcNow);

            return await _connection.ExecuteScalarAsync<int>(sql, parameters);
        }

        public async Task<bool> HasActiveSubscriptionAsync(Guid vehicleId, Guid packageId)
        {
            var sql = @"
                SELECT COUNT(1) 
                FROM VehiclePackages 
                WHERE vehicle_id = @VehicleId 
                AND package_id = @PackageId 
                AND expire_date > @CurrentDate;";

            var parameters = new DynamicParameters();
            parameters.Add("@VehicleId", vehicleId);
            parameters.Add("@PackageId", packageId);
            parameters.Add("@CurrentDate", DateTime.UtcNow);

            var count = await _connection.ExecuteScalarAsync<int>(sql, parameters);
            return count > 0;
        }

        
        public async Task<IEnumerable<PackagePopularityDataModel>> GetPackagePopularityStatsAsync()
        {
            var sql = @"
                SELECT 
                    p.id,
                    p.package_type,
                    p.package_cost,
                    COUNT(vp.id) as subscription_count,
                    COUNT(CASE WHEN vp.expire_date > @CurrentDate THEN 1 END) as active_subscriptions
                FROM Packages p
                LEFT JOIN VehiclePackages vp ON p.id = vp.package_id
                WHERE p.is_active = true
                GROUP BY p.id, p.package_type, p.package_cost
                ORDER BY subscription_count DESC;";

            var parameters = new DynamicParameters();
            parameters.Add("@CurrentDate", DateTime.UtcNow);

            return await _connection.QueryAsync<PackagePopularityDataModel>(sql, parameters);
        }

      
    }

}
