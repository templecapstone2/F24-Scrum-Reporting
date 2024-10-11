using Capstone.Models;
using Capstone.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Register HttpClient
builder.Services.AddHttpClient();

// Register UserService
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DBConnect>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache(); // Use a distributed cache for production scenarios
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set timeout as needed
    options.Cookie.HttpOnly = true; // Make the cookie HTTP only
    options.Cookie.IsEssential = true; // Make the session cookie essential
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Always use HTTPS
});

// Configure LDAP settings
builder.Services.Configure<LDAPSettings>(builder.Configuration.GetSection("LDAPSettings"));

// Configure authentication
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "Login/Login"; // login path
//        options.AccessDeniedPath = "Login/Login"; 
//    });

var app = builder.Build();

// Set up the application path base
app.UsePathBase("/cis4396-f05");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication(); // If you're using authentication
app.UseAuthorization();

string routePrefix = app.Environment.IsProduction() ? "cis4396-f05" : "";
app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
