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
    public class StudentDashboardController : Controller
    {
        StudentDashboardService _objStudentDashboardService;
        List<CourseDetailsModel> _objCourseDetailsModels;
        public StudentDashboardController()
        {
            _objStudentDashboardService = new StudentDashboardService();
            _objCourseDetailsModels = new List<CourseDetailsModel>();
        }
        
        public IActionResult Index(int? userId)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            return View(_objStudentDashboardService.GetStudentMenuDetails(context, userId));
        }
        [HttpPost]
        public IActionResult AddCourse(CourseViewModel _objCourseViewModel)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            return Json(_objStudentDashboardService.AddCourseService(context, _objCourseViewModel));
        }

        [HttpGet]
        public IActionResult SearchCourses(int? userId)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            return View(_objStudentDashboardService.GetStudentMenuDetails(context, userId));
        }

        [HttpPost]
        public IActionResult SearchRegisteredCourse(int? _courseId)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            return Json(_objStudentDashboardService.SearchCourseService(context, _courseId));
        }
        [HttpPost]
        public IActionResult EditCourse(CourseViewModel _objCourseViewModel)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            return Json(_objStudentDashboardService.EditCourseService(context, _objCourseViewModel));
        }

        [HttpGet]
        public IActionResult ViewRegisteredCourses(int? userId)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            return View(_objStudentDashboardService.GetRegisteredCourses(context, userId));
        }
        [HttpPost]
        public IActionResult DeleteCourse(int? deleteCourseId)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            return Json(_objStudentDashboardService.DeleteCourseService(context, deleteCourseId));
        }

        [HttpGet]
        public IActionResult ApplyScholarships(int? userId)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            ScholarShipSuperViewModel _objScholarShipSuperViewModel = new ScholarShipSuperViewModel();
            _objScholarShipSuperViewModel.AvailableScholarships = _objStudentDashboardService.GetListOfAvailableScholarships(context, userId);
            List<CourseDetailsModel> _objCourseDetailsModels = _objStudentDashboardService.GetAppliedCoursesDetailsService(context, userId);
            _objScholarShipSuperViewModel.SavedCourses = from course in _objCourseDetailsModels
                                                         select new SelectListItem
                                                         {
                                                             Text = course.course_name.ToString(),
                                                             Value = course.course_id.ToString()
                                                         };
            _objScholarShipSuperViewModel.userId = userId.ToString();
            return View(_objScholarShipSuperViewModel);
        }
        [HttpPost]
        public IActionResult UpdateScholarships(int? scholarShipId, int? courseId)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            return Json(_objStudentDashboardService.ApplyScholarshipService(context, scholarShipId, courseId));
        }
        [HttpGet]
        public IActionResult ViewScholarshipsStatus(int? userId)
        {
            HumberDbContext context = HttpContext.RequestServices.GetService(typeof(CoreMVCDemo.Models.HumberDbContext)) as HumberDbContext;
            List<GetScholarShipStatusViewModel> _objGetScholarShipStatusViewModel = _objStudentDashboardService.GetScholarShipStatus(context, userId);
            _objGetScholarShipStatusViewModel.FirstOrDefault().user_Id = (int)userId;
            return View(_objGetScholarShipStatusViewModel);
        }
    }
}
