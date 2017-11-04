using UnchurisApp.Models;
using UnchurisApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnchurisApp.Services {
  public interface ISecurityService {
    bool Authenticate(string username, string password);
    User CreateUser(SignupViewModel signupModel, bool login = true);
    bool DoesUserExist(string username);
    User GetCurrentUser();
    bool IsAuthenticated { get; }
    void Login(User user);
    void Login(string username);
    void Logout();
    int UserId { get; set; }
  }
}
