using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VehicleSurveillance.Domain.Models;
using VehicleSurveillance.Payment.API.Models.CreateRequest;
using VehicleSurveillance.Payment.API.Models.Response;
using VehicleSurveillance.Payment.API.Models.UpdateRequest;
using VehicleSurveillance.Services.Implementations;

namespace VehicleSurveillance.Payment.API.Controllers
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

        [HttpGet]
        public IActionResult GetAll()
        {
            var tarifs = _tarifService.GetAll();
            var response = _mapper.Map<List<TarifResponseModel>>(tarifs);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var tarif = _tarifService.GetById(id);
                var response = _mapper.Map<TarifResponseModel>(tarif);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] TarifCreateRequestModel request)
        {
            var model = _mapper.Map<TarifModel>(request);
            _tarifService.Add(model);
            return Ok("Tarif created successfully.");
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, TarifUpdateModel request)
        {
            try
            {
                var model = _mapper.Map<TarifModel>(request);
                model.Id = id;
                _tarifService.Update(model);
                return Ok("Tarif updated successfully.");
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
                _tarifService.Delete(id);
                return Ok("Tarif deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }


}
