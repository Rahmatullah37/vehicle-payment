
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Services.Implementations;
using VisualSoft.Surveillance.Payment.API.Models.CreateRequest;
using VisualSoft.Surveillance.Payment.API.Models.UpdateRequest;
using VisualSoft.Surveillance.Payment.API.Models.Response;

namespace VisualSoft.Surveillance.Payment.API.Controllers
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

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _paymentModeService.GetAllAsync();
            var response = _mapper.Map<List<PaymentModeResponseModel>>(result);
            return Ok(response);
        }

        [HttpPost("Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] PaymentModeCreateRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mode = _mapper.Map<PaymentModeModel>(request);
            await _paymentModeService.AddAsync(mode);
            return Ok(_mapper.Map<PaymentModeResponseModel>(mode));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaymentModeResponseModel>> GetByIdAsync(Guid id)
        {
            var result = await _paymentModeService.GetByIdAsync(id);
            var response = _mapper.Map<PaymentModeResponseModel>(result);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] PaymentModeUpdateModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mode = _mapper.Map<PaymentModeModel>(request);
            mode.Id = id;

            await _paymentModeService.UpdateAsync(mode);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _paymentModeService.DeleteAsync(id);
            return Ok();
        }
    }
}
