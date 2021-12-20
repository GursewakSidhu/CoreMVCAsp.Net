using System;
namespace CoreMVCDemo.Models
{
    public class HumberStudentModel
    {
        
        public int stu_id { get; set; }
        public string stu_name { get; set; }
        public long stu_mob { get; set; }
        public string stu_email { get; set; }
        public string stu_course { get; set; }
        public DateTime reg_date { get; set; }
        public int course_fee { get; set; }
        public int amount { get; set; }
        public string created_by { get; set; }
        public int IsActive { get; set; }

    }
}
