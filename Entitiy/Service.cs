using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace garage_managemet_backend_api.Entitiy
{
    [Table("Service_Record")]
    public class Service
    {
        [Key]
        [Column("ServiceID")]
        public int ServiceID { get; set; }

        [Column("VehicleID")]
        [ForeignKey("Vehicle")]
        public int VehicleID { get; set; }

        [Column("AppointmentID")]
        [ForeignKey("Appointment")]
        public int AppointmentID { get; set; }

        [Column("MechanicID")]
        [ForeignKey("Mechanic")]
        public int MechanicID { get; set; }

        [Column("Description")]
        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Column("start_date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Column("payment_id")]
        public int PaymentID { get; set; }

        [Column("is_delete")]
        public bool IsDelete { get; set; } = false;

        // // Navigation Properties (Optional but recommended)
        // public virtual Vehicle Vehicle { get; set; }
        // public virtual Appointment Appointment { get; set; }
        // public virtual Mechanic Mechanic { get; set; }
    }
}
