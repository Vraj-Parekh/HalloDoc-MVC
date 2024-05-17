using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Table("Timesheet")]
public partial class Timesheet
{
    [Key]
    public int TimesheetId { get; set; }

    public int PhysicianId { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime StartDate { get; set; }

    public bool? IsFinalize { get; set; }

    public bool? IsApproved { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? FinalizedDate { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ApprovedDate { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedDate { get; set; }

    public int? InvoiceTotal { get; set; }

    public int? Bonus { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    [StringLength(100)]
    public string? AdminDescription { get; set; }

    [StringLength(128)]
    public string? ApprovedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsSubmitted { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("Timesheets")]
    public virtual Physician Physician { get; set; } = null!;

    [InverseProperty("Timesheet")]
    public virtual ICollection<TimesheetDetail> TimesheetDetails { get; set; } = new List<TimesheetDetail>();
}
