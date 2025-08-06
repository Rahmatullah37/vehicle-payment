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
    public class TarifTypeController : ControllerBase
    {
        private readonly TarifTypeService _tarifTypeService;
        private readonly IMapper _mapper;

        public TarifTypeController(TarifTypeService tarifTypeService, IMapper mapper)
        {
            _tarifTypeService = tarifTypeService;
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
            var result = await _tarifTypeService.GetAllAsync();
            var response = _mapper.Map<List<TarifTypeResponseModel>>(result);
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

            var tarifType = _mapper.Map<TarifTypeModel>(request);
            await _tarifTypeService.AddAsync(tarifType);
            var response = _mapper.Map<TarifTypeResponseModel>(tarifType);

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
            var tarifType = await _tarifTypeService.GetByIdAsync(id);
            var response = _mapper.Map<TarifTypeResponseModel>(tarifType);
            return Ok(response);
        }

        [HttpPut("{id}")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid id, TarifTypeUpdateModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tarifType = _mapper.Map<TarifTypeModel>(request);
            tarifType.Id = id;

            await _tarifTypeService.UpdateAsync(tarifType);
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
            await _tarifTypeService.DeleteAsync(id);
            return Ok();
        }
    }
}
