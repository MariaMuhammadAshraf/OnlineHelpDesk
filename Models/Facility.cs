using System;
using System.Collections.Generic;

namespace OnlineHelpDesk.Models;

public partial class Facility
{
    public int FacilityId { get; set; }

    public string FacilityName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }
}
