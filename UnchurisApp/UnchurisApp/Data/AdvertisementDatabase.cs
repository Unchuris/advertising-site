using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using UnchurisApp.Models;

namespace UnchurisApp.Data {
  public class AdvertisementDatabase : DbContext {

    public AdvertisementDatabase() : base("AdvertisementConnection") { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Advertisement> Advertisements { get; set; }
  }
}