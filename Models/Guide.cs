using System;
using System.ComponentModel.DataAnnotations;

namespace JourneySpire.Models
{
    public class Guide
    {
        public int GuideId { get; set; }

        [Required]
        public string ? FullName { get; set; }

        [Required]
        public string ? Phone { get; set; }

        public string ? Email { get; set; }
        public string ? Language { get; set; }
        public int Experience { get; set; }
        public string ? Photo { get; set; }

        public string Status { get; set; } = "Active";   // Active / Inactive
        public bool IsAvailable { get; set; } = true;   // Available / Unavailable

        public DateTime CreatedDate { get; set; }
    }
}
