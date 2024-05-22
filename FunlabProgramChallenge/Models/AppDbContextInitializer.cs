using FunlabProgramChallenge.Core.Identity;
using FunlabProgramChallenge.Model;
using FunlabProgramChallenge.Utility;
using FunlabProgramChallenge.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace FunlabProgramChallenge.Models
{
    public class AppDbContextInitializer
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {
            try
            {
                using (var appContext = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
                {
                    var canConnect = appContext.GetService<IDatabaseCreator>().CanConnect();
                    //var canConnect = appContext.Database.CanConnect();
                    if (canConnect)
                    {
                        var ensureCreated = appContext.GetService<IDatabaseCreator>().EnsureCreated();
                        //var ensureCreated = appContext.Database.EnsureCreated();

                        if (ensureCreated)
                        {
                            AppSeedData(appContext);
                        }
                    }
                }

                using (var appIdentityContext = new AppIdentityDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppIdentityDbContext>>()))
                {
                    var canConnect = appIdentityContext.GetService<IDatabaseCreator>().CanConnect();
                    //var canConnect = appIdentityContext.Database.CanConnect();
                    if (canConnect)
                    {
                        var ensureCreated = appIdentityContext.GetService<IDatabaseCreator>().EnsureCreated();
                        //var ensureCreated = appIdentityContext.Database.EnsureCreated();

                        if (ensureCreated)
                        {
                            AppIdentitySeedData(appIdentityContext);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }

            
        }

        public static void AppIdentitySeedData(AppIdentityDbContext context)
        {
            var applicationRoleList = new List<ApplicationRole>() 
            {
                new ApplicationRole() { Id = AppConstants.AppRole.Admin.ToString(), Name = AppConstants.AppRole.Admin.ToString(), NormalizedName = AppConstants.AppRole.Admin.ToString().ToUpper(), ConcurrencyStamp = "2a5e2652-6afe-4bcc-ad00-3e41bb381db3" },
                new ApplicationRole() { Id = AppConstants.AppRole.Member.ToString(), Name = AppConstants.AppRole.Member.ToString(), NormalizedName = AppConstants.AppRole.Member.ToString().ToUpper(), ConcurrencyStamp = "37f3a020-7e95-4874-b08f-6c0cd5928663" },
            };

            foreach (var applicationRole in applicationRoleList)
            {
                //if (!context.Roles.Any(r => r.Id == applicationRole.Id))
                if (true)
                {
                    context.Roles.Add(applicationRole);
                }
            }

            var applicationUserList = new List<ApplicationUser>()
            {
                //Qwer!234
                new ApplicationUser() { Id = AppConstants.AppUser.Admin.ToString()
                , AccessFailedCount = 0, ConcurrencyStamp = "ef44ca0b-de20-4da1-9146-0f3fdf9c5a63", 
                    Email = "admin@mail.com", EmailConfirmed = false, LockoutEnabled = false, NormalizedEmail = "admin@mail.com", NormalizedUserName = "admin@mail.com", PasswordHash = "$2y$10$NdLhvA1rZW92gj1gPsY/gezaZJVZ.YuWN02dQcmfa/59J1JexPa4m", PhoneNumberConfirmed = false, SecurityStamp = "ea19af2a-d7e7-42a0-8b56-7adf13cc4374", TwoFactorEnabled = false, UserName = "admin@mail.com" },
                new ApplicationUser() { Id = AppConstants.AppUser.Member.ToString()
                , AccessFailedCount = 0, ConcurrencyStamp = "3d24e5b9-1924-4747-9365-946fa617d567", 
                    Email = "member@mail.com", EmailConfirmed = false, LockoutEnabled = false, NormalizedEmail = "member@mail.com", NormalizedUserName = "member@mail.com", PasswordHash = "$2y$10$NdLhvA1rZW92gj1gPsY/gezaZJVZ.YuWN02dQcmfa/59J1JexPa4m", PhoneNumberConfirmed = false, SecurityStamp = "3de7b7c2-08e0-412e-80ac-56f6de01f459", TwoFactorEnabled = false, UserName = "member@mail.com" },
            };

            foreach (var applicationUser in applicationUserList)
            {
                //if (!context.Users.Any(r => r.Id == applicationUser.Id))
                if (true)
                {
                    context.Users.Add(applicationUser);
                }
            }

            context.SaveChanges();
        }

        public static void AppSeedData(AppDbContext context)
        {
            // Create an instance and save the entity to the database
            var member = new Member() { 
                //MemberId = 1,
                UserId = AppConstants.AppUser.Member.ToString(),
                FirstName = "Rasel", 
                LastName = "Ahmmed", 
                EmailAddress = "raselahmmed@mail.com", 
                PhoneNumber = "01911-045573",
                PresentAddress = "Dhaka",
                PermanentAddress = "Dhaka",
                CardName = "Rasel Ahmmed",
                CardNumber = "007",
                CardExpirationYear = "26",
                CardExpirationMonth = "12",
                CardCvc = "1234"
            };

            context.Member.Add(member);
            context.SaveChanges();
        }
    }
}
