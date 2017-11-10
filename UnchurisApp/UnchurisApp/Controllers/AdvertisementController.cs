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
        return;
      }
      var advertisement = Advertisements.GetItemByID(Security.UserId, ID);

      if (advertisement == null) {
        return;
      }
      dynamic rez = new ExpandoObject();
      rez.Id = ID;
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

    [HttpPost]
    public void Edit(Advertisement model, HttpPostedFileBase uploadImage) {
      if (!Security.IsAuthenticated) {
        return;
      }
      if (!ModelState.IsValid) {
        return;
      }
      if (uploadImage != null) {
        byte[] imageData = null;

        using (var binaryReader = new BinaryReader(uploadImage.InputStream)) {
          imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
        }

        model.Image = imageData;
      }

      var advertisement = Advertisements.Update(model);
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

      throw new NotImplementedException();
    }

  }
}