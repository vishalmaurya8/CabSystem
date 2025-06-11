using AutoMapper;
using CabSystem.DTOs;
using CabSystem.Exceptions; // ✅ Use your existing custom exception namespace
using CabSystem.Models;
using CabSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CabSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔐 Require authentication globally in this controller
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

        // 🧍 USER-ONLY: Get rides by userId
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

        // 🧍 USER-ONLY: Book a ride
        [Authorize(Roles = "User")]
        [HttpPost("book")]
        public async Task<IActionResult> BookRide([FromBody] CreateRideDTO dto)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid ride data");

            var userId = GetUserIdFromToken(); // 🔐 get from JWT token

            var ride = _mapper.Map<Ride>(dto);

            if (ride == null)
                throw new BadRequestException("Failed to create ride from input");

            // 🔐 Assign user ID securely
            ride.UserId = userId;

            // 🧠 Auto-calculate fare (lookup dummy data or fallback)
            ride.Fare = fareService.CalculateFare(dto.PickupLocation, dto.DropoffLocation);

            // ✅ Ride is set to "Requested" inside repository
            var result = await _rideRepository.BookRideAsync(ride);

            var rideDto = _mapper.Map<RideDTO>(result);
            return Ok(rideDto);
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token.");
            return int.Parse(userIdClaim.Value);
        }



        // 🚗 DRIVER-ONLY: Complete a ride
        [Authorize(Roles = "Driver")]
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
        }



    }
}
