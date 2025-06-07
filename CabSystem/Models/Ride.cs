using System;
using System.Collections.Generic;

namespace CabSystem.Models;

public partial class Ride
{
    public int RideId { get; set; }

    public int UserId { get; set; }

    public int DriverId { get; set; }

    public string PickupLocation { get; set; } = null!;

    public string DropoffLocation { get; set; } = null!;

    public decimal Fare { get; set; }

    public string Status { get; set; } = null!;

    public virtual Driver Driver { get; set; } = null!;

    public virtual Payment? Payment { get; set; }

    public virtual Rating? Rating { get; set; }

    public virtual User User { get; set; } = null!;
}
