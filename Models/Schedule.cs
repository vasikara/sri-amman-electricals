using System;
using System.ComponentModel.DataAnnotations;

namespace JourneySpire.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public int DriverId { get; set; }

        [Required]
        public int RouteId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ScheduleDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        public string Status { get; set; } = "Scheduled";

        /*  Display Fields */
        public string ? VehicleNumber { get; set; }
        public string ? DriverName { get; set; }
        public string ? SourceLocation { get; set; }
        public string ? DestinationLocation { get; set; }
        public DateTime CreatedDate { get; internal set; }
    }
}
