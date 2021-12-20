using System;
using System.Collections.Generic;
using CoreMVCDemo.Models;
using MySql.Data.MySqlClient;

namespace CoreMVCDemo.Logics
{
    public class AdminDashboardService
    {
        public AdminDashboardService()
        {
        }
        public List<HumberStudentModel> GetAllEnrolledStudents(HumberDbContext context)
        {
            List<HumberStudentModel> list = new List<HumberStudentModel>();
            
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbl_students_details;", conn);

                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        list.Add(new HumberStudentModel()
                        {
                            stu_id = Convert.ToInt32(reader["stu_id"]),
                            stu_name = reader["stu_name"].ToString(),
                            stu_mob = Convert.ToInt64(reader["stu_mob"]),
                            stu_email = reader["stu_email"].ToString(),
                            stu_course = reader["stu_course"].ToString(),
                            reg_date = Convert.ToDateTime(reader["reg_date"]),
                            course_fee = Convert.ToInt32(reader["course_fee"]),
                            amount = Convert.ToInt32(reader["amount"]),
                            created_by = reader["created_by"].ToString(),
                            IsActive = Convert.ToInt32(reader["IsActive"])
                        });
                    }
                }
            }
            return list;
        }
        public List<GetScholarShipStatusViewModel> GetAllScholarshipsStatus(HumberDbContext context, int? userId)
        {
            List<GetScholarShipStatusViewModel> _objGetScholarShipStatusViewModel = new List<GetScholarShipStatusViewModel>();
            using (MySqlConnection conn = context.GetConnection())
            {
                conn.Open();
                string query = "call get_scholarship_status(@userId);";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", null);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _objGetScholarShipStatusViewModel.Add(new GetScholarShipStatusViewModel()
                            {
                                user_Id = (int)userId,
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
                                scholarship_name = reader["scholarship_name"].ToString(),
                                ScholarshipAward = Convert.ToInt32(reader["ScholarshipAward"]),
                                scholarship_status_string = Convert.ToInt32(reader["scholarship_status"]) == 0 ? "Not Aprroved yet" : "Aprroved"
                            });
                        }
                    }
                }
            }
            return _objGetScholarShipStatusViewModel;
        }
        public bool UpdateScholarshipsStatus(HumberDbContext context, AdminUpdateScholarShipStatusViewModel _objStatusUpdateData)
        {
            using (MySqlConnection conn = context.GetConnection())
            {
                try
                {
                    conn.Open();
                    for (int i = 0; i < _objStatusUpdateData.ApprovedStatusIds.Count; i++)
                    {
                        string query = "UPDATE tbl_scholarship_status set scholarship_status=?approvedStatus where status_id=?status_id";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.Add("?approvedStatus", MySqlDbType.Int32).Value = 1;
                            cmd.Parameters.Add("?status_id", MySqlDbType.Int32).Value = _objStatusUpdateData.ApprovedStatusIds[i];
                            cmd.ExecuteNonQuery();
                        }
                    }
                    for (int j = 0; j < _objStatusUpdateData.RejectedStatusIds.Count; j++)
                    {
                        string query = "UPDATE tbl_scholarship_status set scholarship_status=?rejectedStatus where status_id=?statusid";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.Add("?rejectedStatus", MySqlDbType.Int32).Value = 0;
                            cmd.Parameters.Add("?statusid", MySqlDbType.Int32).Value = _objStatusUpdateData.RejectedStatusIds[j];
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception("Error in updating scholarship status mysql row. Error: " + ex.Message);
                }
            }
            return true;
        }

    }
}
