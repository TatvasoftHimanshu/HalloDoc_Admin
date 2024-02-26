using System;
using System.Collections;
using System.Collections.Generic;

namespace HalloDoc_Admin_.Entities.Models;

public partial class ShiftDetailRegion
{
    public int ShiftDetailRegionId { get; set; }

    public int ShiftDetailId { get; set; }

    public int RegionId { get; set; }

    public BitArray? IsDeleted { get; set; }

    public virtual Region Region { get; set; } = null!;

    public virtual ShiftDetail ShiftDetail { get; set; } = null!;
}
