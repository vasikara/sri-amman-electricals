using System.ComponentModel.DataAnnotations;

namespace JourneySpire.Models
{
    public class Driver
    {
        public int DriverId { get; set; }

        [Required(ErrorMessage = "Driver Name is required")]
        public string ? DriverName { get; set; }

        [Required(ErrorMessage = "License Number is required")]
        public string ? LicenseNumber { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone]
        public string ? Phone { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
