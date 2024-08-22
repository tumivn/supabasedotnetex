using System.ComponentModel.DataAnnotations;

namespace hellosupa.Dtos;

public class SignInDto
{
    [EmailAddress]
    public string Email { get; set; }
    
    public string Password { get; set; }
}