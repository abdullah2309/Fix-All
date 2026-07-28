using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fix_All.Models
{
    public class BookNow
    {
        [Key]
        public int BookingId { get; set; }

        // Foreign Key → User who booked
        [Required(ErrorMessage = "User is required.")]
        [ForeignKey("UserAccount")]
        public int UserId { get; set; }
        public UserAccount UserAccount { get; set; }

        // Foreign Key → Booked Labor
        [Required(ErrorMessage = "Labor selection is required.")]
        [ForeignKey("approve_laber")]
        public int ApproveLarberId { get; set; }
        public approve_laber ApproveLaber { get; set; }

        // Foreign Key → Service Field
        [Required(ErrorMessage = "Please select a service field.")]
        [ForeignKey("LaborField")]
        public int FieldId { get; set; }
        public LaborField LaborField { get; set; }

        [Required(ErrorMessage = "Booking date is required.")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Service address is required.")]
        [StringLength(250, ErrorMessage = "Service address cannot exceed 250 characters.")]
        public string ServiceAddress { get; set; }

        [StringLength(250, ErrorMessage = "Extra field cannot exceed 250 characters.")]
        public string AddMorefield { get; set; }

        [Required(ErrorMessage = "Service date is required.")]
        public DateTime ServiceDate { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "Booking status is required.")]
        [MaxLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        public string Status { get; set; } = "Pending";
    }
}
