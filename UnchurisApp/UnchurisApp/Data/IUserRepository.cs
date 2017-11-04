using UnchurisApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnchurisApp.Data {
  public interface IUserRepository : IRepository<User> {
    IQueryable<User> All(bool includeProfile);
    User GetBy(int id, bool includeProfile = false, bool includeAdvertisements = false);
    User GetBy(string username, bool includeProfile = false, bool includeAdvertisements = false);
  }
}
