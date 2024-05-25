using FunlabProgramChallenge.Core.Identity;
using FunlabProgramChallenge.Helpers;
using FunlabProgramChallenge.JwtGenerator;
using FunlabProgramChallenge.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FunlabProgramChallenge.Components
{
    public class UserInfo: ViewComponent
    {
        #region Global Variable Declaration
        private readonly ILogger<UserInfo> _iLogger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenManager _iTokenManager;
        #endregion

        #region Constructor
        public UserInfo(ILogger<UserInfo> iLogger, UserManager<ApplicationUser> userManager, ITokenManager iTokenManager)
        {
            _iLogger = iLogger;
            _userManager = userManager;
            _iTokenManager = iTokenManager;
        }
        #endregion

        #region Actions
        public async Task<IViewComponentResult> InvokeAsync(string token)
        {
            LogInUserViewModel loggedUserViewModel = new LogInUserViewModel();
            try
            {
                loggedUserViewModel = await GetUser(token);
            }
            catch (Exception ex)
            {
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "InvokeAsync"));
            }
            return View(loggedUserViewModel);
        }

        public async Task<LogInUserViewModel> GetUser(string token)
        {
            LogInUserViewModel loggedUserViewModel = new LogInUserViewModel();
            try
            {
                var tokenClaimsPrincipal = _iTokenManager.GetClaimsPrincipalByToken(token);
                var user = await _userManager.GetUserAsync(tokenClaimsPrincipal);
                if (user != null) 
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    loggedUserViewModel.UserName = user?.UserName;
                    loggedUserViewModel.RoleName = roles.FirstOrDefault();
                }
                
            }
            catch (Exception ex)
            {
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "GetUser"));
            }
            return loggedUserViewModel;
        }
        #endregion
    }
}
