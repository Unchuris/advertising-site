using UnchurisApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using UnchurisApp.Models;
using System.Dynamic;
using UnchurisApp.Data;

namespace UnchurisApp.Controllers {

  public class HomeController : AdvertisementControllerBase {

    public HomeController() : base() { }

    public ActionResult Index() {
      return View();
    }

    public void UserAdvertisements() {
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

    public void AdvertisementsAllUsers(string text, string author) {
      AdvertisementDatabase adDB = new AdvertisementDatabase();
      LuceneSearch.AddUpdateLuceneIndex(adDB.Advertisements.ToList());
      Advertisement[] advertisements = null;
      int ID = 0;
      bool isNum = int.TryParse(author, out ID);
      advertisements = !String.IsNullOrEmpty(text) ?
       LuceneSearch.Search(text, "Text").ToArray().ToArray() :
       Advertisements.All(true).ToArray();
      if (isNum) {
        advertisements = 
          //LuceneSearch.Search(text, "Text").ToArray();
        Advertisements.Search(s => s.Text.Contains(text) && s.Id == ID).ToArray();
      }
      var result = new List<dynamic>();
      foreach (var advertisement in advertisements) {
        dynamic rez = new ExpandoObject();
        rez.Id = advertisement.Id;
        rez.Title = advertisement.Title;
        rez.Text = advertisement.Text;
        rez.Author = advertisement.Author.Username;
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