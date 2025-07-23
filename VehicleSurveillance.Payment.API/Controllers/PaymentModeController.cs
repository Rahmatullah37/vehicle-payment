using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehicleSurveillance.Domain.Models;
using VehicleSurveillance.Payment.API.Models.CreateRequest;
using VehicleSurveillance.Payment.API.Models.Response;
using VehicleSurveillance.Payment.API.Models.UpdateRequest;
using VehicleSurveillance.Services.Implementations;

namespace VehicleSurveillance.Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentModeController : ControllerBase
    {
        private readonly PaymentModeService _paymentModeService;
        private readonly IMapper _mapper;

        public PaymentModeController(PaymentModeService paymentModeService, IMapper mapper)
        {
            _paymentModeService = paymentModeService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _paymentModeService.GetAll();
            return Ok(_mapper.Map<List<PaymentModeResponseModel>>(result));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var data = _paymentModeService.GetById(id);
                return Ok(_mapper.Map<PaymentModeResponseModel>(data));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("Create")]
        public IActionResult Create(PaymentModeCreateRequestModel request)
        {
            var model = _mapper.Map<PaymentModeModel>(request);
            _paymentModeService.Add(model);
            return Ok("Payment Mode added successfully");
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, PaymentModeUpdateModel request)
        {
            try
            {
                var model = _mapper.Map<PaymentModeModel>(request);
                model.Id = id;
                _paymentModeService.Update(model);
                return Ok("Payment Mode updated successfully");
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
                _paymentModeService.Delete(id);
                return Ok("Payment Mode deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
