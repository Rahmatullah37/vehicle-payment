using VisualSoft.Surveillance.Payment.API.Models.CreateRequest;
using VisualSoft.Surveillance.Payment.API.Models.Response;
using VisualSoft.Surveillance.Payment.API.Models.UpdateRequest;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace VisualSoft.Surveillance.Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypeController : ControllerBase
    {
        private readonly VehicleTypeService _vehicleTypeService;
        private readonly IMapper _mapper;

        public VehicleTypeController(VehicleTypeService vehicleTypeService, IMapper mapper)
        {
            _vehicleTypeService = vehicleTypeService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _vehicleTypeService.GetAllAsync();
            var response = _mapper.Map<List<TarifTypeResponseModel>>(result); // You can change to VehicleTypeResponseModel if available
            return Ok(response);
        }

        [HttpPost("Post")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] TarifTypeCreateRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicleType = _mapper.Map<VehicleTypeModel>(request);
            await _vehicleTypeService.AddAsync(vehicleType);
            var response = _mapper.Map<TarifTypeResponseModel>(vehicleType);
            return Ok(response);
        }

        [HttpGet("{id}")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TarifTypeResponseModel>> GetByIdAsync(Guid id)
        {
            var vehicleType = await _vehicleTypeService.GetByIdAsync(id);
            var response = _mapper.Map<TarifTypeResponseModel>(vehicleType);
            return Ok(response);
        }

        [HttpPut("{id}")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid id, VehicleTypeUpdateModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicleType = _mapper.Map<VehicleTypeModel>(request);
            vehicleType.Id = id;
            await _vehicleTypeService.UpdateAsync(vehicleType);
            return Ok();
        }

        [HttpDelete("{id}")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _vehicleTypeService.DeleteAsync(id);
            return Ok();
        }
    }
}
