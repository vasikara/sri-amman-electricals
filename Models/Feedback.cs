using System;
using System.ComponentModel.DataAnnotations;

namespace JourneySpire.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100)]
        public string ? FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string ? Email { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500)]
        public string ? Message { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        // Optional – link with booking
        public int? BookingId { get; set; }

        // Admin approval
        public bool IsApproved { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
