using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnchurisApp.Data {
  public interface IContext : IDisposable {
    IUserRepository Users { get; }
    IAdvertisementRepository Advertisements { get; }
    IUserProfileRepository Profiles { get; }

    int SaveChanges();
  }
}
