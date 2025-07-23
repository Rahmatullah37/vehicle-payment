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
    public class VehicleTypeService:IVehicleTypeService
    {
       
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public VehicleTypeService(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public List<VehicleTypeModel> GetAll()
            {
                var dataList = _unitOfWork.VehicleTypeRepository.GetAll();
                return _mapper.Map<List<VehicleTypeModel>>(dataList);
            }

            public VehicleTypeModel GetById(Guid id)
            {
                var data = _unitOfWork.VehicleTypeRepository.GetById(id);
                if (data == null)
                    throw new KeyNotFoundException($"Tarif with ID '{id}' was not found.");
                return _mapper.Map<VehicleTypeModel>(data);
            }

            public void Add(VehicleTypeModel model)
            {
                try
                {
                    if (_unitOfWork.Transaction == null)
                        _unitOfWork.BeginTransaction();

                    model.Created_By = AppConstants.Users.System;
                    model.Updated_By = AppConstants.Users.System;
                    

                    var dataModel = _mapper.Map<VehicleTypeDataModel>(model);
                    _unitOfWork.VehicleTypeRepository.Create(dataModel);
                    _unitOfWork.Commit();
                }
                catch
                {
                    _unitOfWork.Rollback();
                    throw;
                }
            }

            public void Update(VehicleTypeModel model)
            {
                try
                {
                    if (_unitOfWork.Transaction == null)
                        _unitOfWork.BeginTransaction();

                    model.Updated_By = AppConstants.Users.System;
                    model.Updated_Date = DateTime.UtcNow;

                    var dataModel = _mapper.Map<VehicleTypeDataModel>(model);
                    _unitOfWork.VehicleTypeRepository.Update(dataModel);
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

                    _unitOfWork.VehicleTypeRepository.Delete(id);
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

