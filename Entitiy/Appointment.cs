using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Appointment")]
    public class Appointment
    {
        [Key]
        [Column("AppointmentID")]
        public int Id { get; set; }

        [Required]
        public string CustomerID { get; set; }

        [Required]
        public string VehicleID { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeOnly Time { get; set; }

        [Required]
        public string ServiceType { get; set; }

        public string? Description { get; set; }

        [Required]
        public string Status { get; set; }
    }