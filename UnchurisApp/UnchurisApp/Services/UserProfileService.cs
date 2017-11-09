using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UnchurisApp.Data;
using UnchurisApp.Models;
using UnchurisApp.ViewModel;

namespace UnchurisApp.Services {
  public class UserProfileService : IUserProfileService {
    private readonly IContext _context;
    private readonly IUserProfileRepository _profiles;

    public UserProfileService(IContext context) {
      _context = context;
      _profiles = context.Profiles;
    }

    public UserProfile GetBy(int id) {
      return _profiles.Find(p => p.Id == id);
    }

    public void Update(EditProfileViewModel model) {
      var profile = new UserProfile() {
        Id = model.Id,
        Bio = model.Bio,
        Email = model.Email,
        Name = model.Name,
        Phone = model.Phone
      };

      _profiles.Update(profile);

      _context.SaveChanges();
    }
  }
}