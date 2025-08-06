
using AutoMapper;
using OneOf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Infrastructure;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Domain.Utils;
using VisualSoft.Surveillance.Payment.Services.Interfaces;

namespace VisualSoft.Surveillance.Payment.Services.Implementations
{
    public class PaymentModeService : IPaymentModeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserIdentificationModel _loggedInUser;

        public PaymentModeService(IUnitOfWork unitOfWork, IMapper mapper, IUserIdentificationModel loggedInUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedInUser = loggedInUser;
        }

        public async Task<List<PaymentModeModel>> GetAllAsync()
        {
            var dataList = await _unitOfWork.PaymentModeRepository.GetAll();
            return _mapper.Map<List<PaymentModeModel>>(dataList);
        }

        public async Task<OneOf<PaymentModeModel, ValidationResult>> GetByIdAsync(Guid id)
        {
            var data = await _unitOfWork.PaymentModeRepository.GetById(id);
            if (data == null)
            {
                var validation = new ValidationResult();
                validation.Errors.Add(new ValidationError("id", $"Payment mode with ID '{id}' not found."));
                return validation;
            }

            return _mapper.Map<PaymentModeModel>(data);
        }

        public async Task<OneOf<PaymentModeModel, ValidationResult>> AddAsync(PaymentModeModel mode)
        {
            var model = _mapper.Map<PaymentModeModel>(mode);
            
            
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                var dataModel = _mapper.Map<PaymentModeDataModel>(model);
                await _unitOfWork.PaymentModeRepository.Create(dataModel);
                await _unitOfWork.CommitAsync();

                return model;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<OneOf<PaymentModeModel, ValidationResult>> UpdateAsync(PaymentModeModel mode)
        {
            var model = _mapper.Map<PaymentModeModel>(mode);
            var existing = await _unitOfWork.PaymentModeRepository.GetById(model.Id);
            if (existing == null)
            {
                var validation = new ValidationResult();
                validation.Errors.Add(new ValidationError("id", $"Payment mode with ID '{model.Id}' not found."));
                return validation;
            }

            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                var dataModel = _mapper.Map<PaymentModeDataModel>(model);
                await _unitOfWork.PaymentModeRepository.Update(dataModel);
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

                await _unitOfWork.PaymentModeRepository.Delete(id);
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
