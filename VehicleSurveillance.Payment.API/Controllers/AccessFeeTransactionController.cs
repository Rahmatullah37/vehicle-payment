using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisualSoft.Surveillance.Payment.Application.Services;
using VisualSoft.Surveillance.Payment.Data.Infrastructure;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.API.Models.CreateRequest;
using VisualSoft.Surveillance.Payment.API.Models.Response;
using VisualSoft.Surveillance.Payment.API.Models.UpdateRequest;
using VisualSoft.Surveillance.Payment.API.Models.VehicleAccount.Response;
using VisualSoft.Surveillance.Payment.Services.Implementations;
using VisualSoft.Surveillance.Payment.Services.Interfaces;

namespace VisualSoft.Surveillance.Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessFeeTransactionController : ControllerBase
    {
        private readonly AccessFeeTransactionService _accessFeeService;
        private readonly IVehicleAccountService _vehicleAccountService;

        private readonly IMapper _mapper;

        public AccessFeeTransactionController(AccessFeeTransactionService accessFeeService, IMapper mapper, IVehicleAccountService vehicleAccountService)
        {
            _accessFeeService = accessFeeService;
            _mapper = mapper;
            _vehicleAccountService = vehicleAccountService;
        }

        [HttpGet("All")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult GetAll()
        //{
        //    var result = _accessFeeService.GetAllAsync();
        //    return Ok(_mapper.Map<List<AccessFeeTransactionResponseModel>>(result));
        //}
        public async Task<IActionResult> GetAll()
        {
            var result = await _accessFeeService.GetAllAsync();
            var mappedResult = _mapper.Map<List<AccessFeeTransactionResponseModel>>(result);
            return Ok(mappedResult);
        }

        [HttpPost("Create")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task< IActionResult> CreateAsync(AccessFeeTransactionCreateRequestModel request)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //var model = _mapper.Map<AccessFeeTransactionModel>(request);
            //var result = await _accessFeeService.AddAsync(model);

            //return Ok((_mapper.Map<AccessFeeTransactionResponseModel>(result)));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = _mapper.Map<AccessFeeTransactionModel>(request);
            var result = await _accessFeeService.AddAsync(model);

            
            return result.Match<IActionResult>(
                success =>
                {
                    var response = _mapper.Map<AccessFeeTransactionResponseModel>(success);
                    return Ok(response);
                },
                error =>
                {
                    return BadRequest(error.Errors); // or error.Message depending on your structure
                });

        }

   

        [HttpGet("{id}")]
       
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccessFeeTransactionModel>> GetByIdAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var model = await _accessFeeService.GetByIdAsync(id);
            var response = _mapper.Map<AccessFeeTransactionResponseModel>(model);
            return Ok(response);
        }

        [HttpPut("{id}")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdateAsync(Guid id, AccessFeeTransactionUpdateModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = _mapper.Map<AccessFeeTransactionModel>(request);
            model.Id = id;

            await _accessFeeService.UpdateAsync(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteAsync(Guid id)
        {
            try
            {
                _accessFeeService.DeleteAsync(id);
                return Ok("Transaction deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        //[HttpPost("calculate-and-save")]
        ////[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> CalculateAndSave(VehicleTarifRequest request)

        //{
        //    try
        //    {
        //        var result =await _accessFeeService.CalculateAndSaveTarifAsync(request.VehicleTypeName, request.PaymentMode);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost("transaction-summary")]
        ////[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task< IActionResult> GetTransactionSummary(TransactionReportRequest summary)
        //        {
        //            if (summary.FromDate > summary.ToDate)
        //                return BadRequest("FromDate cannot be after ToDate.");

        //                 var result =await _accessFeeService.GetTransactionReportAsync(summary);
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    return Ok(result);
          
        //         }

        
        //[HttpPost("hourly-by-name")]
        ////[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetHourlyTarifAmountByName(HourlyTarifRequestModel request)
        //{
        //    var result =await _accessFeeService.CalculateHourlyTarifByNameAsync(
        //        request.VehicleTypeName,
        //        request.FromTime,
        //        request.ToTime,
        //        request.PaymentMode
        //    );

        //    return Ok(result); 
        //}
        //[HttpPost("DistanceTarif")]
        ////[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]

        //public async Task<IActionResult> DistanceTraif(DistanceTarifRequest request)
        //{
        //    try
        //    {
        //        var result = await _accessFeeService.DistanceTarif(request.VehicleTypeName, request.EnterBooth, request.ExitBooth);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[HttpPost("TimeBased")]
        ////[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]

        //public async Task<IActionResult> TimeBased(TimeBasedRequest request)
        //{
        //    try
        //    {
        //        var result = await _accessFeeService.TimeBasedTarif(request.StartHour, request.EndHour,request.vehicleTypeName);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpPost("calculate-fixed")]
        public async Task<IActionResult> CalculateFixedTarif([FromBody] VehicleTarifRequest request)
        {
            try
            {
                var result = await _accessFeeService.CalculateFixedTarifAsync(request.VehicleTypeName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("save-fixed")]
        public async Task<IActionResult> SaveFixedTarif([FromBody] VehicleTarifRequest request)
        {
            try
            {
                await _accessFeeService.SaveFixedTarifTransactionAsync(request.VehicleTypeName, request.PaymentMode);
                return Ok("Transaction saved.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpPost("calculate-hourly")]
        //public async Task<IActionResult> CalculateHourly([FromBody] HourlyTarifRequestModel request)
        //        {
        //            try
        //            {
        //                var result = await _accessFeeService.CalculateHourlyTarifAsync(
        //                    request.VehicleTypeName,
        //                    request.FromTime,
        //                    request.ToTime);
        //                return Ok(result);
        //            }
        //            catch (Exception ex)
        //            {
        //                return BadRequest(ex.Message);
        //            }
        //        }

        //[HttpPost("save-hourly")]
        //public async Task<IActionResult> SaveHourly([FromBody] HourlyTarifRequestModel request)
        //{
        //    try
        //    {
        //        await _accessFeeService.SaveHourlyTarifTransactionAsync(
        //            request.VehicleTypeName,
        //            request.FromTime,
        //            request.ToTime,
        //            request.PaymentMode);
        //        return Ok("Transaction saved.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpPost("calculate-hourly")]
        public async Task<IActionResult> CalculateHourly([FromBody] HourlyTarifRequestModel request)
        {
            try
            {
                var result = await _accessFeeService.CalculateHourlyTarifAsync(
                    request.VehicleTypeName,
                    request.FromTime,
                    request.ToTime, 
                    request.VehicleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("save-hourly")]
        public async Task<IActionResult> SaveHourly([FromBody] TaransectionRequest request)
        {
           
                try
                {
                    var response = new TransactionResponse();

                    switch (request.paymentMode.ToLower())
                    {
                        case "card":
                            response = await ProcessCardPayment(request);
                            break;
                        case "cash":
                            response = await ProcessCashPayment(request);
                            break;
                      
                        default:
                            return BadRequest(new { success = false, message = "Invalid payment mode" });
                    }

                    if (response.Success)
                        return Ok(response);
                    else
                        return BadRequest(response);
                }
                catch (Exception ex)
                {
                    return BadRequest(new TransactionResponse
                    {
                        Success = false,
                        Message = ex.Message
                    });
                }
            
            
                    }

        private async Task<TransactionResponse> ProcessCardPayment(TaransectionRequest request)
        {
            if (!request.VehicleId.HasValue)
            {
                return new TransactionResponse
                {
                    Success = false,
                    Message = "Vehicle ID is required for card payments"
                };
            }

            // Validate account
            var validation = await _vehicleAccountService.ValidateAccountForTransaction(
                request.VehicleId.Value, request.amount);

            if (!validation.IsValid)
            {
                return new TransactionResponse
                {
                    Success = false,
                    Message = validation.Message,
                    CurrentBalance = validation.CurrentBalance,
                    RequiredAmount = request.amount
                };
            }

            // Process payment
            var paymentResult = await _vehicleAccountService.ProcessCardPayment(
                request.VehicleId.Value,
                request.amount
               );

            // Save transaction
            await _accessFeeService.SaveHourlyTarifTransactionAsync(
                request.amount, request.vehicalCategory, request.paymentMode);

            return new TransactionResponse
            {
                Success = paymentResult.Success,
                Message = paymentResult.Success ?
                "Card payment processed successfully" : paymentResult.Message,
                AmountCharged = paymentResult.AmountDeducted,
                RemainingBalance = paymentResult.RemainingBalance,
                PaymentMode = "Card",
                TransactionDate = DateTime.UtcNow
            };
        }

        private async Task<TransactionResponse> ProcessCashPayment(TaransectionRequest request)
        {
            await _accessFeeService.SaveHourlyTarifTransactionAsync(
               request.amount, request.vehicalCategory, request.paymentMode);
            return new TransactionResponse
            {
                Success = true,
                Message = "Cash payment processed successfully",
                AmountCharged = request.amount,
                PaymentMode = "Cash",
                TransactionDate = DateTime.UtcNow
            };
        }

        


    [HttpPost("calculate-distance")]
        public async Task<IActionResult> CalculateDistance([FromBody] DistanceTarifRequest request)
        {
            try
            {
                var result = await _accessFeeService.CalculateDistanceTarifAsync(
                    request.VehicleTypeName,
                    request.EnterBooth,
                    request.ExitBooth,
                   request.VehicleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("save-distance")]
        public async Task<IActionResult> SaveDistance([FromBody] DistanceTarifTransactionRequest request)
        {
            try
            {
                await _accessFeeService.SaveDistanceTarifTransactionAsync(
                    request.VehicleTypeName,
                    request.EnterBooth,
                    request.ExitBooth,
                    request.PaymentMode);
                return Ok("Transaction saved.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("calculate-timebased")]
        public async Task<IActionResult> CalculateTimeBased([FromBody] TimeBasedRequest request)
        {
            try
            {
                var result = await _accessFeeService.CalculateTimeBasedTarifAsync(
                    request.StartHour,
                    request.EndHour,
                    request.vehicleTypeName,
                    request.VehicleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("save-timebased")]
        public async Task<IActionResult> SaveTimeBased([FromBody] TimeBasedTransactionRequest request)
        {
            try
            {
                await _accessFeeService.SaveTimeBasedTarifTransactionAsync(
                    request.StartHour,
                    request.EndHour,
                    request.vehicleTypeName,
                    request.PaymentMode);
                return Ok("Transaction saved.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("transaction-summary")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionSummary(TransactionReportRequest summary)
        {
            if (summary.FromDate > summary.ToDate)
                return BadRequest("FromDate cannot be after ToDate.");

            var result = await _accessFeeService.GetTransactionReportAsync(summary);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(result);

        }
    }
}
