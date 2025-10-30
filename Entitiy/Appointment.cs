using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using garage_managemet_backend_api.Entitiy;

namespace garage_managemet_backend_api.Models
{
    [Table("appointment")]
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int VehicleID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(45)]
        public string Time { get; set; }   

        [Required]
        [StringLength(100)]
        public string ServiceType { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        [StringLength(45)]
        public string Status { get; set; }

        [Column("is_delete")]
        public int IsDelete { get; set; } = 0;

         public Customer? Customer { get; set; }
        public Vehicle? Vehicle { get; set; }
    }
}
