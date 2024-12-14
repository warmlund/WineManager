using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WineManager.EntityModels;

[Table("producer")]
public partial class Producer
{
    [Key]
    [Column("producerId")]
    public int ProducerId { get; set; }

    [Required]
    [StringLength(50)]
    [Column("producerName")]
    public string ProducerName { get; set; } = null!;

    [Required]
    [StringLength(50)]
    [Column("country")]
    public string Country { get; set; } = null!;

    [Required]
    [StringLength(50)]
    [Column("region")]
    public string Region { get; set; } = null!;

    [InverseProperty("Producer")]
    public virtual ICollection<Wine> Wines { get; set; } = new List<Wine>();
}
