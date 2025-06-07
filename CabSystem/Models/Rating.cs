using System;
using System.Collections.Generic;

namespace CabSystem.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public int RideId { get; set; }

    public int Score { get; set; }

    public string? Comments { get; set; }

    public virtual Ride Ride { get; set; } = null!;
}
