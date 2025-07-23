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
    public class AccessFeeTransactionService : IAccessFeeTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AccessFeeTransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public List<AccessFeeTransactionModel> GetAll()
        {
            var dataModels = _unitOfWork.AccessFeeTransactionRepository.GetAll();
            return _mapper.Map<List<AccessFeeTransactionModel>>(dataModels);
        }
        public AccessFeeTransactionModel GetById(Guid id)
        {
            var dataModel = _unitOfWork.AccessFeeTransactionRepository.GetById(id);
            if (dataModel == null)
                throw new KeyNotFoundException($"Transaction with ID '{id}' was not found.");

            return _mapper.Map<AccessFeeTransactionModel>(dataModel);
        }
        public void Add(AccessFeeTransactionModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();
                model.Created_By = AppConstants.Users.System;
                model.Updated_By = AppConstants.Users.System;
                var dataModel = _mapper.Map<AccessFeeTransactionDataModel>(model);
                _unitOfWork.AccessFeeTransactionRepository.Create(dataModel);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
        public void Update(AccessFeeTransactionModel model)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                model.Updated_By = AppConstants.Users.System;
                model.Updated_Date = DateTime.UtcNow;

                var dataModel = _mapper.Map<AccessFeeTransactionDataModel>(model);
                _unitOfWork.AccessFeeTransactionRepository.Update(dataModel);
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

                _unitOfWork.AccessFeeTransactionRepository.Delete(id);
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
        public FixedTarifModel FindFixedTarifByVehicleType(string vehicleTypeName)
        {
            var vehicleType = _unitOfWork.VehicleTypeRepository.GetByName(vehicleTypeName)
                ?? throw new KeyNotFoundException($"No vehicle type found with name: {vehicleTypeName}");

            var tarifType = _unitOfWork.TarifTypeRepository.GetFixedTarifType()
                ?? throw new KeyNotFoundException("Tarif type 'Fixed' not found.");

            var tarif = _unitOfWork.TarifRepository.GetTarif(vehicleType.Id, tarifType.Id)
                ?? throw new KeyNotFoundException($"No active tariff found for vehicle type: {vehicleTypeName}");

            var fixedTarif = _unitOfWork.FixedTarifRepository.GetByTarifId(tarif.Id)
                ?? throw new KeyNotFoundException("No fixed tariff configured for this vehicle type.");

            return _mapper.Map<FixedTarifModel>(fixedTarif);
        }
        public VehicleTarifModel CalculateAndSaveTarif(string vehicleTypeName, string paymentMode)
        {
            //  Validate Payment Mode
            var validMode = _unitOfWork.PaymentModeRepository
                .GetPaymentByName(paymentMode)
                ?? throw new KeyNotFoundException($"Invalid payment mode: {paymentMode}");

            //  Find Fixed Tarif
            var fixedTarif = FindFixedTarifByVehicleType(vehicleTypeName);

            //  Get Vehicle Type
            var vehicle = _unitOfWork.VehicleTypeRepository
                .GetByName(vehicleTypeName)
                ?? throw new KeyNotFoundException($"No vehicle type found with name: {vehicleTypeName}");

            //  Prepare Access Fee Transaction
            var transaction = new AccessFeeTransactionModel
            { 
                AmountCharged = fixedTarif.Amount,
                Vehicle_Category = vehicleTypeName,
                Payment_Mode = paymentMode,
                Created_Date = DateTime.UtcNow,
                Updated_Date = DateTime.UtcNow,               
                Created_By = AppConstants.Users.System,
                Updated_By = AppConstants.Users.System,
                Is_Active = true
            };

            if (_unitOfWork.Transaction == null)
                _unitOfWork.BeginTransaction();
            _unitOfWork.AccessFeeTransactionRepository
                .Create(_mapper.Map<AccessFeeTransactionDataModel>(transaction));

            _unitOfWork.Commit();

            //  Return calculated result
            return new VehicleTarifModel
            {
                VehicleType = vehicleTypeName,
                Amount = fixedTarif.Amount
            };
        }
        public VehicleTarifModel CalculateHourlyTarifByName(string vehicleTypeName, DateTime fromTime, DateTime toTime, string paymentMode)
        {
            // 1. Validate Payment Mode
            var validMode = _unitOfWork.PaymentModeRepository.GetPaymentByName(paymentMode)
                ?? throw new KeyNotFoundException($"Invalid payment mode: {paymentMode}");

            // 2. Get Vehicle Type by name
            var vehicle = _unitOfWork.VehicleTypeRepository.GetByName(vehicleTypeName)
                ?? throw new KeyNotFoundException($"No vehicle type found with name: {vehicleTypeName}");

            // 3. Get TarifType = Hourly
            var tarifType = _unitOfWork.TarifTypeRepository.GetHourlyTarifType()
                ?? throw new KeyNotFoundException("Tarif type 'Hourly' not found.");

            // 4. Get Tarif for this vehicle and type
            var tarif = _unitOfWork.TarifRepository.GetTarif(vehicle.Id, tarifType.Id)
                ?? throw new KeyNotFoundException($"No hourly tarif found for vehicle type: {vehicleTypeName}");

            // 5. Calculate total hours using DateTime
            
            int totalHours = (int)Math.Ceiling((toTime - fromTime).TotalHours);            
            if (totalHours <= 0)
                throw new Exception("ToTime must be greater than FromTime");
            var duration = toTime - fromTime;
            string formattedDuration = $"{(int)duration.TotalHours}:{duration.Minutes:D2}";

            // 6. Get amount using new hourly logic
            var amount = _unitOfWork.TarifTypeRepository.GetAmountByTarif(tarif.Id, TarifTypeEnum.Hourly, totalHours);


            // 7. Save transaction if needed
            var transaction = new AccessFeeTransactionModel
            {
                AmountCharged = amount,
                Vehicle_Category = vehicleTypeName,
                Payment_Mode = paymentMode,
                Created_Date = DateTime.UtcNow,
                Updated_Date = DateTime.UtcNow,
                Created_By = AppConstants.Users.System,
                Updated_By = AppConstants.Users.System,
                Is_Active = true
            };

            if (_unitOfWork.Transaction == null)
                _unitOfWork.BeginTransaction();
            _unitOfWork.AccessFeeTransactionRepository
                .Create(_mapper.Map<AccessFeeTransactionDataModel>(transaction));

            _unitOfWork.Commit();

            // 8. Return calculated result
            return new VehicleTarifModel
            {
                VehicleType = vehicleTypeName,
                Amount = amount,
                totalhours = (int)Math.Ceiling(duration.TotalHours), 
                TotalDuration = $"{(int)duration.TotalHours}:{duration.Minutes:D2}"
            };
        }










        public TransactionReportResponse GetTransactionReport(TransactionReportRequest summary)
        {
            return _unitOfWork.AccessFeeTransactionRepository.GetTransactionReport(summary);
        }
    }
}
