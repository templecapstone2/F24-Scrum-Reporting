using Capstone.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set timeout as needed
    options.Cookie.HttpOnly = true; // Make the cookie HTTP only
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

var app = builder.Build();
builder.Configuration.GetConnectionString("Connection_Database");
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

