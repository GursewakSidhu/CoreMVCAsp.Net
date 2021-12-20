using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
namespace CoreMVCDemo.Models
{
    public class HumberDbContext
    {
        public string ConnectionString { get; set; }


        public HumberDbContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
