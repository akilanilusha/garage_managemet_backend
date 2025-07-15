using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace garage_managemet_backend_api.Models;

[Table("Customer")]
public class Customer
{
    [Key]
    [Column("CustomerID")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [Column("contact_info")]
    public string ContactInfo { get; set; }

    [Column("is_delete")]
    public bool IsDelete { get; set; }
}