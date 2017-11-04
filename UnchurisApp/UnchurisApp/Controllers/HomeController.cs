using UnchurisApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace UnchurisApp.Controllers {
  
  public class HomeController : AdvertisementControllerBase {

    public HomeController() : base() { }

    public ActionResult Index() {
      if (!Security.IsAuthenticated) {
        return View("Landing", new LoginSignupViewModel());
      }

      var timeline = Advertisements.GetTimelineFor(Security.UserId).ToArray();

      return View("Timeline", timeline);
    }
    public ActionResult Profiles() {
      var users = Users.All(true);

      return View(users);
    }
    public ActionResult AdvertisementsAllUsers() {
      var advertisements = Advertisements.All(true).ToArray();

      return View(advertisements);
    }

    [HttpGet]
    [ChildActionOnly]
    public ActionResult Create() {
      return PartialView("_CreateAdvertisementPartial", new CreateAdvertisementViewModel());
    }

    [HttpPost]
    [ChildActionOnly]
    //[ValidateAntiForgeryToken]
    public ActionResult Create(CreateAdvertisementViewModel model, HttpPostedFileBase uploadImage) {
      if (ModelState.IsValid && uploadImage != null) {
        byte[] imageData = null;

        using (var binaryReader = new BinaryReader(uploadImage.InputStream)) {
          imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
        }
        Advertisements.Create(Security.UserId, model.Text, model.Title, imageData);

        Response.Redirect("/");
      }

      return PartialView("_CreateAdvertisementPartial", model);
    }

  }
}