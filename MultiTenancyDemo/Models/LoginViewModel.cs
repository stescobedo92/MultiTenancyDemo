using System.ComponentModel.DataAnnotations;

namespace MultiTenancyDemo.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "The field {0} is required")]
        [EmailAddress(ErrorMessage = "The field must be a valid email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "The field {0} is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Remind me")]
        public bool RemindMe { get; set; }
    }
}
