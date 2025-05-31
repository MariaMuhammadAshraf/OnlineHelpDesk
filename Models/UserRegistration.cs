using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineHelpDesk.Models;

public partial class UserRegistration
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [Required(ErrorMessage = "User name is required.")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string UserEmail { get; set; } = null!;

    public string? UserPassword { get; set; }

    public string? UserRole { get; set; }
}
