using System.ComponentModel.DataAnnotations;

namespace JourneySpire.Models
{
    public class Route
    {
        public int RouteId { get; set; }

        [Required(ErrorMessage = "Source Location is required")]
        public string ? SourceLocation { get; set; }

        [Required(ErrorMessage = "Destination Location is required")]
        public string ? DestinationLocation { get; set; }

        [Required(ErrorMessage = "Distance is required")]
        public decimal DistanceKm { get; set; }

        [Required(ErrorMessage = "Estimated Time is required")]
        public string ? EstimatedTime { get; set; }
        public bool IsActive { get; internal set; }
    }
}
