using UnchurisApp.Models;
using UnchurisApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace UnchurisApp.Services {
  public interface IUserProfileService {
    UserProfile GetBy(int id);
    UserProfile Update(EditProfileViewModel model);
  }
}
