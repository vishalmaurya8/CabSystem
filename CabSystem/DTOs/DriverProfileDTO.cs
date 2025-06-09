namespace CabSystem.DTOs
{
    public class DriverProfileDTO
    {
        public int DriverId { get; set; }
        public string LicenseNo { get; set; }
        public string VehicleDetails { get; set; }
        public string Status { get; set; }
        public string Name { get; set; } // from User
        public string Email { get; set; } // from User
        public string Phone { get; set; } // from User
    }
}