using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcCoreDemo.Configs;
using MvcCoreDemo.Controllers.Utility;
using MvcCoreDemo.Data;
using MvcCoreDemo.Data.DataAccess;
using MvcCoreDemo.Data.Interfacce;
using NLog;
using NLog.Web;



var builder = WebApplication.CreateBuilder(args);

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    builder.Services.AddDbContext<BlogDbContext>(o => o.UseSqlServer(
        builder.Configuration.GetConnectionString("BlogDbConnectionString")));

    //DI DB di autenticazione e binding DB - Ruoli/Utenti 
    builder.Services.AddDbContext<AuthDbContext>(o => o.UseSqlServer(
        builder.Configuration.GetConnectionString("BlogAuthDbConnectionString")));

    builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();

    //Identity Options per configurare la validazione degli utenti in fase di registrazione 
    builder.Services.Configure<IdentityOptions>(o =>
    {
        o.Password.RequireDigit = true;
        o.Password.RequireLowercase = true;
        o.Password.RequireNonAlphanumeric = true;
        o.Password.RequireUppercase = true;
        o.Password.RequiredUniqueChars = 1;
        o.Password.RequiredLength = 6;
    });


    //Servi Tag DAL per interazione con il DB 
    builder.Services.AddScoped<ITagDal, TagDal>();
    builder.Services.AddScoped<IBlogPostDal, BlogPostDal>();
    builder.Services.AddScoped<IImgDal, LocalImageDal>();
    builder.Services.AddScoped<IUserDal, UserDal>();

    //automapper
    builder.Services.AddAutoMapper(typeof(AutoMapperConfigs));

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    //logging utility class
    builder.Services.AddScoped<IUtilityLogger, UserLogger>();

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

    //Pipeline ASP.NET MVC per applicare il processo di autenticazione ed access controll
    app.UseAuthentication();
    app.UseAuthorization();


    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}

