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

            CreateMap<User, UpdateCustomerProfileDTO>().ReverseMap();
            CreateMap<RegistrationDTO, User>()
             .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => Convert.ToInt64(src.Phone)))
             .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<User, UserProfileDTO>()
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.ToString()));

            CreateMap<RegistrationDTO, Driver>()
            .ForMember(dest => dest.LicenseNo, opt => opt.MapFrom(src => src.LicenseNumber))
            .ForMember(dest => dest.VehicleDetails, opt => opt.MapFrom(src => src.VehicleDetails))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); // We'll set this manually


            // Ride <-> DTO mappings
            CreateMap<Ride, RideDTO>()
            .ForMember(dest => dest.DriverNames, opt => opt.MapFrom(src => src.Driver != null && src.Driver.User != null ? src.Driver.User.Name : null))
            .ForMember(dest => dest.PickupLocation, opt => opt.MapFrom(src => src.PickupLocation))
            .ForMember(dest => dest.DropoffLocation, opt => opt.MapFrom(src => src.DropoffLocation))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Fare, opt => opt.MapFrom(src => src.Fare));
            CreateMap<CreateRideDTO, Ride>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Fare, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());
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
                .ForMember(dest => dest.DropoffLocation, opt => opt.MapFrom(src => src.Ride.DropoffLocation))
                .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.Ride.DriverId))
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Ride.Driver != null && src.Ride.Driver.User != null ? src.Ride.Driver.User.Name : null));

            //Driver controller
            CreateMap<Ride, CompletedRideDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null));

        }
    }
}
