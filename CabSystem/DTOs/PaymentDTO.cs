﻿namespace CabSystem.DTOs
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int RideId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
