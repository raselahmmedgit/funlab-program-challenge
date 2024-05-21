using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlacovuCMS.Core.Helpers;
using PlacovuCMS.Core.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FunlabProgramChallenge.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        #region Global Variable Declaration
        private RoleManager<ApplicationRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ILog _log;
        #endregion

        #region Constructor
        public AdminController(RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _log = LogManager.GetLogger(typeof(ContactUsConfigController));
        }
        #endregion

        #region Actions
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Role()
        {
            ViewBag.Title = "All Roles";
            return View(_roleManager.Roles);
        }

        public IActionResult CreateRole()
        {
            ViewBag.Title = "All Roles";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([Required] string name)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var role = new ApplicationRole
                    {
                        Id = name,
                        Name = name,
                        IsActive = true
                    };
                    IdentityResult result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                        return RedirectToAction("Role");
                    else
                        AddErrorsFromResult(result);
                }
                return View(name);
            }
            catch (Exception ex)
            {
                _log.Error(LogMessageHelper.FormateMessageForException(ex, "CreateRole[POST]"));
            }
            return RedirectToAction("Role", "Admin");

        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                ApplicationRole role = await _roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    IdentityResult result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                        return RedirectToAction("Role");
                    else
                        AddErrorsFromResult(result);
                }
                else
                    ModelState.AddModelError("", "No role found");
                return View("Role", _roleManager.Roles);
            }
            catch (Exception ex)
            {
                _log.Error(LogMessageHelper.FormateMessageForException(ex, "CreateRole[POST]"));
                return RedirectToAction("Role", "Admin");
            }

        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            try
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            catch (Exception ex)
            {
                _log.Error(LogMessageHelper.FormateMessageForException(ex, "AddErrorsFromResult"));
            }
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                _log.Error(LogMessageHelper.FormateMessageForException(ex, "Logout"));
            }
            return RedirectToAction("Index", "Login");

        }
        #endregion
    }
}