using System.ComponentModel.DataAnnotations;

namespace YourProject.Models
{
    public class AdminLogin
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Admin Email")]
        public string Email { get; set; } = "admin@email.com";

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "adminAbdullah123";
    }
}
