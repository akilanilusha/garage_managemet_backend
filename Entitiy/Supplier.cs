using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace garage_managemet_backend_api.Models;

[Table("Supplier")]
public class Supplier
{
    [Key]
    [Column("SupplierID")]
    public int SupplierID { get; set; }

    [Column("name")]
    [Required]
    public string Name { get; set; } = string.Empty;

    [Column("phone")]
    [Required]
    public string Phone { get; set; } = string.Empty;

    [Column("is_delete")]
    public bool IsDelete { get; set; }
}
