using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hellosupa.Controllers;

[Authorize]
public class MyProfileController: Controller
{
    public IActionResult Index()
    {
        var user = this.HttpContext.User;
        var email = user.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        var username = user.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

        ViewBag.Email = email; 
        ViewBag.Username = username;
        
        return View();
    }
}