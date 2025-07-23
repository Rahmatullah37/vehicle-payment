using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using VehicleSurveillance.Data.Infrastructure;
using VehicleSurveillance.Data.Models;
using VehicleSurveillance.Domain.Constants;
using VehicleSurveillance.Domain.Models;
using VehicleSurveillance.Services.Interfaces;

namespace VehicleSurveillance.Services.Implementations
{
    public class PackagesServices : IPackagesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PackagesServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<PackageModel> GetAllPackages()
        {
            var dataList = _unitOfWork.PackagesRepository.GetAll();
            return _mapper.Map<List<PackageModel>>(dataList);
        }

        public PackageModel GetPackageById(Guid id)
        {
            var data = _unitOfWork.PackagesRepository.GetById(id);
            if (data == null)
                throw new KeyNotFoundException($"Package with ID '{id}' was not found.");
            return _mapper.Map<PackageModel>(data);
        }

        public void AddPackage(PackageModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                model.Created_By = AppConstants.Users.System;
                model.Updated_By = AppConstants.Users.System;
                model.Created_Date = DateTime.UtcNow;
                model.Updated_Date = DateTime.UtcNow;

                var dataModel = _mapper.Map<PackageDataModel>(model);
                _unitOfWork.PackagesRepository.Create(dataModel);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void UpdatePackage(PackageModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                model.Updated_By = AppConstants.Users.System;
                model.Updated_Date = DateTime.UtcNow;

                var dataModel = _mapper.Map<PackageDataModel>(model);
                _unitOfWork.PackagesRepository.Update(dataModel);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void DeletePackage(Guid id)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                _unitOfWork.PackagesRepository.Delete(id);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
