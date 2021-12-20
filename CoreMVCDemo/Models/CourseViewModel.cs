using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreMVCDemo.Models
{
    public class CourseViewModel
    {
        public CourseViewModel()
        {
            CourseList = new List<SelectListItem>();
        }
        [Display(Name = "CourseId")]
        public int stu_id { get; set; }
        [Display(Name = "Student Name")]
        public string stu_name { get; set; }
        [Display(Name = "Student Contact")]
        [MinLength(10, ErrorMessage = "Contact Number must be atleast 10 characters")]
        public string stu_mob { get; set; }
        [Display(Name = "Student Email")]
        public string stu_email { get; set; }

        [Required(ErrorMessage = "Please select Course")]
        [Display(Name = "Student Course")]
        public int CourseId { get; set; }
        public IEnumerable<SelectListItem> CourseList { get; set; }
        public string stu_course { get; set; }


        [Display(Name = "Registration Date")]
        public DateTime reg_date { get; set; }
        [Display(Name = "Course Fee")]
        public string course_fee { get; set; }
        [Display(Name = "Balance")]

        [Required(ErrorMessage = "Balance is required")]
        [MaxLength(4), MinLength(1)]
        [Range(0, 9999)]
        public string amount { get; set; }
        [Display(Name = "Created By User/Login Id")]
        public int created_by { get; set; }
        [Display(Name = "Course Is Active")]
        public int IsActive { get; set; }
    }
    public class CourseDetailsModel {
        public int course_id { get; set; }
        public string course_name { get; set; }
        public int course_fee { get; set; }

    }
}
