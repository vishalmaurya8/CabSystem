using AutoMapper;
using CabSystem.DTOs;
using CabSystem.Exceptions;
using CabSystem.Models;
using CabSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> MakePayment([FromBody] CreatePaymentDTO dto)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Invalid payment data.");

            var existing = await _paymentRepo.GetPaymentByRideIdAsync(dto.RideId);
            if (existing != null)
                throw new ConflictException("Payment already exists for this ride.");

            var payment = _mapper.Map<Payment>(dto);
            var result = await _paymentRepo.InsertPaymentAsync(payment);

            return Ok(_mapper.Map<PaymentDTO>(result));
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentRepo.GetAllPaymentsAsync();
            if (payments == null || !payments.Any())
                throw new NotFoundException("No payments found.");

            var paymentDtos = _mapper.Map<IEnumerable<PaymentDTO>>(payments);
            return Ok(paymentDtos);
        }
    }
}
