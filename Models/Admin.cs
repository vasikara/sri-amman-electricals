using System;
using System.ComponentModel.DataAnnotations;

namespace JourneySpire.Models
{
    public class AdminModel
    {
        [Key]
        public int AdminId { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100)]
        public string ? FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [StringLength(100)]
        public string ? Email { get; set; }

        // Used only for Register / Login input
        [Required(ErrorMessage = "Password is required")]
        [StringLength(255)]
        public string ? Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [StringLength(50)]
        public string ? Role { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(20)]
        public string ? Phone { get; set; }

        public DateTime CreatedDate { get; set; }
     }
}
