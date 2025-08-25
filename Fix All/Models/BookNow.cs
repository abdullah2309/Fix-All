using Fix_All.Models;
using System.ComponentModel.DataAnnotations;

public class BookNow
{
    [Key]
    public int BookingId { get; set; }

    // Foreign Key → User
    [Required]
    public int UserId { get; set; }
    public UserAccount UserAccount { get; set; }

    // Foreign Key → Labor
    [Required]
    public int ApproveLarberId { get; set; }
    public approve_laber ApproveLaber { get; set; }  // navigation property

    [Required]
    public DateTime BookingDate { get; set; } = DateTime.Now;

    [Required]
    [StringLength(250)]
    public string ServiceAddress { get; set; }

    [Required]
    public DateTime ServiceDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending";
}
