using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace demex
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;

            DB db = new DB();

            db.openConnection();

            string sql = "SELECT password, userID, type from inputdatausers where login = @login";

            MySqlCommand command = new MySqlCommand(sql, db.returnConnection());
            command.Parameters.Add("@login", MySqlDbType.String).Value = login;

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                if (reader[0].ToString() == password)
                {
                    MessageBox.Show("Авторизация успешна!");
                    if (reader[2].ToString() == "Заказчик")
                    { 
                        RequestStart form = new RequestStart(int.Parse(reader[1].ToString()));
                        form.Show();
                    }
                    if (reader[2].ToString() == "Оператор")
                    {
                        OperatorForm form = new OperatorForm();
                        form.Show();
                    }
                    if (reader[2].ToString() == "Специалист")
                    {
                        SpecialForm form = new SpecialForm(reader[1].ToString());
                        form.Show();
                    }
                    if (reader[2].ToString() == "Менеджер")
                    {
                        ManagerForm form = new ManagerForm();
                        form.Show();
                    }
                }
                else
                    MessageBox.Show("Неверынй логин или пароль.");
            }

            reader.Close();
            db.closeConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

    }
}
