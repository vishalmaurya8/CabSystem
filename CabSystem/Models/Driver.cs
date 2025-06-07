using System;
using System.Collections.Generic;

namespace CabSystem.Models;

public partial class Driver
{
    public int DriverId { get; set; }

    public string LicenseNo { get; set; } = null!;

    public string VehicleDetails { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<Ride> Rides { get; set; } = new List<Ride>();

    public virtual User User { get; set; } = null!;
}
