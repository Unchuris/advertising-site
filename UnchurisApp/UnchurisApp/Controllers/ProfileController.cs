﻿using UnchurisApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UnchurisApp.Models;
using System.Dynamic;
using UnchurisApp.Data.lucene;

namespace UnchurisApp.Controllers {

  public class ProfileController : AdvertisementControllerBase {

    public void Index() {
      if (!Security.IsAuthenticated) return;
      var profile = UserProfileSearch.GetByIndexRecords(CurrentUser.UserProfileId.ToString()).ToArray()[0];
      dynamic rez = new ExpandoObject();
      rez.Id = profile.Id;
      rez.Bio = profile.Bio;
      rez.Email = profile.Email;
      rez.Name = profile.Name;
      rez.Phone = profile.Phone;
      var result = new List<dynamic> {
          rez
        };
      ResponseData.WriteList(Response, "result", result);
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public void Edit(EditProfileViewModel model) {
      if (!Security.IsAuthenticated || !ModelState.IsValid) {
        ResponseData.WriteFalse(Response);
        return;
      } 
      var result = new List<UserProfile> {
          Profiles.Update(model)
      };
      UserProfileSearch.AddUpdateLuceneIndex(result);
      ResponseData.WriteTrue(Response);
    }
  }
}