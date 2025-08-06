
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Services.Implementations;
using VisualSoft.Surveillance.Payment.API.Models.CreateRequest;
using VisualSoft.Surveillance.Payment.API.Models.UpdateRequest;
using VisualSoft.Surveillance.Payment.API.Models.Response;
using VisualSoft.Surveillance.Payment.Services.Interfaces;

namespace VisualSoft.Surveillance.Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly PackagesServices _packagesServices;
        private readonly IMapper _mapper;

        public PackageController(PackagesServices packagesServices, IMapper mapper)
        {
            _packagesServices = packagesServices;
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
            var result = await _packagesServices.GetAllAsync();
            var response = _mapper.Map<List<PackageResponseModel>>(result);
            return Ok(response);
        }

        [HttpPost("Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] PackageCreateRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var package = _mapper.Map<PackageModel>(request);
            await _packagesServices.AddAsync(package);
            return Ok(_mapper.Map<PackageResponseModel>(package));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PackageResponseModel>> GetByIdAsync(Guid id)
        {
            var result = await _packagesServices.GetByIdAsync(id);
            var response = _mapper.Map<PackageResponseModel>(result);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] PackageUpdateModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var package = _mapper.Map<PackageModel>(request);
            package.Id = id;

            await _packagesServices.UpdateAsync(package);
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
            await _packagesServices.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("activePackages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetActivePackagesAsync()
        {
            var result = await _packagesServices.GetActivePackagesAsync();
            if (!result.Any())
                return NoContent();

            var response = _mapper.Map<List<PackageResponseModel>>(result);
            return Ok(response);
        }
        [HttpGet("by-cost-range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPackagesByCostRangeAsync([FromQuery] decimal minCost, [FromQuery] decimal maxCost)
        {
            if (minCost < 0 || maxCost < 0 || minCost > maxCost)
                return BadRequest("Invalid cost range parameters");

            var result = await _packagesServices.GetPackagesByCostRangeAsync(minCost, maxCost);
            if (!result.Any())
                return NoContent();

            var response = _mapper.Map<List<PackageResponseModel>>(result);
            return Ok(response);
        }
        [HttpGet("cheapest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCheapestPackageAsync()
        {
            var result = await _packagesServices.GetCheapestPackageAsync();
            if (result == null)
                return NotFound("No active packages found");

            var response = _mapper.Map<PackageResponseModel>(result);
            return Ok(response);
        }
        [HttpGet("most-expensive")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMostExpensivePackageAsync()
        {
            var result = await _packagesServices.GetMostExpensivePackageAsync();
            if (result == null)
                return NotFound("No active packages found");

            var response = _mapper.Map<PackageResponseModel>(result);
            return Ok(response);
        }
        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPackageCountAsync()
        {
            var count = await _packagesServices.GetPackageCountAsync();
            return Ok(new { totalPackages = count });
        }
        [HttpPatch("activate/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActivatePackageAsync(Guid id)
        {
            var result = await _packagesServices.ActivatePackageAsync(id);

            return result.Match<IActionResult>(
                success => success ? Ok(new { message = "Package activated successfully" }) : BadRequest("Failed to activate package"),
                validationResult => NotFound(validationResult.Errors)
            );
        }
        [HttpPatch("deactivate/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeactivatePackageAsync(Guid id)
        {
            var result = await _packagesServices.DeactivatePackageAsync(id);

            return result.Match<IActionResult>(
                success => success ? Ok(new { message = "Package deactivated successfully" }) : BadRequest("Failed to deactivate package"),
                validationResult => NotFound(validationResult.Errors)
            );
        }
        [HttpPatch("update-cost/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePackageCostAsync(Guid id, [FromBody] UpdatePackageCostRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _packagesServices.UpdatePackageCostAsync(id, request.NewCost);

            return result.Match<IActionResult>(
                success => success ? Ok(new { message = "Package cost updated successfully", newCost = request.NewCost }) : BadRequest("Failed to update package cost"),
                validationResult => NotFound(validationResult.Errors)
            );
        }
        [HttpPatch("vehicle-package/extend/{vehiclePackageId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExtendVehiclePackageAsync(Guid vehiclePackageId, [FromBody] ExtendPackageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _packagesServices.ExtendVehiclePackageAsync(vehiclePackageId, request.NewExpireDate);

            return result.Match<IActionResult>(
                success => success ? Ok(new { message = "Package extended successfully", newExpireDate = request.NewExpireDate }) : BadRequest("Failed to extend package"),
                validationResult => BadRequest(validationResult.Errors)
            );
        }

        [HttpGet("expired")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExpiredPackagesAsync()
        {
            var result = await _packagesServices.GetExpiredPackagesAsync();
            if (!result.Any())
                return NoContent();

            var response = _mapper.Map<List<VehiclePackageResponseModel>>(result);
            return Ok(response);
        }
        
        [HttpGet("analytics/popularity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPackagePopularityStatsAsync()
        {
            var result = await _packagesServices.GetPackagePopularityStatsAsync();
            if (!result.Any())
                return NoContent();

            var response = _mapper.Map<List<PackagePopularityResponseModel>>(result);
            return Ok(response);
        }
       


    }
}
