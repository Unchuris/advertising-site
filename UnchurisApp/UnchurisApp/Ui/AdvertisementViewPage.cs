using UnchurisApp.Data;
using UnchurisApp.Models;
using UnchurisApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UnchurisApp.Ui {
  public abstract class AdvertisementViewPage : WebViewPage {
    protected IContext DataContext;
    public User CurrentUser { get; private set; }
    public IUserService Users { get; private set; }
    public ISecurityService Security { get; private set; }

    public AdvertisementViewPage() {
      DataContext = new Context();
      Users = new UserService(DataContext);
      Security = new SecurityService(Users);
      CurrentUser = Security.GetCurrentUser();
    }
  }

  public abstract class AdvertisementViewPage<TModel> : WebViewPage<TModel> {
    protected IContext DataContext;
    public User CurrentUser { get; private set; }
    public IUserService Users { get; private set; }
    public ISecurityService Security { get; private set; }

    public AdvertisementViewPage() {
      DataContext = new Context();
      Users = new UserService(DataContext);
      Security = new SecurityService(Users);
      CurrentUser = Security.GetCurrentUser();
    }
  }
}