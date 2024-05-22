using FunlabProgramChallenge.Core;
using FunlabProgramChallenge.Core.Identity;
using FunlabProgramChallenge.Helpers;
using FunlabProgramChallenge.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FunlabProgramChallenge.ViewModels;
using System.Web;

namespace FunlabProgramChallenge.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        #region Global Variable Declaration
        private readonly ILogger<AccountController> _iLogger;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _iConfiguration;
        #endregion

        #region Constructor
        public AccountController(ILogger<AccountController> iLogger, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration iConfiguration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _iConfiguration = iConfiguration;
            _iLogger = iLogger;
        }
        #endregion

        #region Actions

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            try
            {
                LoginViewModel model = new LoginViewModel();
                model.ReturnUrl = returnUrl;
                return View(model);
            }
            catch (Exception ex)
            {
                return ErrorView(ex);
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestStart("Login[POST]", $"UserEmail: {model.UserEmail}"));
                if (ModelState.IsValid)
                {
                    ApplicationUser user = await _userManager.FindByNameAsync(model.UserEmail);
                    if (user != null)
                    {
                        await _signInManager.SignOutAsync();
                        // This doesn't count login failures towards account lockout
                        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                        var result = await _signInManager.PasswordSignInAsync(model.UserEmail, model.UserPassword, model.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            //var data = await GetUserSignInOutHistoryLocal(model);
                            //await _userLogInOutHistoryManager.CreateLogInOutHistoryAsync(data);
                            _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Login[POST]", $"User logged in, UserEmail: {model.UserEmail}"));
                            var isAdmin = await _userManager.IsInRoleAsync(user, AppConstants.AppRole.Admin);
                            if (isAdmin)
                            {
                                if (string.IsNullOrEmpty(model.ReturnUrl))
                                {
                                    return RedirectToAction("Index", "Admin");
                                }
                                return Redirect(model.ReturnUrl);
                            }
                            else
                            {
                                return RedirectToLocal(model.ReturnUrl);
                            }

                        }

                        if (result.IsLockedOut)
                        {
                            _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Login[POST]", $"User account locked out, UserEmail: {model.UserEmail}"));
                            return View("Lockout");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Login[POST]", $"Invalid login attempt, UserEmail: {model.UserEmail}"));
                            return View(model);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, MessageHelper.Error);
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "Login[POST]"));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "Logout"));
            }
            return RedirectToAction("Index", "Home");

        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            try
            {
                RegisterViewModel model = new RegisterViewModel();
                model.ReturnUrl = returnUrl;
                model.RoleName = AppConstants.AppRole.Member.ToString();
                return View(model);
            }
            catch (Exception ex)
            {
                return ErrorView(ex);
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestStart("Register[POST]", ""));
                if (ModelState.IsValid)
                {
                    string userName = ((model.UserEmail).Split('@')[0]).Trim(); // you are get here username.

                    var user = new ApplicationUser
                    {
                        UserName = model.UserEmail,
                        Email = model.UserEmail
                    };

                    _result = await IsEmailExists(user);
                    if (!_result.Success)
                    {
                        ModelState.AddModelError(string.Empty, _result.Error);
                        _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Register[POST]", _result.Error));
                        return View(model);
                    }

                    var role = new ApplicationRole
                    {
                        Id = model.RoleName,
                        Name = model.RoleName
                    };
                    //IdentityResult resultRole = await _roleManager.CreateAsync(role);
                    var resultRoleName = await _roleManager.GetRoleNameAsync(role);

                    //if (resultRole.Succeeded)
                    if (!string.IsNullOrEmpty(resultRoleName))
                    {
                        var result = await _userManager.CreateAsync(user, model.UserPassword);
                        if (result.Succeeded)
                        {
                            //await _userManager.AddToRoleAsync(user, model.RoleName);
                            await _userManager.AddToRoleAsync(user, resultRoleName);

                            await _signInManager.SignInAsync(user, isPersistent: false);
                            _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Register[POST]", $"User created a new account with password, UserEmail:{model.UserEmail}"));

                            var isAdmin = await _userManager.IsInRoleAsync(user, AppConstants.AppRole.Admin);
                            if (isAdmin)
                            {
                                if (string.IsNullOrEmpty(model.ReturnUrl))
                                {
                                    return RedirectToAction("Index", "Admin");
                                }
                                return Redirect(model.ReturnUrl);
                            }
                            else
                            {
                                return RedirectToLocal(model.ReturnUrl);
                            }

                        }

                        _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Register[POST]", $"User creation failed, UserEmail:{model.UserEmail}"));
                        AddErrors(result);
                    }

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, MessageHelper.Error);
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "Register[POST]"));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return ErrorView(ex);
            }
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestStart("ForgotPassword[POST]", $"UserEmail: {model.Email}"));
                if (ModelState.IsValid)
                {
                    bool isValid = true;
                    var user = await _userManager.FindByNameAsync(model.Email);
                    if (user == null)
                    {
                        isValid = false;
                        // Don't reveal that the user does not exist
                        ModelState.AddModelError(string.Empty, "Invalid email.");
                        _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("ForgotPassword[POST]", $"Invalid email, UserEmail: {model.Email}"));
                        return View(model);
                    }

                    //var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                    //if (!isEmailConfirmed)
                    //{
                    //    isValid = false;
                    //    // Don't reveal that the user email is not confirmed
                    //    ModelState.AddModelError(string.Empty, "Email is not confirmed.");
                    //    _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("ForgotPassword[POST]", $"Email is not confirmed, UserEmail: {model.Email}"));
                    //    return View(model);
                    //}

                    if (isValid)
                    {

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                        // Send an email with this link
                        var message = await GenareteForgotPasswordEmailTemplateAsync(user);
                        //await _emailSender.SendEmailBySendGridAsync(user.Id, model.Email, "Reset Password", message);
                        return View("ForgotPasswordConfirmation");

                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, MessageHelper.Error);
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "ForgotPassword[POST]"));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return ErrorView(ex);
            }
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userId, string email, string code = null)
        {
            try
            {
                if (userId == null || email == null || code == null)
                {
                    return View("Error");
                }
                else
                {
                    ResetPasswordViewModel model = new ResetPasswordViewModel() { Code = HttpUtility.HtmlDecode(code).Trim(), Email = HttpUtility.HtmlDecode(email).Trim() };
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return ErrorView(ex);
            }
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestStart("ResetPassword[POST]", $"UserEmail: {model.Email}"));
                if (ModelState.IsValid)
                {
                    bool isValid = true;
                    var user = await _userManager.FindByNameAsync(model.Email);
                    if (user == null)
                    {
                        isValid = false;
                        // Don't reveal that the user does not exist
                        ModelState.AddModelError(string.Empty, "Invalid email.");
                        _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("ResetPassword[POST]", $"Invalid email, UserEmail: {model.Email}"));
                        return View(model);
                    }

                    //var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                    //if (!isEmailConfirmed)
                    //{
                    //    isValid = false;
                    //    // Don't reveal that the user email is not confirmed
                    //    ModelState.AddModelError(string.Empty, "Email is not confirmed.");
                    //    _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("ResetPassword[POST]", $"Email is not confirmed, UserEmail: {model.Email}"));
                    //    return View(model);
                    //}

                    if (isValid)
                    {
                        var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                        if (result.Succeeded)
                        {
                            return View("ResetPasswordConfirmation");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, MessageHelper.Error);
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "ResetPassword[POST]"));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return ErrorView(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestStart("LogOff", $"User:{User.Identity.Name}"));
                await _signInManager.SignOutAsync();
                _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("LogOff", $"User logged out"));
            }
            catch (Exception ex)
            {
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "LogOff"));
            }
            return RedirectToAction("Login", "Account");
        }

        private async Task<Result> IsEmailExists(ApplicationUser user)
        {
            try
            {
                var isExists = await _userManager.FindByEmailAsync(user.Email);

                if (isExists != null)
                {
                    string isEmailExistsMessage = string.Format(MessageHelper.IsEmailExists, user.Email);
                    return Result.Fail(isEmailExistsMessage);
                }
                else
                {
                    return Result.Ok();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, MessageHelper.Error);
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "IsEmailExists"));
                return Result.Fail(MessageHelper.Error);
            }
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private async Task<string> GenareteForgotPasswordEmailTemplateAsync(ApplicationUser user)
        {
            string htmlTemplate = string.Empty;

            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { userId = user.Id, email = user.Email, code = passwordResetToken }, protocol: HttpContext.Request.Scheme);

            string title = "Please reset your password by clicking here:";
            string linkText = "Forgot Password";

            //htmlTemplate = "Please reset your password by clicking here: <a target='_blank' href=\"" + link + "\">link</a>";
            htmlTemplate = EmailTemplateHelper.GetEmailTemplate(title, link, linkText);

            return htmlTemplate;
        }

        #endregion

        #endregion
    }
}