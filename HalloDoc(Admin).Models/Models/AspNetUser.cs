using System;
using System.Collections.Generic;

namespace HalloDoc_Admin_.Entities.Models;

public partial class AspNetUser
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Ip { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<Admin> AdminAspNetUsers { get; } = new List<Admin>();

    public virtual ICollection<Admin> AdminCreatedByNavigations { get; } = new List<Admin>();

    public virtual ICollection<Admin> AdminModifiedByNavigations { get; } = new List<Admin>();

    public virtual ICollection<Business> BusinessCreatedByNavigations { get; } = new List<Business>();

    public virtual ICollection<Business> BusinessModifiedByNavigations { get; } = new List<Business>();

    public virtual ICollection<Physician> PhysicianAspNetUsers { get; } = new List<Physician>();

    public virtual ICollection<Physician> PhysicianCreatedByNavigations { get; } = new List<Physician>();

    public virtual ICollection<Physician> PhysicianModifiedByNavigations { get; } = new List<Physician>();

    public virtual ICollection<RequestNote> RequestNoteCreatedByNavigations { get; } = new List<RequestNote>();

    public virtual ICollection<RequestNote> RequestNoteModifiedByNavigations { get; } = new List<RequestNote>();

    public virtual ICollection<ShiftDetail> ShiftDetails { get; } = new List<ShiftDetail>();

    public virtual ICollection<Shift> Shifts { get; } = new List<Shift>();

    public virtual ICollection<User> UserAspNetUsers { get; } = new List<User>();

    public virtual ICollection<User> UserCreatedByNavigations { get; } = new List<User>();

    public virtual ICollection<User> UserModifiedByNavigations { get; } = new List<User>();

    public virtual ICollection<AspNetRole> Roles { get; } = new List<AspNetRole>();
}
