using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ServiceProvider
{
    [Key]
    public int LarberId { get; set; } // Primary Key

    [Required, MaxLength(50)]
    public string FirstName { get; set; }

    [Required, MaxLength(50)]
    public string LastName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public string PasswordHash { get; set; } // Store hashed password

    [Required]
    public string CNIC { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string Education { get; set; }

    public bool IsDiploma { get; set; }

    [Required]
    public int FieldId { get; set; } // Foreign Key

    [ForeignKey("FieldId")]
    public LaborField LaborField { get; set; }

    public string CVFilePath { get; set; }
    public string ProfileImagePath { get; set; }

    public bool EmailVerified { get; set; }
}
