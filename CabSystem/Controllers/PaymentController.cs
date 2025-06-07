using AutoMapper;
using CabSystem.DTOs;
using CabSystem.Models;
using CabSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CabSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentRepository paymentRepo, IMapper mapper)
        {
            _paymentRepo = paymentRepo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment([FromBody] CreatePaymentDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var payment = _mapper.Map<Payment>(dto);
            var result = await _paymentRepo.InsertPaymentAsync(payment);
            return Ok(_mapper.Map<PaymentDTO>(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentRepo.GetAllPaymentsAsync();
            var paymentDtos = _mapper.Map<IEnumerable<PaymentDTO>>(payments);
            return Ok(paymentDtos);
        }
    }
}
