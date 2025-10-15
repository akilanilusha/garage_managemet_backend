using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace garage_managemet_backend_api.Models{
    [Table("Mechanic")]
    public class Mechanic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MechanicID { get; set; }  // Primary key, auto-incremented

        [Required]
        public string name { get; set; }

        [Required]
        public string nic_number { get; set; }  // National ID

        [Required]
        public string phone { get; set; }

        public bool is_delete { get; set; } = false;  // Soft delete
    }
}
