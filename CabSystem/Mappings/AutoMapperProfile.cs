using AutoMapper;
using CabSystem.DTOs;
using CabSystem.Models;

namespace CabSystem.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // User <-> DTO mappings (if needed)
            // CreateMap<User, UserDto>().ReverseMap();

            // Ride <-> DTO mappings
            CreateMap<Ride, RideDTO>().ReverseMap();
            CreateMap<CreateRideDTO, Ride>().ReverseMap();
            CreateMap<Ride, CompleteRideResponseDTO>()
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => "Thank you for riding with us!"));
            CreateMap<Rating, RatingDTO>().ReverseMap();
            CreateMap<CreateRatingDTO, Rating>();
            CreateMap<Rating, RatingDTO>().ReverseMap();
            CreateMap<CreatePaymentDTO, Payment>();
            CreateMap<Payment, PaymentDTO>();
            CreateMap<Driver, DriverProfileDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone.ToString()));
            CreateMap<User, UserProfileDTO>().ReverseMap();
            CreateMap<Ride, UserRideDTO>()
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src =>src.Driver != null && src.Driver.User != null ?src.Driver.User.Name: "Not Assigned"));
            CreateMap<Rating, UserRatingDTO>();
        }
    }
}
