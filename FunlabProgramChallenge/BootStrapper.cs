using FunlabProgramChallenge.Core.Identity;
using FunlabProgramChallenge.Model;
using FunlabProgramChallenge.Models;
using FunlabProgramChallenge.Utility;
using FunlabProgramChallenge.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FunlabProgramChallenge
{
    public class BootStrapper
    {
        public static void Run(IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                services.AddRouting(options => options.LowercaseUrls = true);

                // Add functionality to inject IOptions<T>
                services.AddOptions();

                services.AddCors();

                #region Identity
                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection")));

                services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();

                services.Configure<IdentityOptions>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = false;
                });

                services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();

                services.ConfigureApplicationCookie(options =>
                {
                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                    options.LogoutPath = "/Account/Logout";
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.SlidingExpiration = true;
                });

                #endregion

                #region Database
                services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
                #endregion

                #region MemoryCache
                //services.AddDistributedMemoryCache();
                //services.AddMemoryCache();
                #endregion

                //services.AddMvc(
                //   options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
                //);
                //call this in case you need aspnet-user-authtype/aspnet-user-identity
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                services.RegisterAutoMapper();

                // Initializes and seeds the database.
                InitializeAndSeedDbAsync(configuration);

            }
            catch (Exception)
            {
                throw;
            }

        }

        private static void InitializeAndSeedDbAsync(IConfiguration configuration)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var canConnect = context.Database.CanConnect();
                    if (!canConnect)
                    {
                        if (AppDbContextInitializer.CreateIfNotExists())
                        {
                            AppDbContextInitializer.SeedData();
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
