using UnchurisApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UnchurisApp.Data {

  public class AdvertisementRepository : EfRepository<Advertisement>, IAdvertisementRepository {

    public AdvertisementRepository(DbContext context, bool sharedContext)
            : base(context, sharedContext) { }

    public Advertisement GetBy(int id) {
      return Find(r => r.Id == id);
    }

    public IEnumerable<Advertisement> GetFor(User user) {
      return user.Advertisements.OrderByDescending(r => r.DateCreated);
    }

    public void AddFor(Advertisement advertisement, User user) {
      user.Advertisements.Add(advertisement);

      if (!ShareContext) {
        Context.SaveChanges();
      }
    }

    public IQueryable<Advertisement> All(bool includeAdvertisement) {
      return includeAdvertisement ? DbSet.Include(u => u.Author).AsQueryable() : All();
    }
  }
}