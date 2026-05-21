using System;
using System.ComponentModel.DataAnnotations;

namespace JourneySpire.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        public string ?  ReportType { get; set; }   // Booking / Payment / Guide

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string ? GeneratedBy { get; set; }

        public DateTime GeneratedOn { get; set; }
    }
}
