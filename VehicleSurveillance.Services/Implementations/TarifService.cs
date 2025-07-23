using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Data.Infrastructure;
using VehicleSurveillance.Data.Models;
using VehicleSurveillance.Domain.Constants;
using VehicleSurveillance.Domain.Models;
using VehicleSurveillance.Services.Interfaces;

namespace VehicleSurveillance.Services.Implementations
{
    public class TarifService : ITarifService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TarifService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<TarifModel> GetAll()
        {
            var dataList = _unitOfWork.TarifRepository.GetAll();
            return _mapper.Map<List<TarifModel>>(dataList);
        }

        public TarifModel GetById(Guid id)
        {
            var data = _unitOfWork.TarifRepository.GetById(id);
            if (data == null)
                throw new KeyNotFoundException($"Tarif with ID '{id}' was not found.");
            return _mapper.Map<TarifModel>(data);
        }

        public void Add(TarifModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                model.Created_By = AppConstants.Users.System;
                model.Updated_By = AppConstants.Users.System;
                model.Created_Date = DateTime.UtcNow;
                model.Updated_Date = DateTime.UtcNow;

                var dataModel = _mapper.Map<TarifDataModel>(model);
                _unitOfWork.TarifRepository.Create(dataModel);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void Update(TarifModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                model.Updated_By = AppConstants.Users.System;
                model.Updated_Date = DateTime.UtcNow;

                var dataModel = _mapper.Map<TarifDataModel>(model);
                _unitOfWork.TarifRepository.Update(dataModel);
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

                _unitOfWork.TarifRepository.Delete(id);
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
