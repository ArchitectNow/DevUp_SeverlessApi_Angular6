using System.ComponentModel.DataAnnotations;

namespace ArchitectNow.Identity.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
       
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        [Display(Name = "Avatar Image Url")]
        public string ImageUrl { get; set; }       
        
    }
}