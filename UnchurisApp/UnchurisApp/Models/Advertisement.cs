using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UnchurisApp.Models {

  public class Advertisement {
    public int Id { get; set; }

    public int AuthorId { get; set; }

    [ForeignKey("AuthorId")]
    public virtual User Author { get; set; }

    public DateTime DateCreated { get; set; }
    public string Text { get; set; }
    public string Title { get; set; }
    public byte[] Image { get; set; }
  }
}