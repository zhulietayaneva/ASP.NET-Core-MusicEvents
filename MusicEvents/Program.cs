using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicEvents.Data;
using MusicEvents.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services
                .AddDbContext<MusicEventsDbContext>(options => options.UseSqlServer(connectionString));

builder.Services
                .AddDatabaseDeveloperPageExceptionFilter();

builder.Services
                .AddDefaultIdentity<IdentityUser>(options => options.Password.RequireDigit=false)
                .AddEntityFrameworkStores<MusicEventsDbContext>();

builder.Services
                .AddControllersWithViews();

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