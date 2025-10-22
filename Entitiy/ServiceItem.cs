using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace garage_managemet_backend_api.Models
{
    [Table("service_item")]
    public class ServiceItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("service_id")]
        [Required]
        public int ServiceId { get; set; }

        [Column("part_id")]
        [Required]
        public int PartId { get; set; }

        [Column("qty")]
        [Required]
        public int Qty { get; set; } = 1;
    }
}
