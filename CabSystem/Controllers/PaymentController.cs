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

        public PaymentController(IPaymentRepository paymentRepo, IMapper mapper)
        {
            _paymentRepo = paymentRepo;
            _mapper = mapper;
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

            var payment = _mapper.Map<Payment>(dto);
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
