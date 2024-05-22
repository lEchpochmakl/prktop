using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace demex
{
    public partial class OperatorForm : Form
    {
        public OperatorForm()
        {
            InitializeComponent();
        }

        private void OperatorForm_Load(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();

            string sql = "SELECT * from inputdatarequests where requestStatus = 'Новая заявка'";
            MySqlCommand command = new MySqlCommand(sql, db.returnConnection());
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dataGridView2.Rows.Add(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
            }
            reader.Close();

            sql = "SELECT * from inputdatausers where type = 'Специалист'";
            command = new MySqlCommand(sql, db.returnConnection());
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader.GetInt64(0), reader.GetString(1), reader.GetInt64(2));
            }

            reader.Close();

            db.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();

            string sql = "UPDATE inputdatarequests set masterID = @masterId, requestStatus = 'В процессе ремонта' where requestID = @requestId and requestStatus = 'Новая заявка'";
            MySqlCommand command = new MySqlCommand(sql, db.returnConnection());
            command.Parameters.Add("@masterId", MySqlDbType.Int64).Value = int.Parse(textBox2.Text.ToString());
            command.Parameters.Add("@requestId", MySqlDbType.Int64).Value = int.Parse(textBox1.Text.ToString());

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected != 0)
            {
                MySqlDataReader reader = command.ExecuteReader();
                MessageBox.Show("Заявка назначена специалисту!");
                OperatorForm form = new OperatorForm();
                form.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка! ID выбраны неверно.");
            }

            db.closeConnection();
        }
    }
}
