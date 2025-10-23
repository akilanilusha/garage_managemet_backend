using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace garage_managemet_backend_api.Entitiy;

public class Payment
{
        [Key]
        public uint PaymentID { get; set; }

        [Column("Amount")]
        public decimal? Amount { get; set; }

        [Column("Date")]
        public DateTime? Date { get; set; }

        [Column("PaymentType")]
        [StringLength(20)]
        public string? PaymentType { get; set; }

        [Column("is_delete")]
        public bool IsDeleted { get; set; } = false;

        [Column("service_id")]
        public uint ServiceId { get; set; }
}
