
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.API.Models.CreateRequest;
using VisualSoft.Surveillance.Payment.API.Models.Response;
using VisualSoft.Surveillance.Payment.API.Models.UpdateRequest;
using VisualSoft.Surveillance.Payment.Services.Implementations;

namespace VisualSoft.Surveillance.Payment.API.Controllers
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

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _fixedTarifService.GetAllAsync();
            return Ok(_mapper.Map<List<FixedTarifResponseModel>>(result));
        }

        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync(FixedTarifRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = _mapper.Map<FixedTarifModel>(request);
            var result = await _fixedTarifService.AddAsync(model);

            return Ok(_mapper.Map<FixedTarifResponseModel>(result));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _fixedTarifService.GetByIdAsync(id);


            return Ok(_mapper.Map<FixedTarifResponseModel>(result));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(Guid id, FixedTarifUpdateModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = _mapper.Map<FixedTarifModel>(request);
            model.Id = id;

            try
            {
                await _fixedTarifService.UpdateAsync(model);
                return Ok("FixedTarif updated successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await _fixedTarifService.DeleteAsync(id);
                return Ok("FixedTarif deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
