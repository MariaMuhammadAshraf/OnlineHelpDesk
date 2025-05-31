using System;
using System.Collections.Generic;

namespace OnlineHelpDesk.Models;

public partial class Aboutu
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public string? ImagePath { get; set; }
}
