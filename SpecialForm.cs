using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demex
{
    public partial class SpecialForm : Form
    {
        public static string globalMasterId;

        public SpecialForm(string masterId)
        {
            InitializeComponent();
            globalMasterId = masterId;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SpecialForm_Load(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();

            string sql = "select * from inputdatarequests where masterID = @globalMasterId and requestStatus = 'В процессе ремонта'";
            MySqlCommand command = new MySqlCommand(sql, db.returnConnection());
            command.Parameters.Add("@globalMasterId", MySqlDbType.Int64).Value = int.Parse(globalMasterId);
            MySqlDataReader reader = command.ExecuteReader();

            string repairParts;


            while (reader.Read())
            {

                if (reader.GetValue(7) == null)
                {
                    repairParts = "";
                }
                else
                    repairParts = reader.GetValue(7).ToString();

                dataGridView2.Rows.Add(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), repairParts);
            }

            reader.Close();
            db.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();

            string sql = "UPDATE inputdatarequests set repairParts = @parts where requestID = @requestId";
            MySqlCommand command = new MySqlCommand(sql, db.returnConnection());
            command.Parameters.Add("@parts", MySqlDbType.String).Value = textBox3.Text.ToString();
            command.Parameters.Add("@requestId", MySqlDbType.Int64).Value = int.Parse(textBox2.Text.ToString());

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                try
                {

                    MySqlDataReader reader = command.ExecuteReader();
                    MessageBox.Show("Данные занесены!");
                    reader.Close();
                }
                catch
                {
                    MessageBox.Show("Ошибка!");
                }
            }
            else
            {
                MessageBox.Show("Ошибка!");
            }

            sql = "INSERT into inputdatacomments (message, masterID, requestID) values (@message, @globalMasterId, @requestId)";
            command = new MySqlCommand(sql, db.returnConnection());
            command.Parameters.Add("@message", MySqlDbType.String).Value = textBox1.Text.ToString();
            command.Parameters.Add("@globalMasterId", MySqlDbType.String).Value = globalMasterId;
            command.Parameters.Add("@requestId", MySqlDbType.Int64).Value = textBox2.Text.ToString();

            if (textBox1.Text.ToString() != "")
            {
                try
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Close();
                    MessageBox.Show("Комментарий оставлен!");

                }
                catch
                {
                    MessageBox.Show("Ошибка!");
                }

            }

            SpecialForm form = new SpecialForm(globalMasterId);
            form.Show();
            this.Close();

            db.closeConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();

            string sql = "UPDATE inputdatarequests set requestStatus = 'Готов к выдаче' where requestID = @requestId";
            MySqlCommand command = new MySqlCommand(sql, db.returnConnection());
            command.Parameters.Add("@requestId", MySqlDbType.Int64).Value = int.Parse(textBox2.Text.ToString());


            int rowsaffected = command.ExecuteNonQuery();

            if (rowsaffected == 0)
            {
                MessageBox.Show("Ошибка!");
            }
            else
            {
                MySqlDataReader reader = command.ExecuteReader();
                reader.Close();
                MessageBox.Show("Успешно!");
                SpecialForm form = new SpecialForm(globalMasterId);
                form.Show();
                this.Close();
            }
            db.closeConnection();
        }
    }
}
