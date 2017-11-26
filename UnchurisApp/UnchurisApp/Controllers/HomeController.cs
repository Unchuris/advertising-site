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

    public void Index() {
      var result = new List<dynamic>();
      if (Security.IsAuthenticated) {
        var advertisements = Advertisements.GetTimelineFor(Security.UserId).ToArray();
        foreach (var advertisement in advertisements) {
          dynamic rez = new ExpandoObject();
          rez.Id = advertisement.Id;
          rez.Title = advertisement.Title;
          rez.Text = advertisement.Text;
          rez.Author = advertisement.Author.Profile.Name;
          rez.Image = Convert.ToBase64String(advertisement.Image);
          rez.DateCreated = advertisement.DateCreated;
          result.Add(rez);
        }
      }
      ResponseData.WriteList(Response, "result", result);
    }

    public void Profiles() {
      var users = Users.All(true);
      var result = new List<dynamic>();
      foreach (var user in users) {
        dynamic rez = new ExpandoObject();
        rez.Id = user.Id;
        rez.ProfileName = user.Profile.Name;
        rez.Username = user.Username;
        rez.AdvertisementsCount = user.Advertisements.Count;
        rez.ProfileBio = user.Profile.Bio;
        rez.DateCreated = user.DateCreated;
        result.Add(rez);
      }
      ResponseData.WriteList(Response, "result", result);
    }

    public void AdvertisementsAllUsers(string text) {
      Advertisement[] advertisements = null;
      advertisements = !String.IsNullOrEmpty(text)?
        Advertisements.Search(s => s.Text.Contains(text)).ToArray() :
        Advertisements.All(true).ToArray();
      var result = new List<dynamic>();
      foreach (var advertisement in advertisements) {
        dynamic rez = new ExpandoObject();
        rez.Id = advertisement.Id;
        rez.Title = advertisement.Title;
        rez.Text = advertisement.Text;
        rez.Author = advertisement.Author.Profile.Name;
        rez.Image = Convert.ToBase64String(advertisement.Image);
        rez.DateCreated = advertisement.DateCreated;
        result.Add(rez);
      }
      ResponseData.WriteList(Response, "result", result);
    }

    [HttpGet]
    //[ChildActionOnly]
    public ActionResult Create() {
      return PartialView("_CreateAdvertisementPartial", new CreateAdvertisementViewModel());
    }

    [HttpPost]
    //[ChildActionOnly]
    //[ValidateAntiForgeryToken]
    public void Create(CreateAdvertisementViewModel model) {
      if (ModelState.IsValid) {
        Advertisement advertisement = Advertisements.Create(Security.UserId, model.Text, model.Title, Convert.FromBase64String(model.Image));
        dynamic rez = new ExpandoObject();
        rez.Id = advertisement.Id;
        rez.Title = advertisement.Title;
        rez.Text = advertisement.Text;
        rez.Author = advertisement.Author.Profile.Name;
        rez.Image = Convert.ToBase64String(advertisement.Image);
        rez.DateCreated = advertisement.DateCreated;
        var result = new List<dynamic> {
          rez
        };
        ResponseData.WriteList(Response, "result", result);
      }
    }
  }
}