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
    public class SignUpService
    {
        
        public SignUpService()
        {
            
        }
        
        public bool SignUpUserService(HumberDbContext context, SignUpViewModel _objSignUpViewModelData)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                try
                {
                    _objSignUpViewModelData.Gender =  _objSignUpViewModelData.GenderId == 1 ? "Male" : "Female";
                    conn.Open();
                    string query = "INSERT INTO tbl_login_credentials(FullName, Age, Gender, UserName, Password, IsAdmin, AccountCreatedDate) VALUES " +
                        "(?fullName,?age, ?gender, ?username,?password, ?isadmin, ?createdDate);";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("?fullName", MySqlDbType.VarChar).Value = _objSignUpViewModelData.FullName;
                        cmd.Parameters.Add("?age", MySqlDbType.Int32).Value = _objSignUpViewModelData.Age;
                        cmd.Parameters.Add("?gender", MySqlDbType.VarChar).Value = _objSignUpViewModelData.Gender;
                        cmd.Parameters.Add("?username", MySqlDbType.VarChar).Value = _objSignUpViewModelData.Username;
                        cmd.Parameters.Add("?password", MySqlDbType.VarChar).Value = _objSignUpViewModelData.Password;
                        cmd.Parameters.Add("?isadmin", MySqlDbType.Int32).Value = _objSignUpViewModelData.IsAdmin;
                        cmd.Parameters.Add("?createdDate", MySqlDbType.VarChar).Value = DateTime.Now.ToString("yyyyMMddHHmmss");
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error in adding signup mysql row. Error: " + ex.Message);
                }
            }
            return true;
        }
    }
}
