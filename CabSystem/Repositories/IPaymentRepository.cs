using CabSystem.Models;

namespace CabSystem.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> InsertPaymentAsync(Payment payment);
        Task<List<Payment>> GetAllPaymentsAsync();
    }
}
