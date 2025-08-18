using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fix_All.Models
{
    public class ServiceProvider
    {
        [Key]
        public int LarberId { get; set; } // Primary Key

        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^03[0-9]{9}$", ErrorMessage = "Enter a valid Pakistani phone (e.g. 03001234567)")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "CNIC is required")]
        [RegularExpression(@"^\d{5}-\d{7}-\d{1}$", ErrorMessage = "Enter valid CNIC format (e.g. 12345-6789012-3)")]
        public string CNIC { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Education level is required")]
        public string Education { get; set; }

        [Required(ErrorMessage = "Select a service field")]
        [ForeignKey("LaborField")]
        public int? FieldId { get; set; }
        public LaborField? LaborField { get; set; } // ✅ Navigation property

        public bool IsDiploma { get; set; }

        // ✅ New Experience field
        [Required(ErrorMessage = "Experience is required")]
        [RegularExpression(@"^(?:[0-9]|[1-4][0-9]|50)$", ErrorMessage = "Experience must be between 0 and 50 years")]
        public string Experience { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string PasswordHash { get; set; }

        // ✅ File paths
        public string? CVFilePath { get; set; }
        public string? ProfileImagePath { get; set; }
    }
}
