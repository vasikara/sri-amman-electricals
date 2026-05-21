using System;
using System.ComponentModel.DataAnnotations;

public class Customer
{
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Full Name is required")]
    [StringLength(100)]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100)]
    public string Email { get; set; }
    public string  Password { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone must be 10–15 digits")]
    public string Phone { get; set; }

    [StringLength(300)]
    public string Address { get; set; }

    public DateTime CreatedDate { get; set; }
}
