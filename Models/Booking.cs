using System;
using System.ComponentModel.DataAnnotations;

namespace JourneySpire.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public int PackageId { get; set; }

        [Required]
        public string ? PackageName { get; set; }

        [Required]
        public string ? CustomerName { get; set; }

        [Required, EmailAddress]
        public string ? Email { get; set; }

        [Required]
        public string ? Phone { get; set; }

        [Required]
        public DateTime TravelDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int NumberOfPersons { get; set; }

        [Required]
        public decimal Price { get; set; }

        // 🔥 AUTO CALCULATED (SQL)
        public decimal TotalAmount { get; set; }

        public string ? BookingStatus { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
