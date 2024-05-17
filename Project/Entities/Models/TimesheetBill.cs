using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

public partial class TimesheetBill
{
    [Key]
    public int TimesheetBillId { get; set; }

    public int TimesheetDetailId { get; set; }

    [StringLength(100)]
    public string? Item { get; set; }

    public int? Amount { get; set; }

    [StringLength(100)]
    public string? FilePath { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedDate { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("TimesheetDetailId")]
    [InverseProperty("TimesheetBills")]
    public virtual TimesheetDetail TimesheetDetail { get; set; } = null!;
}
