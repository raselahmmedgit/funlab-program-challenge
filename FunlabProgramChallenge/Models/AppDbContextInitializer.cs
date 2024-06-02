using FunlabProgramChallenge.Core.Identity;
using FunlabProgramChallenge.Model;
using FunlabProgramChallenge.Utility;
using FunlabProgramChallenge.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System;

namespace FunlabProgramChallenge.Models
{
    public class AppDbContextInitializer
    {
        #region Global Variable Declaration
        private readonly ILogger<AppDbContextInitializer> _iLogger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _appClaimsPrincipalFactory;
        private readonly AppIdentityDbContext _appIdentityDbContext;
        private readonly AppDbContext _appDbContext;
        private readonly AppDbConfig _appDbConfig;
        #endregion

        #region Constructor
        public AppDbContextInitializer(ILogger<AppDbContextInitializer> iLogger, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IUserClaimsPrincipalFactory<ApplicationUser> appClaimsPrincipalFactory,
            AppIdentityDbContext appIdentityDbContext,
            AppDbContext appDbContext,
            IOptions<AppDbConfig> appDbConfig)
        {
            _iLogger = iLogger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _appClaimsPrincipalFactory = appClaimsPrincipalFactory;
            _appIdentityDbContext = appIdentityDbContext;
            _appDbContext = appDbContext;
            _appDbConfig = appDbConfig.Value;
        }
        #endregion

        public async Task CreateDatabaseAndSeedDataAsync()
        {
            try
            {
                var canConnect = _appIdentityDbContext.Database.CanConnect();
                if (canConnect)
                {
                    var appIdentityEnsureCreated = _appIdentityDbContext.Database.EnsureCreated();
                    if (appIdentityEnsureCreated)
                    {
                        if (!IsExistTable(_appDbContext))
                        {
                            var appDatabaseCreator = (RelationalDatabaseCreator)_appDbContext.Database.GetService<IDatabaseCreator>();
                            appDatabaseCreator.CreateTables();
                        }

                        await SeedDataAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SeedDataAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any() && !_userManager.Users.Any())
                {
                    var adminRole = new ApplicationRole() { Id = AppConstants.AppRole.Admin.ToString(), Name = AppConstants.AppRole.Admin.ToString(), NormalizedName = AppConstants.AppRole.Admin.ToString().ToUpper(), ConcurrencyStamp = "2a5e2652-6afe-4bcc-ad00-3e41bb381db3" };
                    await _roleManager.CreateAsync(adminRole);

                    var memberRole = new ApplicationRole() { Id = AppConstants.AppRole.Member.ToString(), Name = AppConstants.AppRole.Member.ToString(), NormalizedName = AppConstants.AppRole.Member.ToString().ToUpper(), ConcurrencyStamp = "37f3a020-7e95-4874-b08f-6c0cd5928663" };
                    await _roleManager.CreateAsync(memberRole);


                    var adminUser = new ApplicationUser()
                    {
                        Id = AppConstants.AppUser.Admin.ToString(),
                        AccessFailedCount = 0,
                        ConcurrencyStamp = "ef44ca0b-de20-4da1-9146-0f3fdf9c5a63",
                        Email = "admin@mail.com",
                        EmailConfirmed = false,
                        LockoutEnabled = false,
                        NormalizedEmail = "admin@mail.com",
                        NormalizedUserName = "admin@mail.com",
                        //PasswordHash = "$2y$10$NdLhvA1rZW92gj1gPsY/gezaZJVZ.YuWN02dQcmfa/59J1JexPa4m",
                        PhoneNumberConfirmed = false,
                        SecurityStamp = "ea19af2a-d7e7-42a0-8b56-7adf13cc4374",
                        TwoFactorEnabled = false,
                        UserName = "admin@mail.com"
                    };
                    string adminUserPasswordHash = _passwordHasher.HashPassword(adminUser, "Qwer!234");
                    adminUser.PasswordHash = adminUserPasswordHash;

                    await _userManager.CreateAsync(adminUser);
                    await _userManager.AddToRolesAsync(adminUser, new[] { adminRole.Name, memberRole.Name });

                    var adminUserClaims = await _appClaimsPrincipalFactory.CreateAsync(adminUser);
                    await _userManager.AddClaimsAsync(adminUser, adminUserClaims.Claims);

                    var memberUser = new ApplicationUser()
                    {
                        Id = AppConstants.AppUser.Member.ToString(),
                        AccessFailedCount = 0,
                        ConcurrencyStamp = "3d24e5b9-1924-4747-9365-946fa617d567",
                        Email = "member@mail.com",
                        EmailConfirmed = false,
                        LockoutEnabled = false,
                        NormalizedEmail = "member@mail.com",
                        NormalizedUserName = "member@mail.com",
                        //PasswordHash = "$2y$10$NdLhvA1rZW92gj1gPsY/gezaZJVZ.YuWN02dQcmfa/59J1JexPa4m",
                        PhoneNumberConfirmed = false,
                        SecurityStamp = "3de7b7c2-08e0-412e-80ac-56f6de01f459",
                        TwoFactorEnabled = false,
                        UserName = "member@mail.com"
                    };
                    string memberUserPasswordHash = _passwordHasher.HashPassword(memberUser, "Qwer!234");
                    memberUser.PasswordHash = memberUserPasswordHash;

                    await _userManager.CreateAsync(memberUser);
                    await _userManager.AddToRoleAsync(memberUser, memberRole.Name);

                    var memberUserClaims = await _appClaimsPrincipalFactory.CreateAsync(adminUser);
                    await _userManager.AddClaimsAsync(memberUser, memberUserClaims.Claims);

                }

                if (!_appDbContext.Member.Any())
                {
                    // Create an instance and save the entity to the database
                    var member = new Member()
                    {
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
                        CardCvc = "1234",
                        CardCountry = "usd"
                    };

                    _appDbContext.Member.Add(member);
                    _appDbContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }


        }

        private bool IsExistTable(AppDbContext appDbContext)
        {
            try
            {
                appDbContext.Member.ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
