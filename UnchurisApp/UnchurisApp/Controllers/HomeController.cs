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
      if (Security.IsAuthenticated) {
        var advertisements = Advertisements.GetTimelineFor(Security.UserId).ToArray();
        var result = new List<dynamic>();
        dynamic rez = new ExpandoObject();
        foreach (var advertisement in advertisements) {
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
    }

    public void Profiles() {
      var users = Users.All(true);
      var result = new List<dynamic>();
      dynamic rez = new ExpandoObject();
      foreach (var user in users) {
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

    public void AdvertisementsAllUsers(string title, string text, string author) {
      Advertisement[] advertisements = null;
      advertisements = !String.IsNullOrEmpty(title) || !String.IsNullOrEmpty(text) || !String.IsNullOrEmpty(author) ?
        Advertisements.Search(s => s.Title.Contains(title) && s.Text.Contains(text) && s.Author.Profile.Name.Contains(author)).ToArray() :
        Advertisements.All(true).ToArray();
      var result = new List<dynamic>();
      dynamic rez = new ExpandoObject();
      foreach (var advertisement in advertisements) {
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
    [ChildActionOnly]
    public ActionResult Create() {
      return PartialView("_CreateAdvertisementPartial", new CreateAdvertisementViewModel());
    }

    [HttpPost]
    [ChildActionOnly]
    //[ValidateAntiForgeryToken]
    public void Create(CreateAdvertisementViewModel model, HttpPostedFileBase uploadImage) {
      if (ModelState.IsValid && uploadImage != null) {
        byte[] imageData = null;
        using (var binaryReader = new BinaryReader(uploadImage.InputStream)) {
          imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
        }
        Advertisement advertisement = Advertisements.Create(Security.UserId, model.Text, model.Title, imageData);
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