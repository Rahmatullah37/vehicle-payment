using AutoMapper;
using System;
using System.Collections.Generic;
using VehicleSurveillance.Data.Infrastructure;
using VehicleSurveillance.Data.Models;
using VehicleSurveillance.Domain.Constants;
using VehicleSurveillance.Domain.Models;
using VehicleSurveillance.Services.Interfaces;


namespace VehicleSurveillance.Services.Implementations
{
    public class FixedTarifService : IFixedTarifService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FixedTarifService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<FixedTarifModel> GetAll()
        {
            var dataList = _unitOfWork.FixedTarifRepository.GetAll();
            return _mapper.Map<List<FixedTarifModel>>(dataList);
        }

        public FixedTarifModel GetById(Guid id)
        {
            var data = _unitOfWork.FixedTarifRepository.GetById(id);
            if (data == null)
                throw new KeyNotFoundException($"FixedTarif with ID '{id}' was not found.");

            return _mapper.Map<FixedTarifModel>(data);
        }

        public void Add(FixedTarifModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                model.Created_By = AppConstants.Users.System;
                model.Updated_By = AppConstants.Users.System;
                model.Created_Date = DateTime.UtcNow;
                model.Updated_Date = DateTime.UtcNow;

                var dataModel = _mapper.Map<FixedTarifDataModel>(model);
                _unitOfWork.FixedTarifRepository.Create(dataModel);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void Update(FixedTarifModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                model.Updated_By = AppConstants.Users.System;
                model.Updated_Date = DateTime.UtcNow;

                var dataModel = _mapper.Map<FixedTarifDataModel>(model);
                _unitOfWork.FixedTarifRepository.Update(dataModel);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                _unitOfWork.FixedTarifRepository.Delete(id);
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
