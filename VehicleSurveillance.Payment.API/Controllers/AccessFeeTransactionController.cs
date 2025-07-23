using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehicleSurveillance.Data.Infrastructure;
using VehicleSurveillance.Domain.Models;
using VehicleSurveillance.Payment.API.Models.CreateRequest;
using VehicleSurveillance.Payment.API.Models.Response;
using VehicleSurveillance.Payment.API.Models.UpdateRequest;
using VehicleSurveillance.Services.Implementations;
using VehicleSurveillance.Services.Interfaces;

namespace VehicleSurveillance.Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessFeeTransactionController : ControllerBase
    {
        private readonly AccessFeeTransactionService _accessFeeService;
        private readonly IMapper _mapper;

        public AccessFeeTransactionController(AccessFeeTransactionService accessFeeService, IMapper mapper)
        {
            _accessFeeService = accessFeeService;
            _mapper = mapper;
        }

        [HttpGet("GetTransactionList")]
        public IActionResult GetAll()
        {
            var result = _accessFeeService.GetAll();
            return Ok(_mapper.Map<List<AccessFeeTransactionResponseModel>>(result));
        }

        [HttpPost("CreateTransaction")]
        public IActionResult Create(AccessFeeTransactionCreateRequestModel request)
        {
            var model = _mapper.Map<AccessFeeTransactionModel>(request);
            _accessFeeService.Add(model);
            return Ok("Transaction created successfully");
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var data = _accessFeeService.GetById(id);
                var response = _mapper.Map<AccessFeeTransactionResponseModel>(data);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, AccessFeeTransactionUpdateModel request)
        {
            try
            {
                var model = _mapper.Map<AccessFeeTransactionModel>(request);
                model.Id = id;
                _accessFeeService.Update(model);
                return Ok("Transaction updated successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _accessFeeService.Delete(id);
                return Ok("Transaction deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("calculate-and-save")]
        public IActionResult CalculateAndSave(VehicleTarifRequest request)
        {
            try
            {
                var result = _accessFeeService.CalculateAndSaveTarif(request.VehicleTypeName, request.PaymentMode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

            [HttpPost("transaction-summary")]
                public IActionResult GetTransactionSummary(TransactionReportRequest summary)
                {
                    if (summary.FromDate > summary.ToDate)
                        return BadRequest("FromDate cannot be after ToDate.");

                         var result = _accessFeeService.GetTransactionReport(summary);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(result);
          
                 }

        //[HttpPost("hourly-by-name")]
        //public IActionResult GetHourlyTarifAmountByName(HourlyTarifRequestModel request)
        //{
        //    var amount = _accessFeeService.CalculateHourlyTarifByName(
        //        request.VehicleTypeName,
        //        request.FromHour,
        //        request.ToHour,
        //        request.PaymentMode
        //    );

        //    return Ok(new { amount });
        //}
        [HttpPost("hourly-by-name")]
        public IActionResult GetHourlyTarifAmountByName(HourlyTarifRequestModel request)
        {
            var result = _accessFeeService.CalculateHourlyTarifByName(
                request.VehicleTypeName,
                request.FromTime,
                request.ToTime,
                request.PaymentMode
            );

            return Ok(result); // ← return full object now (VehicleTarifModel)
        }

    }






}
