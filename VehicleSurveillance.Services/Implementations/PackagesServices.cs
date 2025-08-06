
using AutoMapper;
using OneOf;
using VisualSoft.Surveillance.Payment.Data.Infrastructure;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Constants;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Domain.Utils;
using VisualSoft.Surveillance.Payment.Services.Interfaces;

namespace VisualSoft.Surveillance.Payment.Services.Implementations
{
    public class PackagesServices : IPackagesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserIdentificationModel _loggedInUser;

        public PackagesServices(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserIdentificationModel loggedInUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedInUser = loggedInUser;
        }

        public async Task<List<PackageModel>> GetAllAsync()
        {
            var dataList = await _unitOfWork.PackagesRepository.GetAllAsync();
            return _mapper.Map<List<PackageModel>>(dataList);
        }

        public async Task<OneOf<PackageModel, ValidationResult>> GetByIdAsync(Guid id)
        {
            var data = await _unitOfWork.PackagesRepository.GetByIdAsync(id);
            if (data == null)
            {
                var validation = new ValidationResult();
                validation.Errors.Add(new ValidationError("id", $"{Constants.Errors.ERROR_DETECTION_INVALID} Id = {id}"));
                return validation;
            }

            return _mapper.Map<PackageModel>(data);
        }
        public async Task<OneOf<PackageModel, ValidationResult>> AddAsync(PackageModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();


                var dataModel = _mapper.Map<PackageDataModel>(model);
                await _unitOfWork.PackagesRepository.CreateAsync(dataModel);
                await _unitOfWork.CommitAsync();
                return model;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<OneOf<PackageModel, ValidationResult>> UpdateAsync(PackageModel model)
        {
            var existing = await _unitOfWork.PackagesRepository.GetByIdAsync(model.Id);
            if (existing == null)
            {
                var validation = new ValidationResult();
                validation.Errors.Add(new ValidationError("id", $"{Constants.Errors.ERROR_DETECTION_INVALID} Id = {model.Id}"));
                return validation;
            }

            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();
                var dataModel = _mapper.Map<PackageDataModel>(model);
                await _unitOfWork.PackagesRepository.UpdateAsync(dataModel);

                await _unitOfWork.CommitAsync();
                return model;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                await _unitOfWork.PackagesRepository.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        //public async Task<List<VehiclePackageModel>> SubscribeVehicleToPackageAsync(Guid vehicleId, Guid packageId)
        //{
        //    var
        //}
        public async Task<List<PackageModel>> GetActivePackagesAsync()
        {
            var dataList = await _unitOfWork.PackagesRepository.GetActivePackagesAsync();
            return _mapper.Map<List<PackageModel>>(dataList);
        }

        public async Task<List<PackageModel>> GetPackagesByTypeAsync(string packageType)
        {
            var dataList = await _unitOfWork.PackagesRepository.GetPackagesByTypeAsync(packageType);
            return _mapper.Map<List<PackageModel>>(dataList);
        }

        public async Task<List<PackageModel>> GetPackagesByCostRangeAsync(decimal minCost, decimal maxCost)
        {
            var dataList = await _unitOfWork.PackagesRepository.GetPackagesByCostRangeAsync(minCost, maxCost);
            return _mapper.Map<List<PackageModel>>(dataList);
        }

        public async Task<PackageModel?> GetCheapestPackageAsync()
        {
            var data = await _unitOfWork.PackagesRepository.GetCheapestPackageAsync();
            return data != null ? _mapper.Map<PackageModel>(data) : null;
        }

        public async Task<PackageModel?> GetMostExpensivePackageAsync()
        {
            var data = await _unitOfWork.PackagesRepository.GetMostExpensivePackageAsync();
            return data != null ? _mapper.Map<PackageModel>(data) : null;
        }

        public async Task<int> GetPackageCountAsync()
        {
            return await _unitOfWork.PackagesRepository.GetPackageCountAsync();
        }

        public async Task<int> GetPackageCountByTypeAsync(string packageType)
        {
            return await _unitOfWork.PackagesRepository.GetPackageCountByTypeAsync(packageType);
        }
        public async Task<OneOf<bool, ValidationResult>> ActivatePackageAsync(Guid id)
        {
            var existing = await _unitOfWork.PackagesRepository.GetByIdAsync(id);
            if (existing == null)
            {
                var validation = new ValidationResult();
                validation.Errors.Add(new ValidationError("id", $"{Constants.Errors.ERROR_DETECTION_INVALID} Id = {id}"));
                return validation;
            }

            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                var result = await _unitOfWork.PackagesRepository.ActivatePackageAsync(id);
                await _unitOfWork.CommitAsync();

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<OneOf<bool, ValidationResult>> DeactivatePackageAsync(Guid id)
        {
            var existing = await _unitOfWork.PackagesRepository.GetByIdAsync(id);
            if (existing == null)
            {
                var validation = new ValidationResult();
                validation.Errors.Add(new ValidationError("id", $"{Constants.Errors.ERROR_DETECTION_INVALID} Id = {id}"));
                return validation;
            }

            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                var result = await _unitOfWork.PackagesRepository.DeactivatePackageAsync(id);
                await _unitOfWork.CommitAsync();

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<OneOf<bool, ValidationResult>> UpdatePackageCostAsync(Guid id, decimal newCost)
        {
            if (newCost <= 0)
            {
                var validation = new ValidationResult();
                validation.Errors.Add(new ValidationError("cost", "Package cost must be greater than zero"));
                return validation;
            }

            var existing = await _unitOfWork.PackagesRepository.GetByIdAsync(id);
            if (existing == null)
            {
                var validation = new ValidationResult();
                validation.Errors.Add(new ValidationError("id", $"{Constants.Errors.ERROR_DETECTION_INVALID} Id = {id}"));
                return validation;
            }

            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                var result = await _unitOfWork.PackagesRepository.UpdatePackageCostAsync(id, newCost);
                await _unitOfWork.CommitAsync();

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _unitOfWork.PackagesRepository.ExistsAsync(id);
        }
        public async Task<OneOf<bool, ValidationResult>> ExtendVehiclePackageAsync(Guid vehiclePackageId, DateTime newExpireDate)
        {
            if (newExpireDate <= DateTime.UtcNow)
            {
                var validation = new ValidationResult();
                validation.Errors.Add(new ValidationError("expireDate", "Expire date must be in the future"));
                return validation;
            }

            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                var result = await _unitOfWork.PackagesRepository.ExtendVehiclePackageAsync(vehiclePackageId, newExpireDate);
                await _unitOfWork.CommitAsync();

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<List<VehiclePackageModel>> GetExpiredPackagesAsync()
        {
            var dataList = await _unitOfWork.PackagesRepository.GetExpiredPackagesAsync();
            return _mapper.Map<List<VehiclePackageModel>>(dataList);
        }

        public async Task<int> GetSubscribedVehicleCountByPackageAsync(Guid packageId)
        {
            return await _unitOfWork.PackagesRepository.GetSubscribedVehicleCountByPackageAsync(packageId);
        }

        public async Task<bool> HasActiveSubscriptionAsync(Guid vehicleId, Guid packageId)
        {
            return await _unitOfWork.PackagesRepository.HasActiveSubscriptionAsync(vehicleId, packageId);
        }
        public async Task<List<PackagePopularityDomainModel>> GetPackagePopularityStatsAsync()
        {
            var dataList = await _unitOfWork.PackagesRepository.GetPackagePopularityStatsAsync();
            return _mapper.Map<List<PackagePopularityDomainModel>>(dataList);
        }
       

    }
}
