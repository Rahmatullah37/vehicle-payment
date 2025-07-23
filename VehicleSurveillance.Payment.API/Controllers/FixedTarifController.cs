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
    public class FixedTarifController : ControllerBase
    {
        private readonly FixedTarifService _fixedTarifService;
        private readonly IMapper _mapper;

        public FixedTarifController(FixedTarifService fixedTarifService, IMapper mapper)
        {
            _fixedTarifService = fixedTarifService;
            _mapper = mapper;
        }

        [HttpGet("GetFixedTarifList")]
        public IActionResult GetAll()
        {
            var result = _fixedTarifService.GetAll();
            return Ok(_mapper.Map<List<FixedTarifResponseModel>>(result));
        }

        [HttpPost("PostFixedTarif")]
        public IActionResult Create(FixedTarifRequestModel request)
        {
            var fixedTarif = _mapper.Map<FixedTarifModel>(request);
            _fixedTarifService.Add(fixedTarif);
            return Ok("FixedTarif Added");
        }

        [HttpGet("{id}")]
        public ActionResult<FixedTarifResponseModel> Get(Guid id)
        {
            try
            {
                var fixedTarif = _fixedTarifService.GetById(id);
                var response = _mapper.Map<FixedTarifResponseModel>(fixedTarif);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, FixedTarifUpdateModel request)
        {
            try
            {
                var fixedTarif = _mapper.Map<FixedTarifModel>(request);
                fixedTarif.Id = id;
                _fixedTarifService.Update(fixedTarif);
                return Ok("FixedTarif updated successfully");
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
                _fixedTarifService.Delete(id);
                return Ok("FixedTarif deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
          

    }
}
