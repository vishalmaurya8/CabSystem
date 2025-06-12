using AutoMapper;
using CabSystem.DTOs;
using CabSystem.Exceptions;
using CabSystem.Models;
using CabSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CabSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // ✅ Protect these endpoints
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IMapper _mapper;
        private readonly IRideRepository rideRepository;

        public PaymentController(IPaymentRepository paymentRepo, IMapper mapper, IRideRepository rideRepository)
        {
            _paymentRepo = paymentRepo;
            _mapper = mapper;
            this.rideRepository = rideRepository;
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token.");
            return int.Parse(userIdClaim.Value);
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> MakePayment([FromBody] CreatePaymentDTO dto)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid payment data");

            var ride = await rideRepository.GetRideByIdAsync(dto.RideId);
            if (ride == null)
                throw new NotFoundException($"Ride with ID {dto.RideId} does not exist.");

            var userId = GetUserIdFromToken();
            if (ride.UserId != userId)
                throw new UnauthorizedAccessException("You cannot pay for another user's ride.");

            // ✅ Update Ride Status
            ride.Status = "Completed";

            // 🔁 Save status change
            await rideRepository.UpdateRideAsync(ride); // You’ll implement this method below

            var payment = _mapper.Map<Payment>(dto);
            payment.Timestamp = DateTime.UtcNow;

            var result = await _paymentRepo.InsertPaymentAsync(payment);
            var response = _mapper.Map<PaymentDTO>(result);
            return Ok(response);
        }



        [Authorize(Roles = "User")]
        [HttpGet("my-payments")]
        public async Task<IActionResult> GetMyPayments()
        {
            var userId = GetUserIdFromToken();

            var payments = await _paymentRepo.GetPaymentsByUserIdAsync(userId);

            if (payments == null || !payments.Any())
                throw new NotFoundException("No payments found for your account.");

            var result = _mapper.Map<List<PaymentDTO>>(payments);
            return Ok(result);
        }



    }
}
