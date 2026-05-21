using System;

namespace JourneySpire.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int BookingId { get; set; }

        public string ? CustomerName { get; set; }

        public string ? PackageName { get; set; }

        public decimal Amount { get; set; }

        public string ? PaymentMode { get; set; }      // Cash / Card / UPI

        public string ? PaymentStatus { get; set; }    // Paid / Pending / Failed

        public string ? TransactionId { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
