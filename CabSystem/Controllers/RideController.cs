using System.Security.Claims;
using AutoMapper;
using CabSystem.DTOs;
using CabSystem.Exceptions; // ✅ Use your existing custom exception namespace
using CabSystem.Models;
using CabSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CabSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class RideController : ControllerBase
    {
        private readonly IRideRepository _rideRepository;
        private readonly IMapper _mapper;
        private readonly IPaymentRepository paymentRepository;
        private readonly IRideFareService fareService;

        public RideController(IRideRepository rideRepository, IMapper mapper, IPaymentRepository paymentRepository, IRideFareService fareService)
        {
            _rideRepository = rideRepository;
            _mapper = mapper;
            this.paymentRepository = paymentRepository;
            this.fareService = fareService;
        }

        //USER-ONLY: Get rides by userId
        [Authorize(Roles = "User")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRidesByUserId(int userId)
        {
            var rides = await _rideRepository.GetRidesByUserIdAsync(userId);
            if (rides == null || !rides.Any())
                throw new NotFoundException($"No rides found for user ID {userId}");

            var rideDtos = _mapper.Map<IEnumerable<RideDTO>>(rides);
            return Ok(rideDtos);
        }

        //USER-ONLY: Book a ride
        [Authorize(Roles = "User")]
        [HttpPost("book")]
        public async Task<IActionResult> BookRide([FromBody] CreateRideDTO dto)
        {
            var userId = GetUserIdFromToken();

            // 🛑 Check if there's any unpaid ride
            var unpaidRide = await _rideRepository.GetLatestUnpaidRideByUserIdAsync(userId);
            if (unpaidRide != null)
                throw new BadRequestException("You already have an unpaid ride. Please complete payment first.");

            var ride = _mapper.Map<Ride>(dto);
            ride.UserId = userId;
            ride.Status = "Requested";
            ride.Fare = fareService.CalculateFare(dto.PickupLocation, dto.DropoffLocation);

            var result = await _rideRepository.BookRideAsync(ride);
            return Ok(_mapper.Map<RideDTO>(result));
        }

        [Authorize(Roles = "User")]
        [HttpGet("locations")]
        public async Task<IActionResult> GetLocations()
        {
            var locations = new List<string>
        {
            "Delhi", "Noida", "Chennai", "Kerala", "Mumbai", "Pune", "Bangalore", "Hyderabad",
            "Kolkata", "Howrah", "Lucknow", "Kanpur", "Indore", "Bhopal", "Nagpur", "Ahmedabad",
            "Surat", "Jaipur", "Udaipur", "Chandigarh", "Shimla", "Goa", "Agra", "Mathura",
            "Varanasi", "Allahabad", "Coimbatore", "Ooty", "Guwahati", "Shillong", "Bhubaneswar",
            "Puri", "Patna", "Gaya", "Amritsar", "Ludhiana", "Jodhpur", "Ranchi", "Jamshedpur",
            "Visakhapatnam", "Vijayawada", "Nashik", "Shirdi", "Dehradun", "Haridwar", "Pondicherry",
            "Durgapur", "Faizabad", "Cochin", "Alleppey", "Mysore"
        };

            // Simulate an asynchronous operation
            return await Task.FromResult(Ok(locations));
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token.");
            return int.Parse(userIdClaim.Value);
        }



        
/*        [Authorize(Roles = "Driver")]
        [HttpPost("complete/{rideId}")]
        public async Task<IActionResult> CompleteRide(int rideId)
        {
            var ride = await _rideRepository.CompleteRideAsync(rideId);
            if (ride == null)
                throw new NotFoundException($"Ride with ID {rideId} not found");

            var payment = new Payment
            {
                RideId = rideId,
                Amount = ride.Fare,
                Method = "Cash",
                Status = "Paid",
                Timestamp = DateTime.UtcNow
            };

            var insertedPayment = await paymentRepository.InsertPaymentAsync(payment);

            return Ok(new
            {
                Message = "Thank you for riding with us!",
                Ride = _mapper.Map<CompleteRideResponseDTO>(ride),
                Payment = _mapper.Map<PaymentDTO>(insertedPayment)
            });
        }*/
    }
}
