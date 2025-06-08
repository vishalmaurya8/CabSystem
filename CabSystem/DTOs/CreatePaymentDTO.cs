using System.ComponentModel.DataAnnotations;

namespace CabSystem.DTOs
{
    public class CreatePaymentDTO
    {
        [Required]
        public int RideId { get; set; }


        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public int Amount { get; set; }


        [Required]
        public string Method { get; set; }  // e.g. "Cash", "UPI", "Card"

        public string Status { get; set; }  // e.g. "Paid", "Failed", "Pending"
    }
}
