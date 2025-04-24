using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehicleRentalPlatform.Application.Dtos.Auth;
using VehicleRentalPlatform.Domain.Entities;
using VehicleRentalPlatform.Infrastructure.Data;
using VehicleRentalPlatform.Infrastructure.Interfaces;

namespace VehicleRentalPlatform.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDatabaseContext _context;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _hasher;

        public AuthService(ApplicationDatabaseContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _hasher = new PasswordHasher<User>();
        }

        public async Task<string> LoginAsync(LoginRequestDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null)
                throw new Exception("Invalid credentials");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid credentials");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpireMinutes"]!)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
