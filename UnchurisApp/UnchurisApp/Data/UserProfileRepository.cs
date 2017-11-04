using System.Data.Entity;
using UnchurisApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnchurisApp.Data {
  public class UserProfileRepository : EfRepository<UserProfile>, IUserProfileRepository {
    public UserProfileRepository(DbContext context, bool sharedContext) : base(context, sharedContext) { }
  }
}