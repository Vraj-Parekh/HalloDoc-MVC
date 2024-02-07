using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc_Project.Models;

[Table("aspnetusers")]
public partial class Aspnetuser
{
    [Key]
    [Column("aspnetuserid")]
    [StringLength(128)]
    public string Aspnetuserid { get; set; } = null!;

    [Column("username")]
    public string Username { get; set; } = null!;

    [Column("passwordhash")]
    [StringLength(255)]
    public string? Passwordhash { get; set; }

    [Column("securitystamp")]
    [StringLength(255)]
    public string? Securitystamp { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("emailconfirmed", TypeName = "bit(1)")]
    public BitArray Emailconfirmed { get; set; } = null!;

    [Column("phonenumber")]
    [StringLength(20)]
    public string? Phonenumber { get; set; }

    [Column("phonenumberconfirmed", TypeName = "bit(1)")]
    public BitArray Phonenumberconfirmed { get; set; } = null!;

    [Column("twofactorenabled", TypeName = "bit(1)")]
    public BitArray Twofactorenabled { get; set; } = null!;

    [Column("lockoutenddateutc", TypeName = "timestamp without time zone")]
    public DateTime? Lockoutenddateutc { get; set; }

    [Column("lockoutenabled", TypeName = "bit(1)")]
    public BitArray Lockoutenabled { get; set; } = null!;

    [Column("accessfailedcount")]
    public int Accessfailedcount { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column("corepasswordhash")]
    [StringLength(255)]
    public string? Corepasswordhash { get; set; }

    [Column("hashversion")]
    public int? Hashversion { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [InverseProperty("Aspnetuser")]
    public virtual ICollection<Admin> AdminAspnetusers { get; set; } = new List<Admin>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Admin> AdminModifiedbyNavigations { get; set; } = new List<Admin>();

    [InverseProperty("User")]
    public virtual ICollection<Aspnetuserrole> Aspnetuserroles { get; set; } = new List<Aspnetuserrole>();

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<Business> BusinessCreatedbyNavigations { get; set; } = new List<Business>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Business> BusinessModifiedbyNavigations { get; set; } = new List<Business>();

    [InverseProperty("Aspnetuser")]
    public virtual ICollection<Physician> PhysicianAspnetusers { get; set; } = new List<Physician>();

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<Physician> PhysicianCreatedbyNavigations { get; set; } = new List<Physician>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Physician> PhysicianModifiedbyNavigations { get; set; } = new List<Physician>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Shiftdetail> Shiftdetails { get; set; } = new List<Shiftdetail>();

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();

    [InverseProperty("Aspnetuser")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
