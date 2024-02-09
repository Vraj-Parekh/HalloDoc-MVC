using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDoc_Project.Models;

[Table("physiciannotification")]
public partial class Physiciannotification
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("isnotificationstopped")]
    public bool Isnotificationstopped { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Physiciannotifications")]
    public virtual Physician Physician { get; set; } = null!;
}
