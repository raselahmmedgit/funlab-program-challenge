using FunlabProgramChallenge.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FunlabProgramChallenge.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        #region Global Variable Declaration
        private readonly ILogger<AdminController> _iLogger;
        #endregion

        #region Constructor
        public AdminController(ILogger<AdminController> iLogger)
        {
            _iLogger = iLogger;
        }
        #endregion

        #region Actions
        public IActionResult Index()
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
        
        #endregion
    }
}