
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
    public class TarifController : ControllerBase
    {
        private readonly TarifService _tarifService;
        private readonly IMapper _mapper;

        public TarifController(TarifService tarifService, IMapper mapper)
        {
            _tarifService = tarifService;
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
            var result = await _tarifService.GetAllAsync();
            var response = _mapper.Map<List<TarifResponseModel>>(result);
            return Ok(response);
        }

        [HttpPost("Post")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] TarifCreateRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tarif = _mapper.Map<TarifModel>(request);
            await _tarifService.AddAsync(tarif);
            var response = _mapper.Map<TarifResponseModel>(tarif);

            return Ok(response);
        }

        [HttpGet("{id}")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TarifResponseModel>> GetByIdAsync(Guid id)
        {
            var tarif = await _tarifService.GetByIdAsync(id);
            var response = _mapper.Map<TarifResponseModel>(tarif);
            return Ok(response);
        }

        [HttpPut("{id}")]
        //[Authorize(Policy = Constants.Permissions.VIEW_TRANSACTION)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid id, TarifUpdateModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tarif = _mapper.Map<TarifModel>(request);
            tarif.Id = id;

            await _tarifService.UpdateAsync(tarif);
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
            await _tarifService.DeleteAsync(id);
            return Ok();
        }
    }
}
