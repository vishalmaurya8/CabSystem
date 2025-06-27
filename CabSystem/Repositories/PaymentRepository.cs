using CabSystem.Data;
using CabSystem.Exceptions;
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
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            //Re-fetch with Ride included
            return await _context.Payments
                .Include(p => p.Ride)
                .FirstOrDefaultAsync(p => p.PaymentId == payment.PaymentId);
        }


        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment?> GetPaymentByRideIdAsync(int rideId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.RideId == rideId);
        }

        public async Task<List<Payment>> GetPaymentsByUserIdAsync(int userId)
        {
            return await _context.Payments
                .Include(p => p.Ride)
                .ThenInclude(r => r.User)
                .Where(p => p.Ride.UserId == userId)
                .ToListAsync();
        }

        public async Task<Payment> InsertPaymentForLatestUnpaidRideAsync(int userId, string method)
        {
            var ride = await _context.Rides
                .Include(r => r.Payment)
                .Where(r => r.UserId == userId && (r.Payment == null || r.Payment.Status != "Paid"))
                .OrderByDescending(r => r.RideId)
                .FirstOrDefaultAsync();

            if (ride == null)
                throw new NotFoundException("No unpaid ride found.");

            var payment = new Payment
            {
                RideId = ride.RideId,
                Amount = ride.Fare,
                Method = method,
                Status = "Paid",
                Timestamp = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }
    }
}
