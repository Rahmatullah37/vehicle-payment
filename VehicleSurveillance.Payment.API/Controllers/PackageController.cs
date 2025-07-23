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
    public class PackageController : ControllerBase
    {
       
            private readonly PackagesServices _packagesServices;
            private readonly IMapper _mapper;
            public PackageController(PackagesServices packagesServices, IMapper mapper)
            {
                     _packagesServices = packagesServices; ;
                     _mapper = mapper;
            }

            [HttpGet("GetPackaegesList")]
            public IActionResult GetAll()
            {
                var result = _packagesServices.GetAllPackages();
                return Ok(_mapper.Map<List<PackageResponseModel>>(result));
            }

            [HttpPost("PostPackaege")]
            public IActionResult Create(PackageCreateRequestModel request)
            {
                var package = _mapper.Map<PackageModel>(request);
            _packagesServices.AddPackage(package);
                return Ok("Package Added");
            }
            [HttpGet("{id}")]
            public ActionResult<PackageResponseModel> Get(Guid id)
            {
                try
                {
                    var package = _packagesServices.GetPackageById(id);
                    var response = _mapper.Map<PackageResponseModel>(package);
                    return Ok(response);
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }
            [HttpPut("{id}")]
            public IActionResult Update(Guid id, PackageUpdateModel request)
            {
                try
                {
                    var package = _mapper.Map<PackageModel>(request);
                    package.Id = id;
                _packagesServices.UpdatePackage(package);
                    return Ok("Package updated successfully");
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
                _packagesServices.DeletePackage(id);
                    return Ok("Package deleted successfully");
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }


            
        }
    
}
