using System.ComponentModel.DataAnnotations;

namespace Fix_All.Models
{
    public class Add_labor_Category
    {
        [Key]
        public int id { get; set; }
        [Required]
        public required string category_name { get; set; }
    }
}
