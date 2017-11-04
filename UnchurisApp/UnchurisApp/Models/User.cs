using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UnchurisApp.Models {
  public class User {

    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime DateCreated { get; set; }

    public int UserProfileId { get; set; }

    [ForeignKey("UserProfileId")]
    public virtual UserProfile Profile { get; set; }

    private ICollection<Advertisement> _advertisements;
    public virtual ICollection<Advertisement> Advertisements {
      get { return _advertisements ?? (_advertisements = new Collection<Advertisement>()); }
      set { _advertisements = value; }
    }

  }
}