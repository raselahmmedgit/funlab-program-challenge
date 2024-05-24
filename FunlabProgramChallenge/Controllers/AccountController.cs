﻿using FunlabProgramChallenge.Core;
using FunlabProgramChallenge.Core.Identity;
using FunlabProgramChallenge.Helpers;
using FunlabProgramChallenge.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FunlabProgramChallenge.ViewModels;
using System.Web;
using FunlabProgramChallenge.Managers;
using System.IdentityModel.Tokens.Jwt;
using FunlabProgramChallenge.JwtGenerator;

namespace FunlabProgramChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        #region Global Variable Declaration
        private readonly ILogger<AccountController> _iLogger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ITokenGenerator _iTokenGenerator;
        private readonly IStripePaymentGatewayManager _iStripePaymentGatewayManager;
        private readonly IEmailSenderManager _iEmailSenderManager;
        private readonly IMemberManager _iMemberManager;
        private readonly IConfiguration _iConfiguration;
        #endregion

        #region Constructor
        public AccountController(ILogger<AccountController> iLogger, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IPasswordHasher<ApplicationUser> passwordHasher,
            ITokenGenerator iTokenGenerator,
            IStripePaymentGatewayManager iStripePaymentGatewayManager,
            IEmailSenderManager iEmailSenderManager,
            IMemberManager iMemberManager,
            IConfiguration iConfiguration)
        {
            _iLogger = iLogger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _iTokenGenerator = iTokenGenerator;
            _iStripePaymentGatewayManager = iStripePaymentGatewayManager;
            _iEmailSenderManager = iEmailSenderManager;
            _iMemberManager = iMemberManager;
            _iConfiguration = iConfiguration;
        }
        #endregion

        #region Actions

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginTokenGenerator([FromBody] LoginViewModel model)
        {
            try
            {
                var token = await _iTokenGenerator.CreateTokenAsync(model.UserEmail);
                if (token.AccessToken != null)
                {
                    var tokenData = new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token.AccessToken),
                        RefreshToken = token.RefreshTokenModel.RefreshToken,
                        ExpiryDate = token.ExpiryDate,
                        Message = MessageHelper.JwtToken
                    };

                    _result = Result.Ok(MessageHelper.Authorized, parentId: "", parentName: "", data: tokenData);
                    return Json(_result);
                }

                _result = Result.Fail(MessageHelper.Unauthorized);
                return Json(_result);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "LoginTokenGenerator[GET]"));
            }

            _result = Result.Fail(MessageHelper.Unauthorized);
            return Json(_result);
        }

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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
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
                            model.ReturnUrl = string.IsNullOrEmpty(model.ReturnUrl) ? "/Admin/Index" : model.ReturnUrl;

                            //var data = await GetUserSignInOutHistoryLocal(model);
                            //await _userLogInOutHistoryManager.CreateLogInOutHistoryAsync(data);
                            _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Login[POST]", $"User logged in, UserEmail: {model.UserEmail}"));
                            var isAdmin = await _userManager.IsInRoleAsync(user, AppConstants.AppRoleName.Admin);
                            if (isAdmin)
                            {
                                if (string.IsNullOrEmpty(model.ReturnUrl))
                                {
                                    _result = Result.Ok(MessageHelper.LogIn, "/Admin/Index");
                                    return Json(_result);
                                }

                                _result = Result.Ok(MessageHelper.LogIn, model.ReturnUrl);
                                return Json(_result);
                            }
                            else
                            {
                                _result = Result.Ok(MessageHelper.LogIn, model.ReturnUrl);
                                return Json(_result);
                            }

                        }

                        if (result.IsLockedOut)
                        {
                            _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Login[POST]", $"User account locked out, UserEmail: {model.UserEmail}"));
                            _result = Result.Fail(MessageHelper.LogInFail);
                            return Json(_result);
                        }
                        else
                        {
                            _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Login[POST]", $"Invalid login attempt, UserEmail: {model.UserEmail}"));
                            _result = Result.Fail(MessageHelper.LogInInvalid);
                            return Json(_result);
                        }
                    }
                    else
                    {
                        _result = Result.Fail(MessageHelper.LogInInvalid);
                        return Json(_result);
                    }
                }
                else {
                    _result = Result.Fail(ExceptionHelper.ModelStateErrorFirstFormat(ModelState));
                    return Json(_result);
                }

            }
            catch (Exception ex)
            {
                //ModelState.AddModelError(string.Empty, MessageHelper.Error);
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "Login[POST]"));
            }

            _result = Result.Fail(MessageHelper.LogInFail);
            return Json(_result);
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
                return Json(_result);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "Logout"));
                //_result = Result.Fail(MessageHelper.LogOutFail);
                //return Json(_result);
            }

            _result = Result.Ok(MessageHelper.LogOutFail, "/Home/Index");
            return Json(_result);
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
                model.RoleName = AppConstants.AppRoleName.Member.ToString();
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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestStart("Register[POST]", ""));
                if (ModelState.IsValid)
                {


                    #region Stripe Payment Gateway

                    var resultStripePayment = await ProcessStripePaymentGatewayAsync(model);
                    if (!resultStripePayment.Success)
                    {
                        _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Register[POST]", resultStripePayment.Message));
                        _result = Result.Fail(resultStripePayment.Message);
                        return Json(_result);
                    }

                    #endregion

                    string userName = ((model.UserEmail).Split('@')[0]).Trim(); // you are get here username.

                    var user = new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserEmail,
                        Email = model.UserEmail
                    };

                    var resultEmailExists = await IsEmailExistsAsync(user);
                    if (!resultEmailExists.Success)
                    {
                        _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Register[POST]", resultEmailExists.Message));
                        _result = Result.Fail(resultEmailExists.Message);
                        return Json(_result);
                    }

                    //var role = new ApplicationRole
                    //{
                    //    Id = model.RoleName,
                    //    Name = model.RoleName
                    //};
                    var role = new ApplicationRole
                    {
                        Id = AppConstants.AppRole.Member.ToString(),
                        Name = AppConstants.AppRoleName.Member.ToString()
                    };
                    //IdentityResult resultRole = await _roleManager.CreateAsync(role);
                    var resultRoleName = await _roleManager.GetRoleNameAsync(role);

                    //if (resultRole.Succeeded)
                    if (!string.IsNullOrEmpty(resultRoleName))
                    {
                        model.UserPassword = string.IsNullOrEmpty(model.UserPassword) ? "Qwer!234" : model.UserPassword;

                        var result = await _userManager.CreateAsync(user, model.UserPassword);
                        if (result.Succeeded)
                        {
                            model.ReturnUrl = string.IsNullOrEmpty(model.ReturnUrl) ? "/Admin/Index" : model.ReturnUrl;

                            //await _userManager.AddToRoleAsync(user, model.RoleName);
                            await _userManager.AddToRoleAsync(user, resultRoleName);

                            await _signInManager.SignInAsync(user, isPersistent: false);
                            _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Register[POST]", $"User created a new account with password, UserEmail:{model.UserEmail}"));

                            #region Create Member

                            var resultMember = await CreateMemberAsync(model);
                            if (!resultMember.Success)
                            {
                                _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Register[POST]", resultMember.Message));
                                _result = Result.Fail(resultMember.Message);
                                return Json(_result);
                            }

                            #endregion

                            var isAdmin = await _userManager.IsInRoleAsync(user, AppConstants.AppRoleName.Admin);
                            if (isAdmin)
                            {
                                if (string.IsNullOrEmpty(model.ReturnUrl))
                                {
                                    _result = Result.Ok(MessageHelper.LogIn, "/Admin/Index");
                                    return Json(_result);
                                }

                                _result = Result.Ok(MessageHelper.Register, model.ReturnUrl);
                                return Json(_result);
                            }
                            else
                            {
                                _result = Result.Ok(MessageHelper.Register, model.ReturnUrl);
                                return Json(_result);
                            }

                        }

                        _iLogger.LogInformation(LoggerMessageHelper.LogFormattedMessageForRequestSuccess("Register[POST]", $"User creation failed, UserEmail:{model.UserEmail}"));
                        _result = Result.Fail(MessageHelper.RegisterFail);
                        return Json(_result);
                    }

                }
                else
                {
                    _result = Result.Fail(ExceptionHelper.ModelStateErrorFirstFormat(ModelState));
                    return Json(_result);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, MessageHelper.Error);
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "Register[POST]"));
            }

            _result = Result.Fail(MessageHelper.RegisterFail);
            return Json(_result);
        }


        private async Task<StripePaymentGatewayResult> ProcessStripePaymentGatewayAsync(RegisterViewModel registerViewModel)
        {
            try
            {
                string cardExpirationMonth = ((registerViewModel.CardExpiration).Split('/')[0]).Trim();
                string cardExpirationYear = ((registerViewModel.CardExpiration).Split('/')[1]).Trim();

                var stripePaymentGatewayViewModel = new StripePaymentGatewayViewModel() {
                    CardNumber = registerViewModel.CardNumber,
                    CardCvc = registerViewModel.CardCvc,
                    CardName = registerViewModel.UserName,
                    CardExpirationMonth = cardExpirationMonth,
                    CardExpirationYear = cardExpirationYear,
                    CardCurrencyCode = registerViewModel.CardCountry,
                    CustomerEmailAddress = registerViewModel.UserEmail,
                    CustomerPhoneNumber = "01911-045573",
                    CustomerName = registerViewModel.UserName,
                    CustomerDescription = "this is test",
                    CardAmount = 10,
                };

                var result = await _iStripePaymentGatewayManager.ProcessAsync(stripePaymentGatewayViewModel);

                return result;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError(string.Empty, MessageHelper.Error);
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "ProcessStripePaymentGatewayAsync"));
                return StripePaymentGatewayResult.Fail(MessageHelper.Error);
            }
        }

        private async Task<Result> CreateMemberAsync(RegisterViewModel registerViewModel)
        {
            try
            {
                string cardExpirationMonth = ((registerViewModel.CardExpiration).Split('/')[0]).Trim();
                string cardExpirationYear = ((registerViewModel.CardExpiration).Split('/')[1]).Trim();

                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

                var memberViewModel = new MemberViewModel()
                {
                    UserId = user.Id,
                    FirstName = registerViewModel.UserName,
                    LastName = registerViewModel.UserName,
                    EmailAddress = registerViewModel.UserEmail,
                    PhoneNumber = "01911-045573",
                    PresentAddress = "Dhaka",
                    PermanentAddress = "Dhaka",
                    CardName = registerViewModel.UserName,
                    CardNumber = registerViewModel.CardNumber,
                    CardExpirationMonth = cardExpirationMonth,
                    CardExpirationYear = cardExpirationYear,
                    CardCvc = registerViewModel.CardCvc,
                    CardCountry = registerViewModel.CardCountry
                };

                var result = await _iMemberManager.InsertMemberAsync(memberViewModel);

                var emailMessage = new EmailMessage()
                {
                    ReceiverEmail = registerViewModel.UserEmail,
                    ReceiverName = registerViewModel.UserName,
                    Subject = "Funlab sign in password",
                    Body = EmailTemplateHelper.GetEmailTemplate("Funlab sign in password", "Funlab system-generated sign in password", registerViewModel.UserPassword),
                    IsHtml = true
                };

                var resultSendEmail = await _iEmailSenderManager.SendEmailMessage(emailMessage);

                return result;
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError(string.Empty, MessageHelper.Error);
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "ProcessStripePaymentGatewayAsync"));
                return Result.Fail(MessageHelper.Error);
            }
        }

        private async Task<Result> IsEmailExistsAsync(ApplicationUser user)
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
                //ModelState.AddModelError(string.Empty, MessageHelper.Error);
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "IsEmailExistsAsync"));
                return Result.Fail(MessageHelper.Error);
            }
        }

        #region Helpers

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

        #endregion

        #endregion
    }
}