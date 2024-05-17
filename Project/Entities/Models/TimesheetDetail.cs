using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Table("TimesheetDetail")]
public partial class TimesheetDetail
{
    [Key]
    public int TimesheetDetailId { get; set; }

    public int TimesheetId { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime Date { get; set; }

    public int? OnCallHours { get; set; }

    public int? TotalHours { get; set; }

    public bool? IsHoliday { get; set; }

    public int? NoOfHouseCall { get; set; }

    public int? NoOfPhoneConsult { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedDate { get; set; }

    [ForeignKey("TimesheetId")]
    [InverseProperty("TimesheetDetails")]
    public virtual Timesheet Timesheet { get; set; } = null!;

    [InverseProperty("TimesheetDetail")]
    public virtual ICollection<TimesheetBill> TimesheetBills { get; set; } = new List<TimesheetBill>();
}
