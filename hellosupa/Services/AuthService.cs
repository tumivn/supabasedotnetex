using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Supabase.Gotrue;
using Client = Supabase.Client;

namespace hellosupa.Services;

public class AuthService(Client client, IHttpContextAccessor accessor, AuthenticationStateProvider authStateProvider)
{
    public async Task SignInAsync(string email, string password)
    {
        await client.Auth.SignIn(email, password);
        var authState = await authStateProvider.GetAuthenticationStateAsync();
        await accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authState.User);
    }
    
    public async Task SignInWithTokensAsync(string accessToken, string refreshToken)
    {
        await client.Auth.SetSession(accessToken, refreshToken);
        var authState = await authStateProvider.GetAuthenticationStateAsync();
        await accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authState.User);
    }
    
    public async Task SignOutAsync()
    {
        await client.Auth.SignOut();
        await authStateProvider.GetAuthenticationStateAsync();
        await accessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
    public async Task SignUpAsync(string email, string username, string password)
    {
        var session = await client.Auth.SignUp(email, password, 
            new SignUpOptions(){Data = new Dictionary<string, object> {{"username", username}}});
        var authState = await authStateProvider.GetAuthenticationStateAsync();
    }
    
    public bool IsAuthenticated()
    {
        var user = accessor.HttpContext?.User;
        return user?.Identity is { IsAuthenticated: true };
    }
}