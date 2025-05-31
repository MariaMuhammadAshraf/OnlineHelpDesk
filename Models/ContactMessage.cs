using System;
using System.Collections.Generic;

namespace OnlineHelpDesk.Models;

public partial class ContactMessage
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public DateTime? SubmittedAt { get; set; }
}
