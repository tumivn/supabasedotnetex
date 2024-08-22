using System.Text;
using hellosupa.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
 // Add services to the container.
builder.Services.AddAuthentication(a =>
{
    a.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    a.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/authentication/signin";
    options.LogoutPath = "/authentication/signout";
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<Client>(sp => 
    new Client(builder.Configuration["Supabase:BaseUrl"], 
        builder.Configuration["Supabase:SecretApiKey"]));
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpMethodOverride(new HttpMethodOverrideOptions(){FormFieldName = "X-Http-Method-Override"});
app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();