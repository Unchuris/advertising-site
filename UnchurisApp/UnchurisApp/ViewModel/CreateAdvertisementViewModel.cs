using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UnchurisApp.ViewModel {
  public class CreateAdvertisementViewModel {
    [Required]
    [MaxLength(140, ErrorMessage = "Text cannot be more than 140 characters.")]
    public string Text { get; set; }

    [Required]
    [MaxLength(30, ErrorMessage = "Title cannot be more than 140 characters.")]
    public string Title { get; set; }

    public byte[] Image { get; set; }
  }
}