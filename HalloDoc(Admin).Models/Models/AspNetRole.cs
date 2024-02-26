using System;
using System.Collections.Generic;

namespace HalloDoc_Admin_.Entities.Models;

public partial class AspNetRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AspNetUser> Users { get; } = new List<AspNetUser>();
}
