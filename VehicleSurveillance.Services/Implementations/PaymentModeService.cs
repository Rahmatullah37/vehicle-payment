using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Data.Infrastructure;
using VehicleSurveillance.Data.Models;
using VehicleSurveillance.Domain.Models;
using VehicleSurveillance.Services.Interfaces;

namespace VehicleSurveillance.Services.Implementations
{
    public class PaymentModeService : IPaymentModeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentModeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<PaymentModeModel> GetAll()
        {
            var dataList = _unitOfWork.PaymentModeRepository.GetAll();
            return _mapper.Map<List<PaymentModeModel>>(dataList);
        }

        public PaymentModeModel GetById(Guid id)
        {
            var data = _unitOfWork.PaymentModeRepository.GetById(id);
            if (data == null)
                throw new KeyNotFoundException($"Payment mode with ID '{id}' not found.");
            return _mapper.Map<PaymentModeModel>(data);
        }

        public void Add(PaymentModeModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                model.Id = Guid.NewGuid();
                var dataModel = _mapper.Map<PaymentModeDataModel>(model);

                _unitOfWork.PaymentModeRepository.Create(dataModel);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void Update(PaymentModeModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                var dataModel = _mapper.Map<PaymentModeDataModel>(model);

                _unitOfWork.PaymentModeRepository.Update(dataModel);
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

                _unitOfWork.PaymentModeRepository.Delete(id);
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
