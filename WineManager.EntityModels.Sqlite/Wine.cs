using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WineManager.EntityModels;

[Table("wine")]
public partial class Wine
{
    [Key]
    [Column("wineId")]
    public int WineId { get; set; }

    [Required]
    [StringLength(50)]
    [Column("wineName")]
    public string WineName { get; set; } = null!;

    [Required]
    [Range(187, 1500)]
    [Column("bottleSize")]
    public int BottleSize { get; set; }

    [Required]
    [Range(0.0, 20.0)]
    [Column("alcoholContent")]
    public double AlcoholContent { get; set; }

    [Required]
    [Column("producerId")]
    public int ProducerId { get; set; }

    [Required]
    [ForeignKey("ProducerId")]
    [InverseProperty("Wines")]
    public virtual Producer Producer { get; set; } = null!;
}
