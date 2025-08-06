
using AutoMapper;
using OneOf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Infrastructure;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Constants;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Domain.Utils;
using VisualSoft.Surveillance.Payment.Services.Interfaces;

namespace VisualSoft.Surveillance.Payment.Services.Implementations
{
    public class FixedTarifService : IFixedTarifService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserIdentificationModel _loggedInUser;

        public FixedTarifService(IUnitOfWork unitOfWork, IMapper mapper, IUserIdentificationModel loggedInUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedInUser = loggedInUser;
        }
        public async Task<List<FixedTarifModel>> GetAllAsync()
        {
            var dataList = await _unitOfWork.FixedTarifRepository.GetAll();
            return _mapper.Map<List<FixedTarifModel>>(dataList);
        }
        public async Task<OneOf<FixedTarifModel, ValidationResult>> GetByIdAsync(Guid id)
        {
            var existing = await _unitOfWork.FixedTarifRepository.GetById(id);
            if (existing == null)
            {
                var validationResult = new ValidationResult();
                validationResult.Errors.Add(new ValidationError("id", $"{Constants.Errors.ERROR_DETECTION_INVALID} Id = {id}"));
                return validationResult;
            }

            var model = _mapper.Map<FixedTarifModel>(existing);
            return model;
        }
        public async Task<OneOf<FixedTarifModel, ValidationResult>> AddAsync(FixedTarifModel model)
       
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

             

                var dataModel = _mapper.Map<FixedTarifDataModel>(model);
                await _unitOfWork.FixedTarifRepository.Create(dataModel);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<FixedTarifModel>(dataModel);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task<OneOf<FixedTarifModel, ValidationResult>> UpdateAsync(FixedTarifModel model)
      
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

               

                var dataModel = _mapper.Map<FixedTarifDataModel>(model);
                await _unitOfWork.FixedTarifRepository.Update(dataModel);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<FixedTarifModel>(dataModel);
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

                await _unitOfWork.FixedTarifRepository.Delete(id);
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
