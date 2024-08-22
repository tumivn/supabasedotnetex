using System.ComponentModel.DataAnnotations;

namespace hellosupa.Dtos;

public class SignUpDto
{
    [EmailAddress]
    public string Email { get; set; }
    
    [Length(5, 20)]
    public string Username { get; set; }
    
    public string Password { get; set; }
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}