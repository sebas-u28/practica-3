using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using practica.Data;
using practica.Integration;
using practica.Integration.Exchange;
using practica.Service;

var builder = WebApplication.CreateBuilder(args);

// Obtén la URL base desde configuración
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient(); 

builder.Services.AddScoped<PostIntegration>();
builder.Services.AddScoped<FeedbackIntegration>();
builder.Services.AddScoped<FeedbackService>();

// Configura HttpClient para FeedbackIntegration usando la URL desde appsettings.json
builder.Services.AddHttpClient<FeedbackIntegration>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
