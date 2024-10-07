var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
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

app.UseAuthorization();

string routePrefix = app.Environment.IsProduction() ? "cis4396-f05" : "";
app.MapControllerRoute(
  name: "default",
pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();

