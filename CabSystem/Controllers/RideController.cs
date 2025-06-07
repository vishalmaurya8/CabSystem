// Controllers/RideController.cs
using AutoMapper;
using CabSystem.DTOs;
using CabSystem.Models;
using CabSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CabSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRidesByUserId(int userId)
        {
            var rides = await _rideRepository.GetRidesByUserIdAsync(userId);
            var rideDtos = _mapper.Map<IEnumerable<RideDTO>>(rides);
            return Ok(rideDtos);
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookRide([FromBody] CreateRideDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ride = _mapper.Map<Ride>(dto);
            var result = await _rideRepository.BookRideAsync(ride);
            var rideDto = _mapper.Map<RideDTO>(result);
            return Ok(rideDto);
        }

        [HttpPost("complete/{rideId}")]
        public async Task<IActionResult> CompleteRide(int rideId)
        {
            var ride = await _rideRepository.CompleteRideAsync(rideId);
            if (ride == null)
                return NotFound("Ride not found.");

            // ⏳ Simulate fare logic or assume ride already has Fare
            var payment = new Payment
            {
                RideId = rideId,
                Amount = ride.Fare,
                Method = "Cash", // or default
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
