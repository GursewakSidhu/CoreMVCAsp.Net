using System;
using CoreMVCDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CoreMVCDemo.Logics
{
    public class LoginService
    {
        List<SignUpViewModel> _objSignUpViewModelData;
        public LoginService()
        {
            _objSignUpViewModelData = new List<SignUpViewModel>();
        }
        public SignUpViewModel LoginUserService(HumberDbContext context, LoginViewModel _objLoginViewModel)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbl_login_credentials;", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _objSignUpViewModelData.Add(new SignUpViewModel()
                        {
                            Login_ID = Convert.ToInt32(reader["Login_ID"]),
                            Username = reader["Username"].ToString(),
                            IsAdmin = Convert.ToInt32(reader["IsAdmin"]),
                            Password = reader["Password"].ToString(),
                            FullName = reader["FullName"].ToString(),
                            AccountCreatedDate = Convert.ToDateTime(reader["AccountCreatedDate"]),
                            Age = reader["Age"].ToString(),
                            Gender = reader["Gender"].ToString()
                        });
                    }
                }
            }
            var matches = _objSignUpViewModelData.Where(p => p.Username == _objLoginViewModel.Username && p.Password == _objLoginViewModel.Password);

            if (matches.Any())
            {
                return matches.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
}
