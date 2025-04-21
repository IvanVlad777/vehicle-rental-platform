using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VehicleRentalPlatform.Application.Dtos.Customer;
using VehicleRentalPlatform.Application.Interfaces;

namespace VehicleRentalPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAll()
        {
            var customers = await _service.GetAllAsync();
            var result = _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDto>> GetById(Guid id)
        {
            var customer = await _service.GetByIdAsync(id);
            if (customer == null) return NotFound();

            var result = _mapper.Map<CustomerResponseDto>(customer);
            result.TotalDistanceDriven = await _service.GetTotalDistanceAsync(id);
            result.TotalRentalPrice = await _service.GetTotalRentalPriceAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerResponseDto>> Create([FromBody] CustomerCreateDto dto)
        {
            var customer = await _service.CreateAsync(dto);
            var result = _mapper.Map<CustomerResponseDto>(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
