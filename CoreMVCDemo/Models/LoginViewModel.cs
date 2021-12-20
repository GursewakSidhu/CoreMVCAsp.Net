using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreMVCDemo.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int IsAdmin { get; set; }
        public int userId { get; set; }
    }
}
