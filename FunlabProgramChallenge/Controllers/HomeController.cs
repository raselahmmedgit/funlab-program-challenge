using FunlabProgramChallenge.Models;
using FunlabProgramChallenge.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FunlabProgramChallenge.Controllers
{
    public class HomeController : BaseController
    {
        #region Global Variable Declaration
        private readonly ILogger<HomeController> _logger;
        #endregion

        #region Constructor
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion
    }
}