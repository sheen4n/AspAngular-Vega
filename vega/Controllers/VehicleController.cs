using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;
using vega.Core.Repositories;

namespace vega.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        // Create a field to store the mapper object
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public VehiclesController(IMapper mapper, IVehicleRepository repository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = _mapper.Map<Vehicle>(vehicleResource);
            vehicle.LastUpdate = System.DateTime.Now;
            _unitOfWork.Vehicles.Add(vehicle);
            await _unitOfWork.CompleteAsync();

            vehicle = await _unitOfWork.Vehicles.GetVehicle(vehicle.Id);

            var result = _mapper.Map<VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpPut("{id}")] //api/vehicles/{id}
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = await _unitOfWork.Vehicles.GetVehicle(id);

            if (vehicle == null)
                return NotFound();

            _mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = System.DateTime.Now;

            await _unitOfWork.CompleteAsync();

            vehicle = await _unitOfWork.Vehicles.GetVehicle(vehicle.Id);
            var result = _mapper.Map<VehicleResource>(vehicle);
            return Ok(result);
        }

        [HttpDelete("{id}")] //api/vehicles/{id}
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _unitOfWork.Vehicles.GetVehicle(id, includeRelated: false);

            if (vehicle == null)
                return NotFound();

            _unitOfWork.Vehicles.Remove(vehicle);
            await _unitOfWork.CompleteAsync();

            return Ok(id);
        }

        [HttpGet("{id}")] //api/vehicles/{id}
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await _unitOfWork.Vehicles.GetVehicle(id);

            if (vehicle == null)
                return NotFound();

            var vehicleResource = _mapper.Map<VehicleResource>(vehicle);
            return Ok(vehicleResource);
        }


        [HttpGet] //api/vehicles/
        public async Task<QueryResultResource<VehicleResource>> GetVehicles(VehicleQueryResource vehicleQueryResource)
        {
            var vehicleQuery = _mapper.Map<VehicleQuery>(vehicleQueryResource);
            var queryResult = await _unitOfWork.Vehicles.GetVehicles(vehicleQuery);

            return _mapper.Map<QueryResultResource<VehicleResource>>(queryResult);
        }
    }
}