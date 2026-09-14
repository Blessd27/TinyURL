using TinyURL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MVC services
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Register the in-memory URL shortener as a singleton so the
// dictionary persists for the lifetime of the running application.
builder.Services.AddSingleton<IUrlShortenerService, UrlShortenerService>();

var app = builder.Build();

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

// Redirect route: GET /{code} -> looks up the code in the in-memory
// dictionary and redirects to the original long URL.
// Constrained to exactly 6 alphanumeric characters (the generated code
// length) so it never collides with real MVC routes like /Home or /Privacy.
app.MapControllerRoute(
    name: "shortLink",
    pattern: "{code:length(6)}",
    defaults: new { controller = "Redirect", action = "Go" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
