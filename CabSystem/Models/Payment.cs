using System;
using System.Collections.Generic;

namespace CabSystem.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int RideId { get; set; }

    public decimal Amount { get; set; }

    public string Method { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? Timestamp { get; set; }

    public virtual Ride Ride { get; set; } = null!;
}
