using UnchurisApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UnchurisApp.Controllers {
  public class UserController : AdvertisementControllerBase {
    public ActionResult Index(string username) {
      var user = Users.GetAllFor(username);

      if (user == null) {
        return new HttpNotFoundResult();
      }

      return View("UserProfile", new UserViewModel() {
        User = user,
        Advertisements = user.Advertisements
      });
    }
  }
}