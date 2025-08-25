using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fix_All.Models
{
    public class BookNow
    {
        [Key]
        public int BookingId { get; set; }

        // Foreign Key → User who booked
        [Required]
        [ForeignKey("UserAccount")]
        public int UserId { get; set; }
        public UserAccount UserAccount { get; set; }

        // Foreign Key → Booked Labor
        [Required]
        [ForeignKey("approve_laber")]
        public int ApproveLarberId { get; set; }
        public approve_laber ApproveLaber { get; set; }

        // Foreign Key → Service Field
        [Required]
        [ForeignKey("LaborField")]
        public int FieldId { get; set; }
        public LaborField LaborField { get; set; }

        [Required(ErrorMessage = "Booking date is required")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Enter booking address")]
        [MaxLength(250)]
        public string ServiceAddress { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    }
}
