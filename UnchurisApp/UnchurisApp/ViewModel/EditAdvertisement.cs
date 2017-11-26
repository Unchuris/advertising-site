using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UnchurisApp.Models;

namespace UnchurisApp.ViewModel {
  public class EditAdvertisement {
    public int Id { get; set; }

    [Required]
    [MaxLength(140, ErrorMessage = "Text cannot be more than 140 characters.")]
    public string Text { get; set; }

    [Required]
    [MaxLength(30, ErrorMessage = "Title cannot be more than 140 characters.")]
    public string Title { get; set; }

    public string Image { get; set; }
  }
}