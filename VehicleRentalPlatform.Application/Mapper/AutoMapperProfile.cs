using AutoMapper;
using VehicleRentalPlatform.Application.Dtos.Customer;
using VehicleRentalPlatform.Application.Dtos.Rental;
using VehicleRentalPlatform.Application.Dtos.Vehicle;
using VehicleRentalPlatform.Domain.Entities;

namespace VehicleRentalPlatform.Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<Rental, RentalResponseDto>();
            CreateMap<Rental, RentalResponseDto>()
                .ForMember(dest => dest.DistanceTraveled, opt => opt.MapFrom(src => (src.EndOdometerKm.HasValue && src.StartOdometerKm.HasValue) ? src.EndOdometerKm - src.StartOdometerKm : null))
                .ForMember(dest => dest.StartBatterySoc, opt => opt.MapFrom(src => src.StartBatterySoc))
                .ForMember(dest => dest.EndBatterySoc, opt => opt.MapFrom(src => src.EndBatterySoc));
            CreateMap<Customer, CustomerResponseDto>();
            CreateMap<Vehicle, VehicleResponseDto>()
                .ForMember(dest => dest.TotalDistanceDriven, opt => opt.Ignore())
                .ForMember(dest => dest.TotalRentalCount, opt => opt.Ignore())
                .ForMember(dest => dest.TotalRentalIncome, opt => opt.Ignore());
        }
    }
}
