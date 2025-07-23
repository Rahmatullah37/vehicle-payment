using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Data.Infrastructure;
using VehicleSurveillance.Data.Models;
using VehicleSurveillance.Data.Repositories;
using VehicleSurveillance.Domain.Constants;
using VehicleSurveillance.Domain.Models;
using VehicleSurveillance.Services.Interfaces;

namespace VehicleSurveillance.Services.Implementations
{
    public class TarifTypeService:ITarifTypeService
    {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public TarifTypeService(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public List<TarifTypeModel> GetAll()
            {
                var dataList = _unitOfWork.TarifTypeRepository.GetAll();
                return _mapper.Map<List<TarifTypeModel>>(dataList);
            }

            public TarifTypeModel GetById(Guid id)
            {
                var data = _unitOfWork.TarifTypeRepository.GetById(id);
                if (data == null)
                    throw new KeyNotFoundException($"Tarif with ID '{id}' was not found.");
                return _mapper.Map<TarifTypeModel>(data);
            }

            public void Add(TarifTypeModel model)
            {
                try
                {
                    if (_unitOfWork.Transaction == null)
                        _unitOfWork.BeginTransaction();

                    model.Created_By = AppConstants.Users.System;
                    model.Updated_By = AppConstants.Users.System;


                    var dataModel = _mapper.Map<TarifTypeDataModel>(model);
                    _unitOfWork.TarifTypeRepository.Create(dataModel);
                    _unitOfWork.Commit();
                }
                catch
                {
                    _unitOfWork.Rollback();
                    throw;
                }
            }

            public void Update(TarifTypeModel model)
            {
                try
                {
                    if (_unitOfWork.Transaction == null)
                        _unitOfWork.BeginTransaction();

                    model.Updated_By = AppConstants.Users.System;
                    model.Updated_Date = DateTime.UtcNow;

                    var dataModel = _mapper.Map<TarifTypeDataModel>(model);
                    _unitOfWork.TarifTypeRepository.Update(dataModel);
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

                    _unitOfWork.TarifTypeRepository.Delete(id);
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

