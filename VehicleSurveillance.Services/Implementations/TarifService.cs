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
    public class TarifService : ITarifService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TarifService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TarifModel>> GetAllAsync()
        {
            var dataList = await _unitOfWork.TarifRepository.GetAll();
            return _mapper.Map<List<TarifModel>>(dataList);
        }

        public async Task<OneOf<TarifModel, ValidationResult>?> GetByIdAsync(Guid id)
        {
            var data = await _unitOfWork.TarifRepository.GetById(id);
            if (data == null)
            {
                var validation = new ValidationResult();
                validation.Errors.Add(new ValidationError("id", $"Tarif with ID '{id}' not found."));
                return validation;
            }

            return _mapper.Map<TarifModel>(data);
        }

        public async Task<OneOf<TarifModel, ValidationResult>?> AddAsync(TarifModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

               

                var dataModel = _mapper.Map<TarifDataModel>(model);
                await _unitOfWork.TarifRepository.Create(dataModel);
                await _unitOfWork.CommitAsync();

                return model;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<OneOf<TarifModel, ValidationResult>?> UpdateAsync(TarifModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                

                var dataModel = _mapper.Map<TarifDataModel>(model);
                await _unitOfWork.TarifRepository.Update(dataModel);
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

                await _unitOfWork.TarifRepository.Delete(id);
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
