using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace garage_management_backend_api.Entity
{
    [Table("Service_Record")]
    public class ServiceRecord
    {
        [Key]
        [Column("ServiceID")]
        public int ServiceID { get; set; }

        [Column("VehicleID")]
        public int VehicleID { get; set; }

        [Column("AppointmentID")]
        public int AppointmentID { get; set; }

        [Column("MechanicID")]
        public int MechanicID { get; set; }

        [Column("Description")]
        [StringLength(250)]
        public string? Description { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("payment_id")]
        public int PaymentID { get; set; }

        [Column("is_delete")]
        public bool IsDeleted { get; set; }
    }
}
