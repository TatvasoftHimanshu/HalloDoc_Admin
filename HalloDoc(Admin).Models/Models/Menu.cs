using System;
using System.Collections.Generic;

namespace HalloDoc_Admin_.Entities.Models;

public partial class Menu
{
    public int MenuId { get; set; }

    public string Name { get; set; } = null!;

    public short AccountType { get; set; }

    public int? SortOrder { get; set; }

    public virtual ICollection<RoleMenu> RoleMenus { get; } = new List<RoleMenu>();
}
