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
            CreateMap<Ride, RideDTO>()
            .ForMember(dest => dest.DriverNames, opt => opt.MapFrom(src => src.Driver != null ? src.Driver.User.Name : null))
            .ForMember(dest => dest.PickupLocation, opt => opt.MapFrom(src => src.PickupLocation))
            .ForMember(dest => dest.DropoffLocation, opt => opt.MapFrom(src => src.DropoffLocation))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Fare, opt => opt.MapFrom(src => src.Fare));
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
            CreateMap<Ride, RequestedRideDTO>()
                .ForMember(dest => dest.PassengerName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.PickupLocation, opt => opt.MapFrom(src => src.PickupLocation))
                .ForMember(dest => dest.DropoffLocation, opt => opt.MapFrom(src => src.DropoffLocation))
                .ForMember(dest => dest.Fare, opt => opt.MapFrom(src => src.Fare));

            //Payment contoller
            CreateMap<Payment, PaymentDTO>()
                .ForMember(dest => dest.PickupLocation, opt => opt.MapFrom(src => src.Ride.PickupLocation))
                .ForMember(dest => dest.DropoffLocation, opt => opt.MapFrom(src => src.Ride.DropoffLocation));

        }
    }
}
