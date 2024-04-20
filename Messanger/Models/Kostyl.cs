using System.ComponentModel.DataAnnotations;

namespace Messanger.Models;

public class RegisterModel
{
    [Required]
    [StringLength(15, MinimumLength = 3)]
    public string name { get; set; }
    
    [Required]
    [EmailAddress]
    public string email { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string password { get; set; }
}

public class LoginModel
{
    [Required]
    [EmailAddress]
    public string email { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string password { get; set; }
}

public class RegAndLogModel
{
    public RegisterModel register { get; set; }
    public LoginModel login { get; set; }
}
