
using ServiceDesk_WebApp.Common;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("MailSettings"));
// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<ServiceDesk_WebAppContext>(opt => opt.UseSqlite("Name=ServiceDeskDB"));
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped(typeof(IRepository), typeof(Repository));
builder.Services.AddSingleton(typeof(Utils));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Home/Index";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.Cookie.Name = "ServiceDesk_WebApp";
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddNotyf(config => { config.DurationInSeconds = 5; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

var app = builder.Build();

app.Use(async (ctx, next) =>
{
    await next();
    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
    {
        //Re-execute the request so the user gets the error page
        var originalPath = ctx.Request.Path.Value;
        ctx.Items["originalPath"] = originalPath;
        ctx.Request.Path = "/Error/PageNotFound";
        ctx.Response.Redirect(ctx.Request.Path);
        await next();
    }
});


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error/Exception");
   // app.UseStatusCodePagesWithRedirects("/404");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
