using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JourneySpire.Models
{
    public class GuideReport
    {
        [Key]
        public int GuideReportId { get; set; }

        [ForeignKey("Report")]
        public int ReportId { get; set; }

        public int GuideId { get; set; }

        public string ? FullName { get; set; }

        public string ? Phone { get; set; }

        public string ? Language { get; set; }

        public int Experience { get; set; }

        public string ? Status { get; set; }

        public bool IsAvailable { get; set; }

        public Report ? Report { get; set; }
    }
}
