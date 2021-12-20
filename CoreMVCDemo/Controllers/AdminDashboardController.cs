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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreMVCDemo.Controllers
{
    public class AdminDashboardController : Controller
    {
        public AdminDashboardService _objAdminDashboardService;
        bool _objStatusSignUpData;
        SignUpService _objLoginService;
        public AdminDashboardController()
        {
            _objAdminDashboardService = new AdminDashboardService();
            _objLoginService = new SignUpService();
        }
        public IActionResult Index(int? userId)
        {
            TempData["userId"] = userId;
            return  View();
        }
        public IActionResult ViewAllEnrolledStudents(int? userId)
        {
            TempData["userId"] = userId;
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            return View(_objAdminDashboardService.GetAllEnrolledStudents(context));
        }
        public IActionResult ViewAllAppliedScholarships(int? userId)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            return View(_objAdminDashboardService.GetAllScholarshipsStatus(context, userId));
        }

        public IActionResult ApproveRejectScholarships(AdminUpdateScholarShipStatusViewModel _objStatusUpdateData)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            return Json(_objAdminDashboardService.UpdateScholarshipsStatus(context, _objStatusUpdateData));
        }
        public IActionResult AddAdminAccount(int? userId)
        {
            TempData["userId"] = userId;
            SignUpViewModel modelData = new SignUpViewModel();
            IEnumerable<GenderType> GenderType = Enum.GetValues(typeof(GenderType))
                                                       .Cast<GenderType>();
            modelData.GenderList = from gender in GenderType
                                   select new SelectListItem
                                   {
                                       Text = gender.ToString(),
                                       Value = ((int)gender).ToString()
                                   };
            return View(modelData);
        }

        [HttpPost]
        public IActionResult SignUpAdminUser(SignUpViewModel _objSignUpViewModel)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            _objSignUpViewModel.IsAdmin = 1;
            _objStatusSignUpData = _objLoginService.SignUpUserService(context, _objSignUpViewModel);
            if (_objStatusSignUpData)
            {
                return Json("User Data saved succesfully.");
            }
            else
            {
                return Json("Username already Exist! Please try again with another username.");
            }
        }
    }
}
