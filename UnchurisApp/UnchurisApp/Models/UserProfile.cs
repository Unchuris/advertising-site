using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnchurisApp.Models {
  public class UserProfile {
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Bio { get; set; }
  }
}