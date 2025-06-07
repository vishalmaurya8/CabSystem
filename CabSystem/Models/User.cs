using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CabSystem.Models;

public partial class User
{
    [Key]
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public long Phone { get; set; }

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string Role { get; set; } = null!;

    public virtual Driver? Driver { get; set; }

    public virtual ICollection<Ride> Rides { get; set; } = new List<Ride>();
}
