using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Application.Dtos.Auth;

namespace VehicleRentalPlatform.Infrastructure.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequestDto dto);
    }
}
