using System;
using System.Collections.Generic;

namespace HalloDoc_Admin_.Entities.Models;

public partial class Region
{
    public int RegionId { get; set; }

    public string Name { get; set; } = null!;

    public string? Abbreviation { get; set; }

    public virtual ICollection<AdminRegion> AdminRegions { get; } = new List<AdminRegion>();

    public virtual ICollection<Business> Businesses { get; } = new List<Business>();

    public virtual ICollection<Concierge> Concierges { get; } = new List<Concierge>();

    public virtual ICollection<PhysicianRegion> PhysicianRegions { get; } = new List<PhysicianRegion>();

    public virtual ICollection<RequestClient> RequestClients { get; } = new List<RequestClient>();

    public virtual ICollection<ShiftDetailRegion> ShiftDetailRegions { get; } = new List<ShiftDetailRegion>();
}
