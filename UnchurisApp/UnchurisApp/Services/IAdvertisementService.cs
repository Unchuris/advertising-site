using UnchurisApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnchurisApp.Services {
  public interface IAdvertisementService {
    Advertisement GetBy(int id);
    Advertisement Create(int userId, string text, string title, byte[] image, DateTime? created = null);
    Advertisement Create(User user, string text, string title, byte[] image, DateTime? created = null);
    IEnumerable<Advertisement> All(bool includeAdvertisement);
    IEnumerable<Advertisement> GetTimelineFor(int userId);
  }
}

