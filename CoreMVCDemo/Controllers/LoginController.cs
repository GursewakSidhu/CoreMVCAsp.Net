using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoreMVCDemo.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using CoreMVCDemo.Logics;

namespace CoreMVCDemo.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        LoginService _objLoginService;
        SignUpViewModel _objSignUpViewModelData;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
            _objLoginService = new LoginService();
            _objSignUpViewModelData = new SignUpViewModel();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginUser(LoginViewModel _objLoginViewModel)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            return Json(_objLoginService.LoginUserService(context, _objLoginViewModel));
            /* if (_objSignUpViewModelData == null) {
                return Json("User Doesn't Exist! Please try again");
            }
            else{
                if (_objSignUpViewModelData.IsAdmin == 1) {
                    return RedirectToAction("Index", "AdminDashboard");
                }
                else {
                    return RedirectToAction("Index", "StudentDashboard");
                }
            }*/
        }
    }
}
