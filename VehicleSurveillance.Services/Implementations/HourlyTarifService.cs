
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
    public class HourlyTarifService : IHourlyTarifService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserIdentificationModel _loggedInUser;

        public HourlyTarifService(IUnitOfWork unitOfWork, IMapper mapper, IUserIdentificationModel loggedInUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedInUser = loggedInUser;
        }

        public async Task<List<HourlyTarifModel>> GetAllAsync()
        {
            var dataList = await _unitOfWork.HourlyTarifRepository.GetAll();
            return _mapper.Map<List<HourlyTarifModel>>(dataList);
        }

        public async Task<HourlyTarifModel?> GetByIdAsync(Guid id)
        {
            var data = await _unitOfWork.HourlyTarifRepository.GetById(id);
            if (data == null)
                return null;

            return _mapper.Map<HourlyTarifModel>(data);
        }

        public async Task<OneOf<HourlyTarifModel, ValidationResult>> AddAsync(HourlyTarifModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                

                var dataModel = _mapper.Map<HourlyTarifDataModel>(model);
                await _unitOfWork.HourlyTarifRepository.Create(dataModel);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HourlyTarifModel>(dataModel);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<OneOf<HourlyTarifModel, ValidationResult>> UpdateAsync(HourlyTarifModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();


                var dataModel = _mapper.Map<HourlyTarifDataModel>(model);
                await _unitOfWork.HourlyTarifRepository.Update(dataModel);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HourlyTarifModel>(dataModel);
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

                await _unitOfWork.HourlyTarifRepository.Delete(id);
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
