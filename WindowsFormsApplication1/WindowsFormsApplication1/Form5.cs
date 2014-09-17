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

namespace WindowsFormsApplication1
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("HOST=localhost; USER=root; DATABASE=library; PASSWORD=1111;");
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM BORROWER", conn);
            Boolean found = false;
            try
            {
                conn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                while(rd.Read())
                {
                    if (textBox1.Text == rd.GetString("card_no") && textBox2.Text == rd.GetString("lname") && maskedTextBox1.Text == rd.GetString("phone"))
                    {
                        MessageBox.Show("Your Password:" + rd.GetString("password"));
                        found = true;
                    }
                }
                if (found == false)
                    MessageBox.Show("You have entered Incorrect Details.");
                rd.Close();
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error:" + ex.ToString() + "");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 firstForm = new Form1(false);
            firstForm.Show();
            this.Hide();
        }
    }
}
