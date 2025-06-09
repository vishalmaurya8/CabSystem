using AutoMapper;
using CabSystem.DTOs;
using CabSystem.Models;
using CabSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CabSystem.Exceptions; // ✅ Use your existing custom exception namespace

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

        public RideController(IRideRepository rideRepository, IMapper mapper, IPaymentRepository paymentRepository)
        {
            _rideRepository = rideRepository;
            _mapper = mapper;
            this.paymentRepository = paymentRepository;
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

            var ride = _mapper.Map<Ride>(dto);

            if (ride == null)
                throw new BadRequestException("Failed to create ride from input");

            var result = await _rideRepository.BookRideAsync(ride);
            var rideDto = _mapper.Map<RideDTO>(result);
            return Ok(rideDto);
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
