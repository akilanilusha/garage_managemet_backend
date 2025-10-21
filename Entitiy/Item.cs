using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace garage_managemet_backend_api
{
    [Table("Item")]
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }

        [Column("item_name")]
        [Required]
        public string ItemName { get; set; } = string.Empty;

        [Column("item_code")]
        [Required]
        public string ItemCode { get; set; } = string.Empty;

        [Column("qty")]
        public int Qty { get; set; }

        [Column("mesure_unit")]
        public string MesureUnit { get; set; } = string.Empty;

        [Column("unit_price")]
        public decimal UnitPrice { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("is_delete")]
        public bool IsDelete { get; set; } = false;
    }
}
