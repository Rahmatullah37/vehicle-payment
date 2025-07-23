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
    public class HourlyTarifService:IHourlyTarifService
    {
       
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public HourlyTarifService(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public List<HourlyTarifModel> GetAll()
            {
                var dataList = _unitOfWork.HourlyTarifRepository.GetAll();
                return _mapper.Map<List<HourlyTarifModel>>(dataList);
            }

            public HourlyTarifModel GetById(Guid id)
            {
                var data = _unitOfWork.FixedTarifRepository.GetById(id);
                if (data == null)
                    throw new KeyNotFoundException($"FixedTarif with ID '{id}' was not found.");

                return _mapper.Map<HourlyTarifModel>(data);
            }

            public void Add(HourlyTarifModel model)
            {
                try
                {
                    if (_unitOfWork.Transaction == null)
                        _unitOfWork.BeginTransaction();

                    model.Created_By = AppConstants.Users.System;
                    model.Updated_By = AppConstants.Users.System;
                    model.Created_Date = DateTime.UtcNow;
                    model.Updated_Date = DateTime.UtcNow;

                    var dataModel = _mapper.Map<HourlyTarifDataModel>(model);
                    _unitOfWork.HourlyTarifRepository.Create(dataModel);
                    _unitOfWork.Commit();
                }
                catch
                {
                    _unitOfWork.Rollback();
                    throw;
                }
            }

            public void Update(HourlyTarifModel model)
            {
                try
                {
                    if (_unitOfWork.Transaction == null)
                        _unitOfWork.BeginTransaction();

                    model.Updated_By = AppConstants.Users.System;
                    model.Updated_Date = DateTime.UtcNow;

                    var dataModel = _mapper.Map<HourlyTarifDataModel>(model);
                    _unitOfWork.HourlyTarifRepository.Update(dataModel);
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

                    _unitOfWork.HourlyTarifRepository.Delete(id);
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
