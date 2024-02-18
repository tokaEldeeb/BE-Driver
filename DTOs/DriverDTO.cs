using System.ComponentModel.DataAnnotations;

public class DriverDTO
{

    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name must be less than or equal to 50 characters")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(50, ErrorMessage = "Last Name must be less than or equal to 50 characters")]
    public string LastName { get; set; }

    [RegularExpression(@"^\+\d{1,3}\s(\d{10,11})$", ErrorMessage = "Invalid phone number")]
    public string? Phone { get; set; }
}