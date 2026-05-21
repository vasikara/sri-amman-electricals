using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JourneySpire.Models
{
    public class BookingReport
    {
        [Key]
        public int BookingReportId { get; set; }

        [ForeignKey("Report")]
        public int ReportId { get; set; }

        public int BookingId { get; set; }

        public string ? CustomerName { get; set; }

        public string ? Email { get; set; }

        public string ? Phone { get; set; }

        public string ? PackageName { get; set; }

        public DateTime TravelDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string ? Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public Report ? Report { get; set; }
    }
}
