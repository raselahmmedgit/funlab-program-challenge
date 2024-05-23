using FunlabProgramChallenge.Core.Identity;
using FunlabProgramChallenge.Helpers;
using FunlabProgramChallenge.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FunlabProgramChallenge.Components
{
    public class UserInfo: ViewComponent
    {
        #region Global Variable Declaration
        private readonly ILogger<UserInfo> _iLogger;
        private UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructor
        public UserInfo(ILogger<UserInfo> iLogger, UserManager<ApplicationUser> userManager)
        {
            _iLogger = iLogger;
            _userManager = userManager;
        }
        #endregion

        #region Actions
        public async Task<IViewComponentResult> InvokeAsync()
        {
            LoggedUserViewModel loggedUserViewModel = new LoggedUserViewModel();
            try
            {
                //string userName = HttpContext.User.Identity.Name;
                loggedUserViewModel = await GetUser();
            }
            catch (Exception ex)
            {
                _iLogger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "InvokeAsync"));
            }
            return View(loggedUserViewModel);
        }

        public async Task<LoggedUserViewModel> GetUser()
        {
            LoggedUserViewModel loggedUserViewModel = new LoggedUserViewModel();
            try
            {
                ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
                var roles = await _userManager.GetRolesAsync(user);
                
                loggedUserViewModel.UserName = user?.UserName;
                loggedUserViewModel.Role = roles.FirstOrDefault();
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
