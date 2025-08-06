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
    public class VehicleTypeService : IVehicleTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<VehicleTypeModel>> GetAllAsync()
        {
            var dataList = await _unitOfWork.VehicleTypeRepository.GetAll();
            return _mapper.Map<List<VehicleTypeModel>>(dataList);
        }

        public async Task<OneOf<VehicleTypeModel, ValidationResult>> GetByIdAsync(Guid id)
        {
            var data = await _unitOfWork.VehicleTypeRepository.GetById(id);
            if (data == null)
            {
                var validation = new ValidationResult();
                validation.Errors.Add(new ValidationError("id", $"Vehicle type with ID '{id}' was not found."));
                return validation;
            }

            return _mapper.Map<VehicleTypeModel>(data);
        }

        public async Task<OneOf<VehicleTypeModel, ValidationResult>> AddAsync(VehicleTypeModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                var dataModel = _mapper.Map<VehicleTypeDataModel>(model);
                await _unitOfWork.VehicleTypeRepository.Create(dataModel);
                await _unitOfWork.CommitAsync();

                return model;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<OneOf<VehicleTypeModel, ValidationResult>> UpdateAsync(VehicleTypeModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                var dataModel = _mapper.Map<VehicleTypeDataModel>(model);
                await _unitOfWork.VehicleTypeRepository.Update(dataModel);
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

                await _unitOfWork.VehicleTypeRepository.Delete(id);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
