using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UnchurisApp
{
    public class RouteConfig
    {
    public static void RegisterRoutes(RouteCollection routes) {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute(
          name: "signUp",
          url: "api/signUp",
          defaults: new { controller = "account", action = "signUp" }
      );

      routes.MapRoute(
          name: "signIn",
          url: "api/signIn",
          defaults: new { controller = "account", action = "login" }
      );

      routes.MapRoute(
          name: "isAuthenticated",
          url: "api/isAuthenticated",
          defaults: new { controller = "account", action = "authenticated" }
      );

      routes.MapRoute(
          name: "logout",
          url: "api/logout",
          defaults: new { controller = "account", action = "logout" }
      );

      routes.MapRoute(
          name: "edit",
          url: "api/edit",
          defaults: new { controller = "advertisement", action = "edit" }
      );

      routes.MapRoute(
         name: "profileEdit",
         url: "api/profile/edit",
         defaults: new { controller = "profile", action = "Edit" }
     );

     routes.MapRoute(
         name: "profile",
         url: "api/profile",
         defaults: new { controller = "profile", action = "index" }
     );

     routes.MapRoute(
        name: "profiles",
        url: "api/profiles",
        defaults: new { controller = "home", action = "profiles" }
     );

      routes.MapRoute(
          name: "create",
          url: "api/ad/create",
          defaults: new { controller = "home", action = "create" }
      );

      routes.MapRoute(
        name: "AdvertisementApi",
        url: "api/adv",
        defaults: new { controller = "home", action = "advertisementsAllUsers" }
      );

      routes.MapRoute(
          name: "Advertisement",
          url: "api/adv/{id}",
          defaults: new { controller = "advertisement", action = "Index", id = "a" }
      );

      routes.MapRoute(
          name: "Default",
          url: "",
          defaults: new { controller = "home", action = "index" }
      );

      routes.MapRoute(
        "NotFound",
        "{*url}",
         new { controller = "home", action = "index" }
      );

    }
  }
}
