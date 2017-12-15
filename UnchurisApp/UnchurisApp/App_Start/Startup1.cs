using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using UnchurisApp.Controllers;
using UnchurisApp.Data;

[assembly: OwinStartup(typeof(UnchurisApp.App_Start.Startup1))]

namespace UnchurisApp.App_Start {
  public class Startup1 {
    public void Configuration(IAppBuilder app) {
      // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
      using (AdvertisementDatabase adDB = new AdvertisementDatabase()) {
        LuceneSearch.AddUpdateLuceneIndex(adDB.Advertisements.ToList());
      }
    }
  }
}
