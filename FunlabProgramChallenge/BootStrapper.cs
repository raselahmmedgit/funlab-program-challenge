﻿using FunlabProgramChallenge.Core.Identity;
using FunlabProgramChallenge.JwtGenerator;
using FunlabProgramChallenge.Managers;
using FunlabProgramChallenge.Model;
using FunlabProgramChallenge.Models;
using FunlabProgramChallenge.Repositories;
using FunlabProgramChallenge.Utility;
using FunlabProgramChallenge.Web.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FunlabProgramChallenge
{
    public class BootStrapper
    {
        public static void Run(WebApplicationBuilder builder)
        {
            try
            {
                // Add services to the container.
                builder.Services.AddControllersWithViews();

                //Add our Config object so it can be injected
                builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(ConnectionStrings.Name));
                builder.Services.Configure<SmsConfig>(builder.Configuration.GetSection(SmsConfig.Name));
                builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection(EmailConfig.Name));
                builder.Services.Configure<AppConfig>(builder.Configuration.GetSection(AppConfig.Name));
                builder.Services.Configure<StripePaymentGatewayConfig>(builder.Configuration.GetSection(StripePaymentGatewayConfig.Name));

                builder.Services.AddScoped<IMemberRepository, MemberRepository>();
                builder.Services.AddScoped<IMemberManager, MemberManager>();

                builder.Services.AddScoped<IEmailSenderManager, EmailSenderManager>();
                builder.Services.AddScoped<IStripePaymentGatewayManager, StripePaymentGatewayManager>();

                builder.Services.AddScoped<ITokenManager, TokenManager>();

                builder.Services.AddRouting(options => options.LowercaseUrls = true);

                // Add functionality to inject IOptions<T>
                builder.Services.AddOptions();
                builder.Services.AddCors();

                AppConstants.WebRootPath = builder.Environment.WebRootPath;
                AppConstants.ContentRootPath = builder.Environment.ContentRootPath;

                string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

                #region Identity
                builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseSqlServer(connectionString));

                builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();

                builder.Services.Configure<IdentityOptions>(options =>
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

                builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();

                //builder.Services.ConfigureApplicationCookie(options =>
                //{
                //    // Cookie settings
                //    options.Cookie.HttpOnly = true;
                //    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                //    options.LogoutPath = "/Account/Logout";
                //    options.LoginPath = "/Account/Login";
                //    options.AccessDeniedPath = "/Account/AccessDenied";
                //    options.SlidingExpiration = true;
                //});

                #endregion

                #region Database
                builder.Services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer(connectionString));
                #endregion

                #region MemoryCache
                //builder.Services.AddDistributedMemoryCache();
                //builder.Services.AddMemoryCache();
                #endregion

                //builder.Services.AddMvc(
                //   options => options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
                //);
                //call this in case you need aspnet-user-authtype/aspnet-user-identity
                builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                #region JwtToken

                JwtTokenOptions jwtTokenOptions = new JwtTokenOptions();
                var jwtTokenSectiion = builder.Configuration.GetSection(JwtTokenOptions.Name);
                jwtTokenSectiion.Bind(jwtTokenOptions);
                builder.Services.Configure<JwtTokenOptions>(jwtTokenSectiion);

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,

                        ValidAudience = builder.Configuration["JwtToken:ValidAudience"],
                        ValidIssuer = builder.Configuration["JwtToken:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtToken:Secret"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies[CookieAuthenticationDefaults.AuthenticationScheme];
                            context.HttpContext.User = context.Principal;
                            return Task.CompletedTask;
                        },
                    };
                }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

                #endregion

                builder.Services.RegisterAutoMapper();

            }
            catch (Exception)
            {
                throw;
            }

        }

        public static void RunSeedData(WebApplication app)
        {
            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    // Initializes and seeds the database.
                    AppDbContextInitializer.SeedData(services);
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
