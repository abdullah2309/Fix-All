using System.ComponentModel.DataAnnotations;

public class LaborField
{
    [Key]
    public int FieldId { get; set; }

    [Required]
    [MaxLength(100)]
    public string FieldName { get; set; }
}
