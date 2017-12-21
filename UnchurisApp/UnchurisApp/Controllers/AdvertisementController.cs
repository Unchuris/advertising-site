using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnchurisApp.Models;
using UnchurisApp.ViewModel;

namespace UnchurisApp.Controllers {
  public class AdvertisementController : AdvertisementControllerBase {

    [Route("advertisement/{id?}")]
    public void Index(string id) {
      int ID = 0;
      bool isNum = int.TryParse(id, out ID);
      if (!Security.IsAuthenticated) {
        ResponseData.WriteList(Response, "result", new List<dynamic> {});
        return;
      }
      var advertisement = AdvertisementSearch.GetByIndexRecords("AuthorId", Security.UserId, "Id", ID).ToArray()[0];
      if (advertisement == null) {
        ResponseData.WriteList(Response, "result", new List<dynamic> { });
        return;
      }
      dynamic rez = new ExpandoObject();
      rez.Id = ID;
      rez.Title = advertisement.Title;
      rez.Text = advertisement.Text;
      rez.Author = advertisement.Author.Username;
      rez.Image = Convert.ToBase64String(advertisement.Image);
      rez.DateCreated = advertisement.DateCreated;
      var result = new List<dynamic> {
          rez
        };
      ResponseData.WriteList(Response, "result", result);
    }

    [HttpPost]
    public void Edit(EditAdvertisement model) {
      if (!Security.IsAuthenticated || !ModelState.IsValid) {
        ResponseData.WriteList(Response, "result", new List<dynamic> { });
        return;
      }
      Advertisement ad = AdvertisementSearch.GetByIndexRecords("AuthorId", Security.UserId, "Id", model.Id).ToArray()[0];
      Advertisement newAd = new Advertisement {
        Id = model.Id,
        Image = Convert.FromBase64String(model.Image),
        Title = model.Title,
        Text = model.Text,
        DateCreated = ad.DateCreated,
        AuthorId = ad.AuthorId
      };
      Advertisement advertisement = Advertisements.Update(newAd);
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
      AdvertisementSearch.AddUpdateLuceneIndex(advertisement);
      ResponseData.WriteList(Response, "result", result);
    }

  }
}