using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JourneySpire.Models
{
    public class PaymentReport
    {
        [Key]
        public int PaymentReportId { get; set; }

        [ForeignKey("Report")]
        public int ReportId { get; set; }

        public int PaymentId { get; set; }

        public int BookingId { get; set; }

        public string ? CustomerName { get; set; }

        public decimal Amount { get; set; }

        public string ? PaymentMethod { get; set; }

        public string ? PaymentStatus { get; set; }

        public DateTime PaymentDate { get; set; }

        public Report ? Report { get; set; }
    }
}
