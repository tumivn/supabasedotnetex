using hellosupa.Dtos;
using hellosupa.Services;
using Microsoft.AspNetCore.Mvc;

namespace hellosupa.Controllers;

public class AuthenticationController(AuthService authService) : Controller
{
    private AuthService _authService = authService;

    [HttpGet]
    public Task<ViewResult> SignIn()
    {
        return Task.FromResult(View());
    }   
   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(SignInDto signInDto)
    {
        if (!ModelState.IsValid)
        {
            return View(signInDto);
        }

        try
        {
            await _authService.SignInAsync(signInDto.Email, signInDto.Password);
            return RedirectToAction("Index", "Home");    
        }catch (Exception e)
        {
            Console.Error.WriteLine(e);
            ModelState.AddModelError(string.Empty, "Invalid email or password");
            return View(signInDto);
        }
    }
    
    public  async Task<IActionResult> LogOut()
    {
        await _authService.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public async Task<IActionResult> SignUp(){
        if (_authService.IsAuthenticated())
        {
            TempData["ErrorMessage"] = "You are already logged in. Please log out to sign up a new account.";
            return RedirectToAction("Index", "Home");
        }
        
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(SignUpDto signUpDto)
    {
        
        
        if (!ModelState.IsValid)
        {
            return View(signUpDto);
        }

        try
        {
            await _authService.SignUpAsync(signUpDto.Email,  signUpDto.Username, signUpDto.Password);
            return RedirectToAction("ConfirmEmail");
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            ModelState.AddModelError(string.Empty, "Invalid email or password");
            return View(signUpDto);
        }
        
    }
    
    [HttpGet]
    public Task<ViewResult> ConfirmEmail()
    {
        return Task.FromResult(View());
    }
    
    [HttpGet]
    public async Task<ViewResult> Confirmed()
    {
        var queryParams = base.Request.Query;
        if(queryParams.ContainsKey("email") && queryParams.ContainsKey("token"))
        {
            var email = queryParams["email"];
            var token = queryParams["token"];
            await _authService.SignInWithTokensAsync(email, token);
        }
        return await Task.FromResult(View());
    }
    
    
}