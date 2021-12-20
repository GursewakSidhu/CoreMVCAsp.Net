using System;
using CoreMVCDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreMVCDemo.Logics
{
    public class StudentDashboardService
    {
        public StudentDashboardService()
        {
        }

        public CourseViewModel GetStudentMenuDetails(HumberDbContext context, int? userId)
        {
            CourseViewModel modelData = new CourseViewModel();
            List<CourseDetailsModel> _objCourseDetailsModels = GetCourseDetailsService(context);
            modelData.CourseList = from course in _objCourseDetailsModels
                                   select new SelectListItem
                                   {
                                       Text = course.course_name.ToString(),
                                       Value = course.course_fee.ToString()
                                   };
            LoginViewModel _objLoginViewModel = GetLoggedInUserDetails(context, userId);
            modelData.stu_name = _objLoginViewModel.Username;
            modelData.stu_email = _objLoginViewModel.Email;
            modelData.created_by = _objLoginViewModel.userId;
            return modelData;
        }

        public LoginViewModel GetLoggedInUserDetails(HumberDbContext context, int? userId)
        {
            LoginViewModel _objLoginViewModel = new LoginViewModel();
            using (MySqlConnection conn = context.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM tbl_login_credentials where Login_ID = ?userId; ", conn))
                    {
                        cmd.Parameters.Add("?userId", MySqlDbType.Int32).Value = userId;
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _objLoginViewModel.Email = reader["Username"].ToString();
                                _objLoginViewModel.Username = reader["FullName"].ToString();
                                _objLoginViewModel.IsAdmin = Convert.ToInt32(reader["IsAdmin"]);
                                _objLoginViewModel.userId = Convert.ToInt32(reader["Login_ID"]);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error in fetching user details mysql row. Error: " + ex.Message);
                }
            }
            return _objLoginViewModel;
        }

        public List<CourseDetailsModel> GetCourseDetailsService(HumberDbContext context)
        {
            List<CourseDetailsModel> _listCourseDetailsModel = new List<CourseDetailsModel>();
            using (MySqlConnection conn = context.GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM tbl_course_details;", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _listCourseDetailsModel.Add(new CourseDetailsModel()
                            {
                                course_id = Convert.ToInt32(reader["course_id"]),
                                course_name = reader["course_name"].ToString(),
                                course_fee = Convert.ToInt32(reader["course_fee"])
                            });
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error in fetching course details mysql row. Error: " + ex.Message);
                }
            }
            return _listCourseDetailsModel;
        }

        public bool AddCourseService(HumberDbContext context, CourseViewModel _objCourseViewModel)
        {
            return true;
            List<CourseDetailsModel> _objCourseDetailsModels = GetCourseDetailsService(context);
            _objCourseViewModel.stu_course = _objCourseDetailsModels.Where(item => item.course_fee == _objCourseViewModel.CourseId).FirstOrDefault().course_name;

            using (MySqlConnection conn = context.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO tbl_students_details(stu_name, stu_mob, stu_email, stu_course, course_fee, amount, created_by,IsActive, reg_date) VALUES " +
                        "(?stu_name, ?stu_mob, ?stu_email, ?stu_course, ?course_fee, ?amount, ?created_by, ?IsActive, ?reg_date);";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("?stu_name", MySqlDbType.VarChar).Value = _objCourseViewModel.stu_name;
                        cmd.Parameters.Add("?stu_mob", MySqlDbType.VarChar).Value = _objCourseViewModel.stu_mob;
                        cmd.Parameters.Add("?stu_email", MySqlDbType.VarChar).Value = _objCourseViewModel.stu_email;
                        cmd.Parameters.Add("?stu_course", MySqlDbType.VarChar).Value = _objCourseViewModel.stu_course;
                        cmd.Parameters.Add("?course_fee", MySqlDbType.Int32).Value = _objCourseViewModel.course_fee;
                        cmd.Parameters.Add("?amount", MySqlDbType.Int32).Value = _objCourseViewModel.amount;
                        cmd.Parameters.Add("?created_by", MySqlDbType.Int32).Value = _objCourseViewModel.created_by;
                        cmd.Parameters.Add("?IsActive", MySqlDbType.Int32).Value = _objCourseViewModel.IsActive;
                        cmd.Parameters.Add("?reg_date", MySqlDbType.VarChar).Value = DateTime.Now.ToString("yyyyMMddHHmmss");
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error in adding course mysql row. Error: " + ex.Message);
                }
            }
            return true;
        }

        public CourseViewModel SearchCourseService(HumberDbContext context, int? _courseId)
        {
            CourseViewModel _objCourseViewModel = new CourseViewModel();
            using (MySqlConnection conn = context.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM tbl_students_details where stu_id = ?courseId; ", conn))
                    {
                        cmd.Parameters.Add("?courseId", MySqlDbType.Int32).Value = _courseId;
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _objCourseViewModel.stu_name = reader["stu_name"].ToString();
                                _objCourseViewModel.stu_mob = reader["stu_mob"].ToString();
                                _objCourseViewModel.stu_email = reader["stu_email"].ToString();
                                _objCourseViewModel.stu_course = reader["stu_course"].ToString();
                                _objCourseViewModel.course_fee = reader["course_fee"].ToString();
                                _objCourseViewModel.amount = reader["amount"].ToString();
                                _objCourseViewModel.reg_date = Convert.ToDateTime(reader["reg_date"]);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error in fetching user details mysql row. Error: " + ex.Message);
                }
            }
            return _objCourseViewModel;
        }
        public bool EditCourseService(HumberDbContext context, CourseViewModel _objCourseViewModel)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = "UPDATE tbl_students_details set stu_name=?stu_name, stu_mob=?stu_mob, stu_email=?stu_email, " +
                        "stu_course=?stu_course, course_fee=?course_fee, amount=?amount, created_by=?created_by,IsActive=?IsActive, " +
                        "reg_date=?reg_date where stu_id = ?stu_id;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("?stu_id", MySqlDbType.VarChar).Value = _objCourseViewModel.stu_id;
                        cmd.Parameters.Add("?stu_name", MySqlDbType.VarChar).Value = _objCourseViewModel.stu_name;
                        cmd.Parameters.Add("?stu_mob", MySqlDbType.VarChar).Value = _objCourseViewModel.stu_mob;
                        cmd.Parameters.Add("?stu_email", MySqlDbType.VarChar).Value = _objCourseViewModel.stu_email;
                        cmd.Parameters.Add("?stu_course", MySqlDbType.VarChar).Value = _objCourseViewModel.stu_course;
                        cmd.Parameters.Add("?course_fee", MySqlDbType.Int32).Value = _objCourseViewModel.course_fee;
                        cmd.Parameters.Add("?amount", MySqlDbType.Int32).Value = _objCourseViewModel.amount;
                        cmd.Parameters.Add("?created_by", MySqlDbType.Int32).Value = _objCourseViewModel.created_by;
                        cmd.Parameters.Add("?IsActive", MySqlDbType.Int32).Value = _objCourseViewModel.IsActive;
                        cmd.Parameters.Add("?reg_date", MySqlDbType.VarChar).Value = DateTime.Now.ToString("yyyyMMddHHmmss");
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error in adding course mysql row. Error: " + ex.Message);
                }
            }
            return true;
        }

        public List<GetScholarShipStatusViewModel> GetScholarShipStatus(HumberDbContext context, int? userId)
        {
            List<GetScholarShipStatusViewModel> _objGetScholarShipStatusViewModel = new List<GetScholarShipStatusViewModel>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                string query = "call get_scholarship_status(@userId);";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _objGetScholarShipStatusViewModel.Add(new GetScholarShipStatusViewModel()
                            {
                                stu_id = Convert.ToInt32(reader["stu_id"]),
                                stu_name = reader["stu_name"].ToString(),
                                stu_mob = reader["stu_mob"].ToString(),
                                stu_email = reader["stu_email"].ToString(),
                                stu_course = reader["stu_course"].ToString(),
                                reg_date = Convert.ToDateTime(reader["reg_date"]),
                                course_fee = reader["course_fee"].ToString(),
                                amount = reader["amount"].ToString(),
                                status_id = Convert.ToInt32(reader["status_id"]),
                                scholarship_status = Convert.ToInt32(reader["scholarship_status"]),
                                scholarship_name= reader["scholarship_name"].ToString(),
                                ScholarshipAward= Convert.ToInt32(reader["ScholarshipAward"]),
                                scholarship_status_string = Convert.ToInt32(reader["scholarship_status"]) == 0 ? "Not Aprroved yet": "Aprroved"
                            });
                        }
                    }
                }
            }
            return _objGetScholarShipStatusViewModel;
        }
        public List<CourseViewModel> GetRegisteredCourses(HumberDbContext context, int? userId)
        {
            List<CourseViewModel> _objCourseViewModel = new List<CourseViewModel>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                string query = "select * from tbl_students_details where created_by = ?userId and IsActive=1;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.Add("?userId", MySqlDbType.VarChar).Value = userId;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _objCourseViewModel.Add(new CourseViewModel()
                            {
                                stu_id = Convert.ToInt32(reader["stu_id"]),
                                stu_name = reader["stu_name"].ToString(),
                                stu_mob = reader["stu_mob"].ToString(),
                                stu_email = reader["stu_email"].ToString(),
                                stu_course = reader["stu_course"].ToString(),
                                reg_date = Convert.ToDateTime(reader["reg_date"]),
                                course_fee = reader["course_fee"].ToString(),
                                amount = reader["amount"].ToString(),
                                created_by = Convert.ToInt32(reader["created_by"]),
                                IsActive = Convert.ToInt32(reader["IsActive"])
                            });
                        }
                    }
                }
            }
            return _objCourseViewModel;
        }
        public bool DeleteCourseService(HumberDbContext context, int? deleteCourseId)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE tbl_students_details set IsActive=?IsActive where stu_id=?stu_id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("?stu_id", MySqlDbType.VarChar).Value = deleteCourseId;
                        cmd.Parameters.Add("?IsActive", MySqlDbType.Int32).Value = 0;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error in adding course mysql row. Error: " + ex.Message);
                }
            }
            return true;
        }
        public List<ScholarShipViewModel> GetListOfAvailableScholarships(HumberDbContext context, int? userId)
        {
            List<ScholarShipViewModel> _objScholarShipViewModel = new List<ScholarShipViewModel>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                //string query = "select * from tbl_scholarships_details where created_by=?userId and IsActive=?IsActive;";
                //using (MySqlCommand cmd = new MySqlCommand(query, conn))

                string query = "select * from tbl_scholarships_details;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _objScholarShipViewModel.Add(new ScholarShipViewModel()
                            {
                                scholarship_id = Convert.ToInt32(reader["scholarship_id"]),
                                scholarship_name = reader["scholarship_name"].ToString(),
                                award = Convert.ToInt32(reader["award"])
                            });
                        }
                    }
                }
            }
            return _objScholarShipViewModel;
        }
        public List<CourseDetailsModel> GetAppliedCoursesDetailsService(HumberDbContext context, int? userId)
        {
            List<CourseDetailsModel> _listCourseDetailsModel = new List<CourseDetailsModel>();
            using (MySqlConnection conn = context.GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM tbl_students_details where IsActive=?isActive and created_by=?userId;", conn);
                    cmd.Parameters.Add("?userId", MySqlDbType.Int32).Value = userId;
                    cmd.Parameters.Add("?isActive", MySqlDbType.Int32).Value = 1;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _listCourseDetailsModel.Add(new CourseDetailsModel()
                            {
                                course_id = Convert.ToInt32(reader["stu_id"]),
                                course_name = reader["stu_course"].ToString()
                            });
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error in fetching course details mysql row. Error: " + ex.Message);
                }
            }
            return _listCourseDetailsModel;
        }
        public bool ApplyScholarshipService(HumberDbContext context, int? scholarShipId, int? courseId)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO tbl_scholarship_status(scholarship_status, scholarship_id, student_id,IsActive) VALUES (?scholarship_status,?scholarship_id, ?courseId, ?isActive);";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("?scholarship_status", MySqlDbType.Int32).Value = 0;
                        cmd.Parameters.Add("?scholarship_id", MySqlDbType.Int32).Value = scholarShipId;
                        cmd.Parameters.Add("?courseId", MySqlDbType.Int32).Value = courseId;
                        cmd.Parameters.Add("?isActive", MySqlDbType.Int32).Value = 1;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error in adding course mysql row. Error: " + ex.Message);
                }
            }
            return true;
        }
    }
}