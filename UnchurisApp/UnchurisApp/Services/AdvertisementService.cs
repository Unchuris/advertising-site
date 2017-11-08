using UnchurisApp.Data;
using UnchurisApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;

namespace UnchurisApp.Services {
  public class AdvertisementService : IAdvertisementService {
    private readonly IContext _context;
    private readonly IAdvertisementRepository _advertisements;

    public AdvertisementService(IContext context) {
      _context = context;
      _advertisements = context.Advertisements;
    }

    public Advertisement GetBy(int id) {
      return _advertisements.GetBy(id);
    }

    public Advertisement Create(User user, string text, string title, byte[] image, DateTime? created = null) {
      return Create(user.Id, text, title, image, created);
    }

    public Advertisement Create(int userId, string text, string title, byte[] image, DateTime? created = null) {

      var advertisement = new Advertisement() {
        AuthorId = userId,
        Text = text,
        Title = title,
        Image = image,
        DateCreated = created ?? DateTime.Now

      };

      _advertisements.Create(advertisement);

      _context.SaveChanges();

      return advertisement;
    }

    public IEnumerable<Advertisement> All(bool includeAdvertisement) {
      return _advertisements.All(includeAdvertisement)
          .OrderByDescending(r => r.DateCreated);
    }

    public void Update(Advertisement model) {

      var advetisement = new Advertisement() {
        Id = model.Id,
        AuthorId = model.AuthorId,
        Author = model.Author,
        DateCreated = model.DateCreated,
        Text = model.Text,
        Title = model.Title,
        Image = model.Image
      };

      _advertisements.Update(advetisement);

      _context.SaveChanges();
    }

    public IEnumerable<Advertisement> GetTimelineFor(int userId) {
      return _advertisements.FindAll(r => r.AuthorId == userId)
          .OrderByDescending(r => r.DateCreated);
    }

    public Advertisement GetItemByID(int userId, int itemId) {
      return _advertisements.Find(r => (r.AuthorId == userId) && (r.Id == itemId));
    }
  }
}