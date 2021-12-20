using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreMVCDemo.Models
{
    public class ScholarShipViewModel
    {
        [Display(Name = "Scholarship Id")]
        public int  scholarship_id { get; set; }
        [Display(Name = "Scholarship Name")]
        public string scholarship_name { get; set; }
        [Display(Name = "Award")]
        public int award { get; set; }
    }
    public class ScholarShipSuperViewModel
    {
        public List<ScholarShipViewModel> AvailableScholarships { get; set; }
        [Required(ErrorMessage = "Please select Course")]
        [Display(Name = "Student Course")]
        public int CourseId { get; set; }
        public IEnumerable<SelectListItem> SavedCourses { get; set; }
        public string userId { get; set; }
    }
    public class GetScholarShipStatusViewModel
    {
        [Display(Name = "User Id")]
        public int user_Id { get; set; }
        [Display(Name = "CourseId")]
        public int stu_id { get; set; }
        [Display(Name = "Student Name")]
        public string stu_name { get; set; }
        [Display(Name = "Student Contact")]
        [MinLength(10, ErrorMessage = "Contact Number must be atleast 10 characters")]
        public string stu_mob { get; set; }
        [Display(Name = "Student Email")]
        public string stu_email { get; set; }
        [Display(Name = "Student Course")]
        public string stu_course { get; set; }
        [Display(Name = "Registration Date")]
        public DateTime reg_date { get; set; }
        [Display(Name = "Course Fee")]
        public string course_fee { get; set; }
        [Display(Name = "Balance")]
        public string amount { get; set; }
        [Display(Name = "Status Id")]
        public int status_id { get; set; }
        [Display(Name = "Scholarship Id")]
        public int scholarship_id { get; set; }
        [Display(Name = "Scholarship Name")]
        public string scholarship_name { get; set; }
        [Display(Name = "ScholarShip Award")]
        public int ScholarshipAward { get; set; }
        [Display(Name = "Scholarship Status")]
        public int scholarship_status { get; set; }
        public string scholarship_status_string { get; set; }
    }
}
