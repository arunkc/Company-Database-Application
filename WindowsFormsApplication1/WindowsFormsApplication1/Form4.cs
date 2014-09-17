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
    public partial class Form4 : Form
    {
        Boolean logged_in=false;
        String text1;
        String text2;
        String text3;
        String text4;
        public Form4(String t1, String t2, String t3, String t4, Boolean arg5)
        {
            InitializeComponent();
            text1 = t1;
            text2 = t2;
            text3 = t3;
            text4 = t4;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("HOST=localhost; USER=root; DATABASE=library; PASSWORD=1111;");
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM BORROWER", conn);
            try
            {
                conn.Open();
                MySqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    if (textBox1.Text == rd.GetString("card_no") && textBox2.Text == rd.GetString("password"))
                    {
                        MessageBox.Show("Welcome Back :" + rd.GetString("fname") + " " + rd.GetString("lname") + ".");
                        logged_in = true;
                        Form2 secondForm = new Form2(text1,text2,text3,text4,rd.GetString("card_no"),logged_in);
                        secondForm.Show();
                        this.Hide();
                    }                    
                }
                if(!logged_in)
                    {
                        MessageBox.Show("Card number or password is incorrect.\nPassword is case sensitive check your CapsLk.\nAlso Check your password by clicking Reveal Password.\nIf problem still persists click forgot password.");
                        logged_in = false;
                    }
                rd.Close();
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


        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 thirdForm = new Form3();
            thirdForm.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '\0';
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form5 fifthForm = new Form5();
            fifthForm.Show();
            this.Hide();
        }

       
    }
}
