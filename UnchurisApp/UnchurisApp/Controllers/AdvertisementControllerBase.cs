using UnchurisApp.Data;
using UnchurisApp.Models;
using UnchurisApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace UnchurisApp.Controllers {
  public class AdvertisementControllerBase : Controller {
    protected IContext DataContext;
    public User CurrentUser { get; private set; }
    public IAdvertisementService Advertisements { get; private set; }
    public IUserService Users { get; private set; }
    public ISecurityService Security { get; private set; }
    public IUserProfileService Profiles { get; private set; }

    public AdvertisementControllerBase() {
      DataContext = new Context();
      Users = new UserService(DataContext);
      Advertisements = new AdvertisementService(DataContext);
      Security = new SecurityService(Users);
      CurrentUser = Security.GetCurrentUser();
      Profiles = new UserProfileService(DataContext);
    }

    protected override void Dispose(bool disposing) {
      if (DataContext != null) {
        DataContext.Dispose();
      }
      base.Dispose(disposing);
    }

    public ActionResult GoToReferrer() {
      if (Request.UrlReferrer != null) {
        return Redirect(Request.UrlReferrer.AbsoluteUri);
      }

      return RedirectToAction("Index", "Home");
    }
  }
} 