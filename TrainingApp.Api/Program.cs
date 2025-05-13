using TrainingApp.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Poprawna kolejnoœæ middleware
app.UseAuthentication();  // Autentykacja powinna byæ przed autoryzacj¹
app.UseAuthorization();   // Autoryzacja powinna byæ po autentykacji

app.UseRouting();         // Routing powinien byæ po autoryzacji i autentykacji

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages()
   .WithStaticAssets();

app.Run();