using UnchurisApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UnchurisApp.Controllers {
  public class AccountController : AdvertisementControllerBase {
    [HttpPost]
    public void SignUp(LoginSignupViewModel model) {
      //if (Security.IsAuthenticated) {
      //  ResponseData.WriteFalse(Response);
      //  return;
      //}

      model.Login = new LoginViewModel();

      var signup = model.Signup;

      if (!ModelState.IsValid) {
        ResponseData.WriteFalse(Response);
        return;
      }

      if (Security.DoesUserExist(signup.Username)) {
        ModelState.AddModelError("Username", "Username is already taken.");

        ResponseData.WriteFalse(Response);
        return;
      }

      Security.CreateUser(signup);

      ResponseData.WriteTrue(Response);
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public void Login(LoginSignupViewModel model) {
      //if (Security.IsAuthenticated) {
      //  Security.Logout();
      //}

      model.Signup = new SignupViewModel();

      var login = model.Login;

      if (!ModelState.IsValid) {
        ResponseData.WriteFalse(Response);
        return;
      }

      if (!Security.Authenticate(login.Username, login.Password)) {
        ModelState.AddModelError("Username", "Username and/or password was incorrect.");

        ResponseData.WriteFalse(Response);
        return;
      }

      Security.Login(login.Username);
      ResponseData.WriteTrue(Response);
    }

    public void Authenticated() {
      if (Security.IsAuthenticated) { 
        ResponseData.WriteTrue(Response);
      } else {
        ResponseData.WriteFalse(Response);
      }
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public void Logout() {
      Security.Logout();

      ResponseData.WriteTrue(Response);
    }
  }
}