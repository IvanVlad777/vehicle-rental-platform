using Microsoft.AspNetCore.Mvc;
using VehicleRentalPlatform.Application.Dtos.Auth;
using VehicleRentalPlatform.Infrastructure.Interfaces;

namespace VehicleRentalPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration config, IAuthService authService, ILogger<AuthController> logger )
        {
            _config = config;
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            try
            {
                var token = await _authService.LoginAsync(dto);
                _logger.LogInformation("Login successful for user {Email}", dto.Email);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Login failed for user {Email}: {Message}", dto.Email, ex.Message);
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}
