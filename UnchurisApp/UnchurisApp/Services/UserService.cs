using UnchurisApp.Data;
using UnchurisApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace UnchurisApp.Services {
  public class UserService : IUserService {
    private readonly IContext _context;
    private readonly IUserRepository _users;

    public UserService(IContext context) {
      _context = context;
      _users = context.Users;
    }

    public IEnumerable<User> All(bool includeProfile) {
      return _users.All(includeProfile).ToArray();
    }

    public User GetAllFor(int id) {
      return _users.GetBy(id,
                          includeProfile: true,
                          includeAdvertisements: true);
    }

    public User GetAllFor(string username) {
      return _users.GetBy(username,
                          includeProfile: true,
                          includeAdvertisements: true);
    }

    public User GetBy(int id) {
      return _users.GetBy(id);
    }

    public User GetBy(string username) {
      return _users.GetBy(username);

    }

    public User Create(string username, string password,
        UserProfile profile, DateTime? created = null) {
      var user = new User() {
        Username = username,
        Password = Crypto.HashPassword(password),
        DateCreated = created.HasValue ? created.Value : DateTime.Now,
        Profile = profile
      };

      _users.Create(user);

      _context.SaveChanges();

      return user;
    }
  }
}