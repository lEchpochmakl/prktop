using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
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
    public partial class requestEdit : Form
    {
        public static int globalRequestId;
        public static int globaClientId;

        public requestEdit(int requestId, int clientId)
        {
            globalRequestId = requestId;
            globaClientId = clientId;
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RequestStart form = new RequestStart(globaClientId);
            form.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();

            string sql = "UPDATE inputdatarequests set climateTechType = @type, climateTechModel = @model, problemDescryption = @description where requestID = @globalRequestId";

            MySqlCommand command = new MySqlCommand(sql, db.returnConnection());
            command.Parameters.Add("@type", MySqlDbType.String).Value = textBox1.Text;
            command.Parameters.Add("@model", MySqlDbType.String).Value = textBox2.Text;
            command.Parameters.Add("@description", MySqlDbType.String).Value = textBox3.Text;
            command.Parameters.Add("@globalRequestId", MySqlDbType.Int64).Value = globalRequestId;

            try
            {
                MySqlDataReader reader = command.ExecuteReader();
                MessageBox.Show("Заявка успешно изменена!");
                reader.Close();


                db.closeConnection();

                RequestStart form = new RequestStart(globaClientId);
                form.Show();
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка! Проверьте правильность введённых данных." + ex);

            }

            db.closeConnection();

        }

        private void requestEdit_Load(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();

            string sql = "SELECT climateTechType, climateTechModel, problemDescryption from inputdatarequests where requestID = @globalRequestId";
            MySqlCommand command = new MySqlCommand(sql, db.returnConnection());
            command.Parameters.Add("@globalRequestId", MySqlDbType.Int64).Value = globalRequestId;
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                textBox1.Text = reader.GetString(0);
                textBox2.Text = reader.GetString(1);
                textBox3.Text = reader.GetString(2);
            }

            db.closeConnection();
        }
    }
}
