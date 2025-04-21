using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VehicleRentalPlatform.Application.Dtos.Vehicle;
using VehicleRentalPlatform.Application.Interfaces;

namespace VehicleRentalPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleController(IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleResponseDto>>> GetAll()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            var result = _mapper.Map<IEnumerable<VehicleResponseDto>>(vehicles);
            return Ok(result);
        }

        [HttpGet("{vin}")]
        public async Task<ActionResult<VehicleResponseDto>> GetByVin(string vin)
        {
            var vehicle = await _vehicleService.GetByVinAsync(vin);
            if (vehicle == null) return NotFound();

            var result = _mapper.Map<VehicleResponseDto>(vehicle);
            result.TotalDistanceDriven = await _vehicleService.GetTotalDistanceAsync(vin);
            result.TotalRentalCount = await _vehicleService.GetRentalCountAsync(vin);
            result.TotalRentalIncome = await _vehicleService.GetTotalIncomeAsync(vin);

            return Ok(result);
        }
    }
}
