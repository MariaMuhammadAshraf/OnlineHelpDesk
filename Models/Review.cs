using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineHelpDesk.Models;

public partial class Review
{
    public int Id { get; set; }

    [Required]
    public string ClientName { get; set; } = string.Empty;

    [Required]
    public string Profession { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; }

    public string? ImagePath { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }  // For image upload from form
}
