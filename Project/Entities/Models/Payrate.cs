using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Table("payrates")]
public partial class Payrate
{
    [Key]
    [Column("payrate_id")]
    public int PayrateId { get; set; }

    [Column("physician_id")]
    public int? PhysicianId { get; set; }

    [Column("shift")]
    public int? Shift { get; set; }

    [Column("nightshift_weekend")]
    public int? NightshiftWeekend { get; set; }

    [Column("housecall")]
    public int? Housecall { get; set; }

    [Column("housecallnight_weekend")]
    public int? HousecallnightWeekend { get; set; }

    [Column("phone_consult")]
    public int? PhoneConsult { get; set; }

    [Column("phone_consult_night_weekend")]
    public int? PhoneConsultNightWeekend { get; set; }

    [Column("batch_testing")]
    public int? BatchTesting { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("Payrates")]
    public virtual Physician? Physician { get; set; }
}
