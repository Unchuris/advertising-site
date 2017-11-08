using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnchurisApp.Models;
using UnchurisApp.ViewModel;

namespace UnchurisApp.Controllers {
  public class AdvertisementController : AdvertisementControllerBase {

    [Route("advertisement/{id?}")]
    public ActionResult Index(string id) {
      int ID = 0;
      bool isNum = int.TryParse(id, out ID);
      if (!Security.IsAuthenticated) {
        return View("Landing", new LoginSignupViewModel());
      }
      var advertisement = Advertisements.GetItemByID(Security.UserId, ID);

      if (advertisement == null) {
        return View("Landing", new LoginSignupViewModel());
      }
      return View(new Advertisement() {
        Id = ID,
        AuthorId = advertisement.AuthorId,
        Author = advertisement.Author,
        DateCreated = advertisement.DateCreated,
        Text = advertisement.Text,
        Title = advertisement.Title,
        Image = advertisement.Image
      });
    }

    [HttpPost]
    public ActionResult Edit(Advertisement model, HttpPostedFileBase uploadImage) {
      if (!Security.IsAuthenticated) {
        return RedirectToAction("Index", "Home");
      }
      if (!ModelState.IsValid) {
        return View("Index", model);
      }
      if (uploadImage != null) {
        byte[] imageData = null;

        using (var binaryReader = new BinaryReader(uploadImage.InputStream)) {
          imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
        }

        model.Image = imageData;
      }

      Advertisements.Update(model);

      return RedirectToAction("Index", new { id = model.Id });

      throw new NotImplementedException();
    }

  }
}