using UnchurisApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using UnchurisApp.Models;
using System.Dynamic;

namespace UnchurisApp.Controllers {

  public class HomeController : AdvertisementControllerBase {

    public HomeController() : base() { }

    public ActionResult Index() {
      if (!Security.IsAuthenticated) {
        return View("landing", new LoginSignupViewModel());
      }

      var timeline = Advertisements.GetTimelineFor(Security.UserId).ToArray();
      return View("Timeline", timeline);
    }

    public ActionResult Profiles() {
      var users = Users.All(true);
      return View(users);
    }

    public void AdvertisementsAllUsers(string title, string text, string author) {
      Advertisement[] advertisements = null;
      advertisements = !String.IsNullOrEmpty(title) || !String.IsNullOrEmpty(text) || !String.IsNullOrEmpty(author) ?
        Advertisements.Search(s => s.Title.Contains(title) && s.Text.Contains(text) && s.Author.Profile.Name.Contains(author)).ToArray() :
        Advertisements.All(true).ToArray();
      var adv = new List<dynamic>();
      dynamic rez = new ExpandoObject();
      foreach (var advertisement in advertisements) {
        rez.Title = advertisement.Title;
        rez.Text = advertisement.Text;
        rez.DateCreated = advertisement.DateCreated;
        rez.Image = Convert.ToBase64String(advertisement.Image);
        rez.Id = advertisement.Id;
        rez.Author = advertisement.Author.Profile.Name;
        adv.Add(rez);
      }
      ResponseData.WriteList(Response, "result", adv);
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