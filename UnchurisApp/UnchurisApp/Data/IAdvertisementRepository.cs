using UnchurisApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnchurisApp.Data {

  public interface IAdvertisementRepository : IRepository<Advertisement> {
    IQueryable<Advertisement> All(bool includeProfile);
    Advertisement GetBy(int id);
    IEnumerable<Advertisement> GetFor(User user);
    void AddFor(Advertisement advertisement, User user);
  }
}
