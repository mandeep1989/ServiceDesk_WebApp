using Microsoft.EntityFrameworkCore;
using ServiceDesk_WebApp.Data;
using ServiceDesk_WebApp.Services;
using ServiceDesk_WebApp.Services.Interface;
using ServiceDesk_WebApp.RepositoryLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ServiceDesk_WebAppContext>(opt => opt.UseSqlite("Name=ServiceDeskDB"));
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
builder.Services.AddScoped(typeof(IRepository), typeof(Repository));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
