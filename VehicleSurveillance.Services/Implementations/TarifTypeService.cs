using AutoMapper;
using OneOf;
using VisualSoft.Surveillance.Payment.Data.Infrastructure;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Services.Interfaces;
using VisualSoft.Surveillance.Payment.Domain.Utils;

namespace VisualSoft.Surveillance.Payment.Services.Implementations
{
    public class TarifTypeService : ITarifTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TarifTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TarifTypeModel>> GetAllAsync()
        {
            var dataList = await _unitOfWork.TarifTypeRepository.GetAll();
            return _mapper.Map<List<TarifTypeModel>>(dataList);
        }

        public async Task<OneOf<TarifTypeModel, ValidationResult>> GetByIdAsync(Guid id)
        {
            var data = await _unitOfWork.TarifTypeRepository.GetById(id);
            if (data == null)
            {
                var validation = new ValidationResult();
                validation.Errors.Add(new ValidationError("id", $"Tarif with ID '{id}' not found."));
                return validation;
            }

            return _mapper.Map<TarifTypeModel>(data);
        }

        public async Task<OneOf<TarifTypeModel, ValidationResult>> AddAsync(TarifTypeModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();



                var dataModel = _mapper.Map<TarifTypeDataModel>(model);
                await _unitOfWork.TarifTypeRepository.Create(dataModel);
                await _unitOfWork.CommitAsync();

                return model;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<OneOf<TarifTypeModel, ValidationResult>> UpdateAsync(TarifTypeModel model)
        {

            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();



                var dataModel = _mapper.Map<TarifTypeDataModel>(model);
                await _unitOfWork.TarifTypeRepository.Update(dataModel);
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

                await _unitOfWork.TarifTypeRepository.Delete(id);
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
