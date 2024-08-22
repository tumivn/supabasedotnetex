using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Supabase;

namespace hellosupa.Services;

public class CustomAuthStateProvider(Client client) : AuthenticationStateProvider
{
    private  byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                return Convert.FromBase64String(base64 + "==");
            case 3:
                return Convert.FromBase64String(base64 + "=");
            default:
                return Convert.FromBase64String(base64);
        }
    }
    
    IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var kvp = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return kvp.Select(kv => new Claim(kv.Key, kv.Value.ToString()));
    }   


    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await client.InitializeAsync();
        var indentity = client.Auth.CurrentSession is { AccessToken: not null }
            ? new ClaimsIdentity(ParseClaimsFromJwt(client.Auth.CurrentSession.AccessToken), "jwt")
            : new ClaimsIdentity();
        var user = new ClaimsPrincipal(indentity);
        var state = new AuthenticationState(user);
        return state;
    }
}