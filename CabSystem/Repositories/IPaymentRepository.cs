using CabSystem.Models;

namespace CabSystem.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> InsertPaymentAsync(Payment payment);
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<Payment?> GetPaymentByRideIdAsync(int rideId);
        Task<List<Payment>> GetPaymentsByUserIdAsync(int userId);
        Task<Payment> InsertPaymentForLatestUnpaidRideAsync(int userId, string method);


    }
}
