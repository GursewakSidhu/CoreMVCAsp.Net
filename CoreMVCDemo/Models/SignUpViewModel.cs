using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreMVCDemo.Models
{
    public class SignUpViewModel
    {
        public SignUpViewModel()
        {
            GenderList = new List<SelectListItem>();
        }

        [ScaffoldColumn(false)]
        public int Login_ID { get; set; }

        [Required(ErrorMessage = "Please enter Gender")]
        [Display(Name = "Gender")]
        public int GenderId { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }
        public string Gender { get; set; }


        [DataType(DataType.EmailAddress)]
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter Username Email ID")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [MinLength(10, ErrorMessage = "The Password must be atleast 10 characters")]
        [MaxLength(20, ErrorMessage = "The Password cannot be more than 20 characters")]
        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Must be between 10 and 20 characters", MinimumLength = 10)]
        [Required(ErrorMessage = "Confirm password is required ")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }

        public int IsAdmin { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter Fullname"), MaxLength(50)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [MaxLength(3), MinLength(1)]
        [Range(18, 100)]
        public string Age { get; set; }

        public DateTime AccountCreatedDate { get; set; }

        //[Required(ErrorMessage = "Phone Number Required")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter PhoneNumber as 0123456789, 012-345-6789, (012)-345-6789.")]
    }
    public enum GenderType
    {
        Male = 1,
        Female = 2
    }
}
