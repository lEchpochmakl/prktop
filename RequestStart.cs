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
    public partial class RequestStart : Form
    {
        public static int globalId;

        public RequestStart(int id)
        {
            InitializeComponent();
            globalId = id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();

            string sql = "INSERT into inputdatarequests (startDate, climateTechType, climateTechModel, problemDescryption, requestStatus, clientID) values (@startDate, @type, @model, @description, @status, @clientId)";
            MySqlCommand command = new MySqlCommand(sql, db.returnConnection());
            command.Parameters.Add("@startDate", MySqlDbType.VarChar).Value = DateTime.Now;
            command.Parameters.Add("@type", MySqlDbType.String).Value = textBox1.Text.ToString();
            command.Parameters.Add("@model", MySqlDbType.String).Value = textBox2.Text.ToString();
            command.Parameters.Add("@description", MySqlDbType.String).Value = textBox3.Text.ToString();
            command.Parameters.Add("@status", MySqlDbType.String).Value = "Новая заявка";
            command.Parameters.Add("@clientId", MySqlDbType.Int64).Value = globalId;

            try
            {
                MySqlDataReader reader = command.ExecuteReader();


                reader.Close();

                MessageBox.Show("Заявка успешно добавлена!");

                RequestStart form = new RequestStart(globalId);
                form.Show();
                this.Close();

                db.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка! Некорректно введены данные." + ex);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void RequestStart_Load(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();

            string sql = "SELECT * from inputdatarequests where clientID = @globalId";

            MySqlCommand command = new MySqlCommand(sql, db.returnConnection());
            command.Parameters.Add("@globalId", MySqlDbType.Int64).Value = globalId;
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
            }


            db.closeConnection();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            requestEdit form = new requestEdit(int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()), globalId);
            form.Show();
            this.Close();
        }
    }
}
