using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace demex
{
    internal class DB
    {
        MySqlConnection Mysql = new MySqlConnection("server=127.0.0.1; port=3306; username=root;password=;database=byrulov");

        public void openConnection()
        {
            Mysql.Open();
        }
        public void closeConnection()
        {
            Mysql.Close();
        }
        public MySqlConnection returnConnection()
        {
            return Mysql;
        }
    }

}
