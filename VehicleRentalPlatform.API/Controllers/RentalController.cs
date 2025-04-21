using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VehicleRentalPlatform.Application.Dtos.Rental;
using VehicleRentalPlatform.Application.Interfaces;
using VehicleRentalPlatform.Domain.Entities;
namespace VehicleRentalPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController: ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly IMapper _mapper;

        public RentalController(IRentalService rentalService, IMapper mapper)
        {
            _rentalService = rentalService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalResponseDto>>> GetAll(int page = 1, int pageSize = 1000)
        {
            var rentals = await _rentalService.GetAllAsync();
            var result = _mapper.Map<IEnumerable<RentalResponseDto>>(rentals);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentalResponseDto>> GetById(Guid id)
        {
            var rental = await _rentalService.GetByIdAsync(id);
            if (rental == null) { return NotFound(); }
            var result = _mapper.Map<RentalResponseDto>(rental);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<RentalResponseDto>> CreateRental([FromBody] RentalCreateDto dto)
        {
            try
            {
                var rental = await _rentalService.CreateRentalAsync(dto);
                var result = _mapper.Map<RentalResponseDto>(rental);
                return CreatedAtAction(nameof(GetById), new { id = rental.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                await _rentalService.CancelAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RentalUpdateDto dto)
        {
            try
            {
                await _rentalService.UpdateAsync(id, dto.StartTime, dto.EndTime);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
} 
