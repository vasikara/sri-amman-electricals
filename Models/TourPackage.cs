using System;
using System.ComponentModel.DataAnnotations;

namespace JourneySpire.Models
{
    public class TourPackage
    {
        public int PackageId { get; set; }

        [Required]
        public string ? PackageName { get; set; }

        [Required]
        public string ? Location { get; set; }

        [Required]
        public int Days { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ? Description { get; set; }

        public string ? ImagePath { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
