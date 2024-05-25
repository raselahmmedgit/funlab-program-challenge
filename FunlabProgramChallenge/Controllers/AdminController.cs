using FunlabProgramChallenge.Core;
using FunlabProgramChallenge.Core.Identity;
using FunlabProgramChallenge.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FunlabProgramChallenge.Controllers
{
    //[Authorize]
    public class AdminController : BaseController
    {
        #region Global Variable Declaration
        private readonly ILogger<AdminController> _iLogger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        #endregion

        #region Constructor
        public AdminController(ILogger<AdminController> iLogger, SignInManager<ApplicationUser> signInManager)
        {
            _iLogger = iLogger;
            _signInManager = signInManager;
        }
        #endregion

        #region Actions
        public IActionResult Index()
        {
            try
            {
                var currentUser = HttpContext.User;

                if (currentUser != null)
                {
                    return View();
                }
                else
                {
                    return View("/Home/Unauthorized");
                }
                
            }
            catch (Exception ex)
            {
                return ErrorView(ex);
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestStart("LogOut", $"User:{User.Identity.Name}"));
                await _signInManager.SignOutAsync();
                _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("LogOut", $"User logged out"));

                _result = Result.Ok(MessageHelper.LogOut);
                return Ok(_result);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "Logout"));
                //_result = Result.Fail(MessageHelper.LogOutFail);
                //return Json(_result);
            }

            _result = Result.Ok(MessageHelper.LogOutFail, "/Home/Index");
            return Ok(_result);
        }

        #endregion
    }
}