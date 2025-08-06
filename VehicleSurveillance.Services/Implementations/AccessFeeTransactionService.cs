
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneOf;
using VisualSoft.Surveillance.Payment.Data.Infrastructure;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Constants;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Services.Interfaces;
using VisualSoft.Surveillance.Payment.Domain.Utils;

namespace VisualSoft.Surveillance.Payment.Services.Implementations
{
    public class AccessFeeTransactionService : IAccessFeeTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IUserIdentificationModel _loggedInUser;
        private readonly IVehicleAccountService _vehicleAccountService;


        // private readonly IVehiclePackagesRepository _vehiclePackagesRepository;

        public AccessFeeTransactionService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IUserIdentificationModel loggedInUser,
            IVehicleAccountService vehicleAccountService
             //IVehiclePackagesRepository vehiclePackagesRepository
            )

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedInUser = loggedInUser;
            _vehicleAccountService = vehicleAccountService;
            //_vehiclePackagesRepository = vehiclePackagesRepository;
        }
        public async Task<List<AccessFeeTransactionModel>> GetAllAsync()
        {
            var dataModels = await _unitOfWork.AccessFeeTransactionRepository.GetAll();
            return _mapper.Map<List<AccessFeeTransactionModel>>(dataModels);

        }
        public async Task<OneOf<AccessFeeTransactionModel, ValidationResult>> GetByIdAsync(Guid id)
        {
            var dataModel = await _unitOfWork.AccessFeeTransactionRepository.GetById(id);
            if (dataModel == null)
            {

                var validationResult = new ValidationResult();
                validationResult.Errors.Add(new ValidationError("id", $"{Constants.Errors.ERROR_DETECTION_INVALID} Id = {id}")); ;
                return validationResult;
            }

            return _mapper.Map<AccessFeeTransactionModel>(dataModel);
        }
        public async Task<OneOf<AccessFeeTransactionModel, ValidationResult>> AddAsync(AccessFeeTransactionModel model)

        {


            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();
                var dataModel = _mapper.Map<AccessFeeTransactionDataModel>(model);
                await _unitOfWork.AccessFeeTransactionRepository.Create(dataModel);
                _unitOfWork.Commit();
                return  model;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
           
        }     
        public async Task<OneOf<AccessFeeTransactionModel, ValidationResult>> UpdateAsync(AccessFeeTransactionModel model)
        {
            var existing = await _unitOfWork.AccessFeeTransactionRepository.GetById(model.Id);
            if (existing == null)
            {

                var validationResult = new ValidationResult();
                validationResult.Errors.Add(new ValidationError("id", $"{Constants.Errors.ERROR_DETECTION_INVALID} Id = ")); ;
                return validationResult;
            }
            try
            {
                var dataModel = _mapper.Map<AccessFeeTransactionDataModel>(model);
                await _unitOfWork.AccessFeeTransactionRepository.Update(dataModel);
                _unitOfWork.Commit();
                return model;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                if (_unitOfWork.Transaction == null)
                    _unitOfWork.BeginTransaction();

                await _unitOfWork.AccessFeeTransactionRepository.Delete(id);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        // Package checking methods
        private async Task<VehiclePackageDataModel> GetActivePackageForVehicleAsync(Guid vehicleId)
        {
            return await _unitOfWork.VehicalPackageRepository.GetActivePackageByVehicleId(vehicleId);
        }

        private async Task<bool> HasActivePackageAsync(Guid vehicleId)
        {
            return await _unitOfWork.VehicalPackageRepository.HasActivePackage(vehicleId);
        }

        private async Task SaveTransactionAsync(decimal amount, string vehicleType, string paymentMode)
        {
            if (_unitOfWork.Transaction == null)
                _unitOfWork.BeginTransaction();

            var transaction = new AccessFeeTransactionModel
            {
                AmountCharged = amount,
                Vehicle_Category = vehicleType,
                Payment_Mode = paymentMode,
                IsPackageTransaction = false

            };


            await _unitOfWork.AccessFeeTransactionRepository.Create(_mapper.Map<AccessFeeTransactionDataModel>(transaction));
            await _unitOfWork.CommitAsync();
        }
        public async Task<FixedTarifModel> FindFixedTarifByVehicleTypeAsync(string vehicleTypeName)
        {
            var vehicleType = await _unitOfWork.VehicleTypeRepository.GetByName(vehicleTypeName)
                ?? throw new KeyNotFoundException($"No vehicle type found with name: {vehicleTypeName}");

            var tarifType = await _unitOfWork.TarifTypeRepository.GetFixedTarifType()
                ?? throw new KeyNotFoundException("Tarif type 'Fixed' not found.");

            var tarif = await _unitOfWork.TarifRepository.GetTarif(vehicleType.Id, tarifType.Id)
                ?? throw new KeyNotFoundException($"No active tariff found for vehicle type: {vehicleTypeName}");

            var fixedTarif = await _unitOfWork.FixedTarifRepository.GetByTarifId(tarif.Id)
                ?? throw new KeyNotFoundException("No fixed tariff configured for this vehicle type.");

            return _mapper.Map<FixedTarifModel>(fixedTarif);
        }

        public async Task<VehicleTarifModel> CalculateFixedTarifAsync(string vehicleTypeName)
        {
            var fixedTarif = await FindFixedTarifByVehicleTypeAsync(vehicleTypeName);

            return new VehicleTarifModel
            {
                VehicleType = vehicleTypeName,
                Amount = fixedTarif.Amount
            };
        }

        public async Task SaveFixedTarifTransactionAsync(string vehicleTypeName, string paymentMode)
        {
            var result = await CalculateFixedTarifAsync(vehicleTypeName);
            await SaveTransactionAsync(result.Amount, result.VehicleType, paymentMode);
        }

        //public async Task<VehicleTarifModel> CalculateHourlyTarifAsync(string vehicleTypeName, DateTime from, DateTime to)
        //{
        //    var vehicle = await _unitOfWork.VehicleTypeRepository.GetByName(vehicleTypeName)
        //        ?? throw new KeyNotFoundException($"Vehicle type {vehicleTypeName} not found");

        //    var tarifType = await _unitOfWork.TarifTypeRepository.GetHourlyTarifType()
        //        ?? throw new KeyNotFoundException("Hourly tarif type not found");

        //    var tarif = await _unitOfWork.TarifRepository.GetTarif(vehicle.Id, tarifType.Id)
        //        ?? throw new KeyNotFoundException("Hourly tarif not found");

        //    int totalHours = (int)Math.Ceiling((to - from).TotalHours);
        //    if (totalHours <= 0)
        //        throw new Exception("ToTime must be after FromTime");

        //    var amount = await _unitOfWork.TarifTypeRepository.GetAmountByTarif(tarif.Id, TarifTypeEnum.Hourly, totalHours);

        //    return new VehicleTarifModel
        //    {
        //        VehicleType = vehicleTypeName,
        //        Amount = amount,
        //        totalhours = totalHours,
        //        TotalDuration = $"{(int)(to - from).TotalHours}:{(to - from).Minutes:D2}"
        //    };
        //}

        //public async Task SaveHourlyTarifTransactionAsync(string vehicleTypeName, DateTime from, DateTime to, string paymentMode)
        //{
        //    var result = await CalculateHourlyTarifAsync(vehicleTypeName, from, to);
        //    await SaveTransactionAsync(result.Amount, result.VehicleType, paymentMode);
        //}
        public async Task<VehicleTarifModel> CalculateHourlyTarifAsync(string vehicleTypeName, DateTime from, DateTime to, Guid? vehicleId = null)
        {
            // Check if vehicle has active package
            if (vehicleId.HasValue)
            {
                var hasActivePackage = await HasActivePackageAsync(vehicleId.Value);
                if (hasActivePackage)
                {
                   
                    var activePackage = await GetActivePackageForVehicleAsync(vehicleId.Value);
                    
                    if (_unitOfWork.Transaction == null)
                        _unitOfWork.BeginTransaction();

                    var transaction = new AccessFeeTransactionModel
                    {

                        AmountCharged = 0,
                        Vehicle_Category = vehicleTypeName,
                        Payment_Mode = "Package",
                        Vehicle_Id = (Guid)vehicleId,
                        IsPackageTransaction = true,
                        PackageId = activePackage?.PackageId
                };

                    await _unitOfWork.AccessFeeTransactionRepository.Create(_mapper.Map<AccessFeeTransactionDataModel>(transaction));
                    await _unitOfWork.CommitAsync();
                    return new VehicleTarifModel
                    {
                        VehicleType = vehicleTypeName,
                        Amount = 0,
                        IsPackageTransaction = true,
                        Message = "Free passage - Active package"
                    };
                }
            }
            var vehicle = await _unitOfWork.VehicleTypeRepository.GetByName(vehicleTypeName)
                ?? throw new KeyNotFoundException($"Vehicle type '{vehicleTypeName}' not found.");

            var tarifType = await _unitOfWork.TarifTypeRepository.GetHourlyTarifType()
                ?? throw new KeyNotFoundException("Tarif type 'Hourly' not found.");

            var tarif = await _unitOfWork.TarifRepository.GetTarif(vehicle.Id, tarifType.Id)
                ?? throw new KeyNotFoundException($"No hourly tarif found for vehicle type '{vehicleTypeName}'.");

            int totalHours = (int)Math.Ceiling((to - from).TotalHours);
            if (totalHours <= 0)
                throw new ArgumentException("End time must be greater than start time.");

            var amount = await _unitOfWork.TarifTypeRepository
                .GetAmountByTarif(tarif.Id, TarifTypeEnum.Hourly, totalHours);

            var duration = to - from;

            return new VehicleTarifModel
            {
                VehicleType = vehicleTypeName,
                Amount = amount,
                totalhours = totalHours,
                TotalDuration = $"{(int)duration.TotalHours}:{duration.Minutes:D2}"
            };
        }

       

        public async Task<TransactionResult> SaveHourlyTarifTransactionAsync(
            decimal amount,
            string vehicleCategory,
            string paymentMode,
            Guid? vehicleId = null)
        {
            var result = new TransactionResult();

            try
            {
                // Handle different payment modes
                switch (paymentMode.ToLower())
                {
                    case "card":
                        result = await ProcessCardPaymentTransaction(amount, vehicleCategory, vehicleId);
                        break;
                    case "cash":
                        result = await ProcessCashPaymentTransaction(amount, vehicleCategory, vehicleId);
                        break;
                    
                    default:
                        result.Success = false;
                        result.Message = "Invalid payment mode";
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Transaction failed: {ex.Message}";
                return result;
            }
        }



        private async Task<TransactionResult> ProcessCardPaymentTransaction(
            decimal amount,
            string vehicleCategory,
            Guid? vehicleId)
        {
            var result = new TransactionResult();

            if (!vehicleId.HasValue)
            {
                result.Success = false;
                result.Message = "Vehicle ID is required for card payments";
                return result;
            }

            // Check if vehicle account exists, create if not
            if (!await _unitOfWork.VehicleAccountsRepository.AccountExists(vehicleId.Value))
            {
                result.Success = false;
                result.Message = "Vehicle account does not exist. Please create an account first.";
                return result;
            }

            // Process card payment through vehicle account service
            var paymentResult = await _vehicleAccountService.ProcessCardPayment(
                vehicleId.Value,
                amount);

            if (!paymentResult.Success)
            {
                result.Success = false;
                result.Message = paymentResult.Message;
                result.RemainingBalance = paymentResult.RemainingBalance;
                return result;
            }

            // Save transaction record
            await SaveTransactionAsync(amount, vehicleCategory, "Card", vehicleId);

            result.Success = true;
            result.Message = $"Card payment successful. Amount deducted: {amount:C}";
            result.AmountCharged = paymentResult.AmountDeducted;
            result.RemainingBalance = paymentResult.RemainingBalance;

            return result;
        }

        private async Task<TransactionResult> ProcessCashPaymentTransaction(
            decimal amount,
            string vehicleCategory,
            Guid? vehicleId)
        {
            var result = new TransactionResult();

            // For cash payments, no account deduction needed
            await SaveTransactionAsync(amount, vehicleCategory, "Cash", vehicleId);

            result.Success = true;
            result.Message = "Cash payment processed successfully";
            result.AmountCharged = amount;
            result.RemainingBalance = vehicleId.HasValue ?
            await _unitOfWork.VehicleAccountsRepository.GetBalanceByVehicleId(vehicleId.Value) : 0;

            return result;
        }
        private async Task SaveTransactionAsync(
            decimal amount,
            string vehicleCategory,
            string paymentMode,
            Guid? vehicleId = null)
        {
            if (_unitOfWork.Transaction == null)
                _unitOfWork.BeginTransaction();

            try
            {
                var transaction = new AccessFeeTransactionModel
                {
                    AmountCharged = amount,
                    Vehicle_Category = vehicleCategory,
                    Payment_Mode = paymentMode,
                   
                    IsPackageTransaction = paymentMode.ToLower() == "package",
                    
                };

                await _unitOfWork.AccessFeeTransactionRepository.Create(
                    _mapper.Map<AccessFeeTransactionDataModel>(transaction));

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }














        public async Task<DistanceCalculation> CalculateDistanceTarifAsync(string vehicleTypeName, string enterBoothName, string exitBoothName, Guid? vehicleId = null)
        {
            if (vehicleId.HasValue)
            {
                var hasActivePackage = await HasActivePackageAsync(vehicleId.Value);
                if (hasActivePackage)
                {

                    var activePackage = await GetActivePackageForVehicleAsync(vehicleId.Value);

                    if (_unitOfWork.Transaction == null)
                        _unitOfWork.BeginTransaction();

                    var transaction = new AccessFeeTransactionModel
                    {

                        AmountCharged = 0,
                        Vehicle_Category = vehicleTypeName,
                        Payment_Mode = "Package",
                        Vehicle_Id = (Guid)vehicleId,
                        IsPackageTransaction = true,
                        PackageId = activePackage?.PackageId
                    };

                    await _unitOfWork.AccessFeeTransactionRepository.Create(_mapper.Map<AccessFeeTransactionDataModel>(transaction));
                    await _unitOfWork.CommitAsync();
                  
                    return new DistanceCalculation
                    {
                        VehicleType = vehicleTypeName,
                        Amount = 0,
                        Message = "Free passage - Active package"
                    };
                }
            }
            var vehicleType = await _unitOfWork.VehicleTypeRepository.GetByName(vehicleTypeName)
                ?? throw new KeyNotFoundException($"No vehicle type found with name: {vehicleTypeName}");

            var tarifType = await _unitOfWork.TarifTypeRepository.GetFixedTarifType()
                ?? throw new KeyNotFoundException("Tarif type 'Fixed' not found.");

            var tarif = await _unitOfWork.TarifRepository.GetTarif(vehicleType.Id, tarifType.Id)
                ?? throw new KeyNotFoundException("Tarif not found.");

            var (enterBoothId, exitBoothId) = await _unitOfWork.TollBoothRepository.GetBoothIds(enterBoothName, exitBoothName);
            if (enterBoothId == null || exitBoothId == null)
                throw new KeyNotFoundException("Invalid booth name(s).");

            var segments = await _unitOfWork.DistanceTarifRepository.GetSegmentsForTariffAsync(tarif.Id);
            if (segments == null || !segments.Any())
                throw new InvalidOperationException("No distance tariff segments found.");

            var visited = new HashSet<Guid>();
            var currentBoothId = enterBoothId.Value;
            var targetBoothId = exitBoothId.Value;
            decimal totalAmount = 0;
            var routeDetails = new List<RouteSegmentModel>();

            while (currentBoothId != targetBoothId)
            {
                visited.Add(currentBoothId);
                var nextSegment = segments.FirstOrDefault(s =>
                    (s.EntryBoothId == currentBoothId && !visited.Contains(s.ExitBoothId)) ||
                    (s.ExitBoothId == currentBoothId && !visited.Contains(s.EntryBoothId)));

                if (nextSegment == null)
                    throw new InvalidOperationException("Unable to find complete route.");

                var nextBoothId = (nextSegment.EntryBoothId == currentBoothId)
                    ? nextSegment.ExitBoothId
                    : nextSegment.EntryBoothId;

                var fromBoothName = await _unitOfWork.TollBoothRepository.GetBoothNameById(currentBoothId);
                var toBoothName = await _unitOfWork.TollBoothRepository.GetBoothNameById(nextBoothId);

                var segmentAmount = nextSegment.Distance * nextSegment.RatePerKm;
                totalAmount += segmentAmount;

                routeDetails.Add(new RouteSegmentModel
                {
                    Entery = fromBoothName,
                    Exit = toBoothName,
                    Distance = nextSegment.Distance,
                    RatePerKm = nextSegment.RatePerKm,
                    Amount = segmentAmount
                });

                currentBoothId = nextBoothId;
            }

            return new DistanceCalculation
            {
                VehicleType = vehicleTypeName,
                Amount = totalAmount,
                Route = routeDetails
            };
        }

        public async Task SaveDistanceTarifTransactionAsync(string vehicleTypeName, string enterBoothName, string exitBoothName, string paymentMode)
        {
            var result = await CalculateDistanceTarifAsync(vehicleTypeName, enterBoothName, exitBoothName);
            await SaveTransactionAsync(result.Amount, vehicleTypeName, paymentMode);
        }

        public async Task<VehicleTarifModel> CalculateTimeBasedTarifAsync(DateTime startHour, DateTime endHour, string vehicleTypeName, Guid? vehicleId = null)
        {
            if (vehicleId.HasValue)
            {
                var hasActivePackage = await HasActivePackageAsync(vehicleId.Value);
                if (hasActivePackage)
                {

                    var activePackage = await GetActivePackageForVehicleAsync(vehicleId.Value);

                    if (_unitOfWork.Transaction == null)
                        _unitOfWork.BeginTransaction();

                    var transaction = new AccessFeeTransactionModel
                    {

                        AmountCharged = 0,
                        Vehicle_Category = vehicleTypeName,
                        Payment_Mode = "Package",
                        Vehicle_Id = (Guid)vehicleId,
                        IsPackageTransaction = true,
                        PackageId = activePackage?.PackageId
                    };

                    await _unitOfWork.AccessFeeTransactionRepository.Create(_mapper.Map<AccessFeeTransactionDataModel>(transaction));
                    await _unitOfWork.CommitAsync();
                    return new VehicleTarifModel
                    {
                        VehicleType = vehicleTypeName,
                        Amount = 0,
                        IsPackageTransaction = true,
                        Message = "Free passage - Active package"
                    };
                }
            }
            var vehicleType = await _unitOfWork.VehicleTypeRepository.GetByName(vehicleTypeName)
                ?? throw new KeyNotFoundException($"No vehicle type found with name: {vehicleTypeName}");

            var tarifType = await _unitOfWork.TarifTypeRepository.GetFixedTarifType()
                ?? throw new KeyNotFoundException("Tarif type 'TimeBased' not found.");

            var tarif = await _unitOfWork.TarifRepository.GetTarif(vehicleType.Id, tarifType.Id)
                ?? throw new KeyNotFoundException("No active tariff found.");

            var timeTarif = await _unitOfWork.TimeBasedRepository.GetTimeBasedTarif(startHour, endHour)
                ?? throw new KeyNotFoundException("No matching time-based tariff found.");

            return new VehicleTarifModel
            {
                VehicleType = vehicleTypeName,
                Amount = timeTarif.Amount
            };
        }

        public async Task SaveTimeBasedTarifTransactionAsync(DateTime startHour, DateTime endHour, string vehicleTypeName, string paymentMode)
        {
            var result = await CalculateTimeBasedTarifAsync(startHour, endHour, vehicleTypeName);
            await SaveTransactionAsync(result.Amount, vehicleTypeName, paymentMode);
        }
        public async Task<TransactionReportResponse?> GetTransactionReportAsync(TransactionReportRequest summary)
        {
            return await _unitOfWork.AccessFeeTransactionRepository.GetTransactionReport(summary);
        }
    }
}
