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
    public class VehicleTypeController : ControllerBase
    {
       
            private readonly VehicleTypeService _vehicleTypeService;
            private readonly IMapper _mapper;

            public VehicleTypeController(VehicleTypeService VehicleTypeService, IMapper mapper)
            {
                _vehicleTypeService = VehicleTypeService;
                _mapper = mapper;
            }

            [HttpGet]
            public IActionResult GetAll()
            {
                var VehicleType = _vehicleTypeService.GetAll();
                var response = _mapper.Map<List<TarifTypeResponseModel>>(VehicleType);
                return Ok(response);
            }

            [HttpGet("{id}")]
            public IActionResult GetById(Guid id)
            {
                try
                {
                    var tarifType = _vehicleTypeService.GetById(id);
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
                var model = _mapper.Map<VehicleTypeModel>(request);
                _vehicleTypeService.Add(model);
                return Ok("VehicleType created successfully.");
            }

            [HttpPut("{id}")]
            public IActionResult Update(Guid id, VehicleTypeUpdateModel request)
            {
                try
                {
                    var model = _mapper.Map<VehicleTypeModel>(request);
                    model.Id = id;
                    _vehicleTypeService.Update(model);
                    return Ok("VehicleType updated successfully.");
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
                _vehicleTypeService.Delete(id);
                    return Ok("VehicleType deleted successfully.");
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }
        }
}
