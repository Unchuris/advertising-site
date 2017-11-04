using UnchurisApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace UnchurisApp.Data {
  public class UserRepository : EfRepository<User>, IUserRepository {
    public UserRepository(DbContext context, bool sharedContext) : base(context, sharedContext) { }

    public IQueryable<User> All(bool includeProfile) {
      return includeProfile ? DbSet.Include(u => u.Profile).AsQueryable() : All();
    }
    public User GetBy(int id, bool includeProfile = false, bool includeAdvertisements = false) {
      var query = BuildUserQuery(includeProfile, includeAdvertisements);

      return query.SingleOrDefault(u => u.Id == id);
    }

    private IQueryable<User> BuildUserQuery(bool includeProfile, bool includeAdvertisements) {
      var query = DbSet.AsQueryable();
      if (includeProfile)
        query = DbSet.Include(u => u.Profile);

      if (includeAdvertisements)
        query = DbSet.Include(u => u.Advertisements);
      return query;
    }

    public User GetBy(string username, bool includeProfile = false, bool includeAdvertisements = false) {
      var query = BuildUserQuery(includeProfile, includeAdvertisements);

      return query.SingleOrDefault(u => u.Username == username);

    }
  }
}