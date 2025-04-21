using VehicleRentalPlatform.Application.Interfaces;
using VehicleRentalPlatform.Application.Services;
using VehicleRentalPlatform.Infrastructure.Repositories;

namespace VehicleRentalPlatform.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IVehicleService, VehicleService>();


            services.AddScoped<IVehicleRepository, EFVehicleRepository>();
            services.AddScoped<ICustomerRepository, EFCustomerRepository>();
            services.AddScoped<IRentalRepository, EFRentalRepository>();
            services.AddScoped<ITelemetryRepository, EFTelemetryRepository>();
            return services;
        }
    }
}
