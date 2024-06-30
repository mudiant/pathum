using Hatid;
using Hatid.Data;
using Hatid.Data.Models;
using Hatid.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuGet.DependencyResolver;

bool isFirstTimeRun = false;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("HatidConnectionString");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(15);
    options.SlidingExpiration = true;

    // moved here from AddCookie()
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/LogOut";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddRazorPages();

var emailSettings = builder.Configuration.GetSection("EmailSettings");
var defaultFromEmail = emailSettings["DefaultFromEmail"];
var host = emailSettings["SMTPSetting:Host"];
var port = emailSettings.GetValue<int>("SMTPSetting:Port");
var userName = emailSettings["SMTPSetting:UserName"];
var password = emailSettings["SMTPSetting:Password"];

builder.Services.AddFluentEmail(defaultFromEmail).AddSmtpSender(host, port, userName, password).AddRazorRenderer();

builder.Services.AddScoped<IEmailSender, EmailSender>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


if (isFirstTimeRun)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {

            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<Role>>();
            
            //create roles
            await roleManager.CreateAsync(new Role { Name = EnumRole.Admin.ToString() });
            await roleManager.CreateAsync(new Role { Name = EnumRole.Manager.ToString() });
            await roleManager.CreateAsync(new Role { Name = EnumRole.Sales.ToString() });

            //create default admin
            var defaultUser = new User
            {
                Email = "admin@hatid.com",
                UserName = "admin@hatid.com",
                PhoneNumber = "00000000000",
                EmailConfirmed = true,
                FirstName = "Hatid",
                LastName = "Admin",
                RoleId = (int)EnumRole.Admin
            };
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Admin@123");
                await userManager.AddToRoleAsync(defaultUser, EnumRole.Admin.ToString());
            }
        }
        catch (Exception ex)
        {

        }
    }
}

app.Run();

