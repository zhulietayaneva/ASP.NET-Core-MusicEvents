using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicEvents.Data;
using MusicEvents.Infrastructure;
using MusicEvents.Services;
using MusicEvents.Services.Artists;
using MusicEvents.Services.Cities;
using MusicEvents.Services.Countries;
using MusicEvents.Services.Events;
using MusicEvents.Services.Organizers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services
                .AddDbContext<MusicEventsDbContext>(options => options.UseSqlServer(connectionString));

builder.Services
                .AddDatabaseDeveloperPageExceptionFilter();

builder.Services
                .AddDefaultIdentity<IdentityUser>(options => { options.Password.RequireDigit = false; options.Password.RequireNonAlphanumeric = false; options.Password.RequireUppercase=false; options.Password.RequireLowercase = false; })
                .AddEntityFrameworkStores<MusicEventsDbContext>();

builder.Services
                .AddControllersWithViews();

builder.Services.AddTransient<IEventService, EventService>();
builder.Services.AddTransient<IArtistService, ArtistService>();
builder.Services.AddTransient<ICountryService, CountryService>();
builder.Services.AddTransient<ICityService, CityService>();
builder.Services.AddTransient<IOrganizerService, OrganizerService>();

builder.Services.AddMemoryCache();

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


app.PrepareDatabase();
app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
