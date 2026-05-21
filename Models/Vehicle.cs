using System;
using System.ComponentModel.DataAnnotations;

namespace JourneySpire.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Vehicle Number is required")]
        public string ? VehicleNumber { get; set; }

        [Required(ErrorMessage = "Vehicle Type is required")]
        public string ? VehicleType { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        public int Capacity { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
