using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WineManager.EntityModels;

[Table("wine")]
public partial class Wine
{
    [Key]
    [Column("wineId")]
    public int WineId { get; set; }

    [Required]
    [Column("wineName")]
    public string WineName { get; set; } = null!;

    [Required]
    [Column("bottleSize")]
    public int BottleSize { get; set; }

    [Required]
    [Column("alcoholContent")]
    public double AlcoholContent { get; set; }

    [Required]
    [Column("producerName")]
    public string ProducerName { get; set; } = null!;

    [InverseProperty("Wines")]
    public virtual Producer? Producer { get; set; } // Nullable navigation property
}
