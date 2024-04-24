using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Table("encounterform")]
public partial class Encounterform
{
    [Key]
    [Column("encounterformid")]
    public int Encounterformid { get; set; }

    [Column("requestid")]
    public int? Requestid { get; set; }

    [Column("historyofpresentillnessorinjury")]
    public string? Historyofpresentillnessorinjury { get; set; }

    [Column("medicalhistory")]
    public string? Medicalhistory { get; set; }

    [Column("medications")]
    public string? Medications { get; set; }

    [Column("allergies")]
    public string? Allergies { get; set; }

    [Column("temp")]
    public string? Temp { get; set; }

    [Column("hr")]
    public string? Hr { get; set; }

    [Column("rr")]
    public string? Rr { get; set; }

    [Column("bloodpressuresystolic")]
    public string? Bloodpressuresystolic { get; set; }

    [Column("bloodpressurediastolic")]
    public string? Bloodpressurediastolic { get; set; }

    [Column("o2")]
    public string? O2 { get; set; }

    [Column("pain")]
    public string? Pain { get; set; }

    [Column("heent")]
    public string? Heent { get; set; }

    [Column("cv")]
    public string? Cv { get; set; }

    [Column("chest")]
    public string? Chest { get; set; }

    [Column("abd")]
    public string? Abd { get; set; }

    [Column("extremeties")]
    public string? Extremeties { get; set; }

    [Column("skin")]
    public string? Skin { get; set; }

    [Column("neuro")]
    public string? Neuro { get; set; }

    [Column("other")]
    public string? Other { get; set; }

    [Column("diagnosis")]
    public string? Diagnosis { get; set; }

    [Column("treatmentplan")]
    public string? Treatmentplan { get; set; }

    [Column("medicationsdispensed")]
    public string? Medicationsdispensed { get; set; }

    [Column("procedures")]
    public string? Procedures { get; set; }

    [Column("followup")]
    public string? Followup { get; set; }

    [Column("adminid")]
    public int? Adminid { get; set; }

    [Column("physicianid")]
    public int? Physicianid { get; set; }

    [Column("isfinalize")]
    public bool Isfinalize { get; set; }

    [ForeignKey("Adminid")]
    [InverseProperty("Encounterforms")]
    public virtual Admin? Admin { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Encounterforms")]
    public virtual Physician? Physician { get; set; }

    [ForeignKey("Requestid")]
    [InverseProperty("Encounterforms")]
    public virtual Request? Request { get; set; }
}
