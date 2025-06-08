using CabSystem.Data;
using CabSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CabSystem.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CabSystemContext _context;

        public PaymentRepository(CabSystemContext context)
        {
            _context = context;
        }

        public async Task<Payment> InsertPaymentAsync(Payment payment)
        {
            payment.Timestamp = DateTime.UtcNow;
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment?> GetPaymentByRideIdAsync(int rideId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.RideId == rideId);
        }

    }
}
