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
    public class TarifTypeController : ControllerBase
    {

            private readonly TarifTypeService _tarifTypeService;
            private readonly IMapper _mapper;

            public TarifTypeController(TarifTypeService tarifTypeService, IMapper mapper)
            {
                _tarifTypeService = tarifTypeService;
                _mapper = mapper;
            }

            [HttpGet]
            public IActionResult GetAll()
            {
                var tarifType = _tarifTypeService.GetAll();
                var response = _mapper.Map<List<TarifTypeResponseModel>>(tarifType);
                return Ok(response);
            }

            [HttpGet("{id}")]
            public IActionResult GetById(Guid id)
            {
                try
                {
                    var tarifType = _tarifTypeService.GetById(id);
                    var response = _mapper.Map<TarifTypeResponseModel>(tarifType);
                    return Ok(response);
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }

            [HttpPost]
            public IActionResult Create(TarifTypeCreateRequestModel request)
            {
                var model = _mapper.Map<TarifTypeModel>(request);
            _tarifTypeService.Add(model);
                return Ok("TarifType created successfully.");
            }

            [HttpPut("{id}")]
            public IActionResult Update(Guid id, TarifTypeUpdateModel request)
            {
                try
                {
                    var model = _mapper.Map<TarifTypeModel>(request);
                    model.Id = id;
                _tarifTypeService.Update(model);
                    return Ok("TarifType updated successfully.");
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
                _tarifTypeService.Delete(id);
                    return Ok("TarifType deleted successfully.");
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }
        }
    
}
