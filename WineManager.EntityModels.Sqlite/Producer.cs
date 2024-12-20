using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
namespace WineManager.EntityModels;

/// <summary>
/// Entity model for producer
/// </summary>

[Table("producer")]
public partial class Producer
{
    [Key]
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
    [XmlIgnore]
    public virtual ICollection<Wine> Wines { get; set; } = new List<Wine>();
}
