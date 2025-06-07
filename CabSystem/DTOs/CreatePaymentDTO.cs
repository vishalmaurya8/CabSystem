namespace CabSystem.DTOs
{
    public class CreatePaymentDTO
    {
        public int RideId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }  // e.g. "Cash", "UPI", "Card"
        public string Status { get; set; }  // e.g. "Paid", "Failed", "Pending"
    }
}
