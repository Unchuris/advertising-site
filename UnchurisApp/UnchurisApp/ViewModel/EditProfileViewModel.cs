using System.ComponentModel.DataAnnotations;

namespace UnchurisApp.ViewModel {

  public class EditProfileViewModel {
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please enter your name")]
    public string Name { get; set; }

    [Phone(ErrorMessage = "Please enter a valid Phone")]
    public string Phone { get; set; }

    [MaxLength(140, ErrorMessage = "Bio can only be {0} characters.")]
    public string Bio { get; set; }
  }
}