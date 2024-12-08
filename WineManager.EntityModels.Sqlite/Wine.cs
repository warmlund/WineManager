using System;
using System.Collections.Generic;
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

    [Column("wineName")]
    public string WineName { get; set; } = null!;

    [Column("bottleSize")]
    public int BottleSize { get; set; }

    [Column("alcoholContent")]
    public double AlcoholContent { get; set; }

    [Column("producerId")]
    public int ProducerId { get; set; }

    [ForeignKey("ProducerId")]
    [InverseProperty("Wines")]
    public virtual Producer Producer { get; set; } = null!;
}
