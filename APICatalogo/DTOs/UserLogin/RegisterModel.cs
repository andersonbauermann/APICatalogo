using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs.UserLogin;

public class RegisterModel : LoginModel
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }
}