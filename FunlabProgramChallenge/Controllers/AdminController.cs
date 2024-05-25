﻿using FunlabProgramChallenge.Core.Identity;
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

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    return View();
                }
                else
                {
                    return View("Unauthorized");
                }
                
            }
            catch (Exception ex)
            {
                return ErrorView(ex);
            }
        }
        
        #endregion
    }
}