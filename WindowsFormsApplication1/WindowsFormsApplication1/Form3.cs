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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int card_no=0;
            int exists = 1;
            MySqlConnection conn = new MySqlConnection("HOST=localhost; USER=root; DATABASE=library; PASSWORD=1111;");
            MySqlCommand cmd1 = new MySqlCommand("SELECT count(*) AS card_count FROM BORROWER", conn);
            MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM BORROWER", conn);
            try
            {
                conn.Open();
                MySqlDataReader rd1 = cmd1.ExecuteReader();
                while (rd1.Read())
                    card_no = rd1.GetInt32("card_count");
                rd1.Close();
                MySqlDataReader rd2 = cmd2.ExecuteReader();
                while (rd2.Read())
                {
                    if (textBox1.Text.Equals(rd2.GetString("fname")) && textBox2.Text.Equals(rd2.GetString("lname")) && textBox3.Text.Equals(rd2.GetString("address")))
                    {
                        MessageBox.Show("This user already exists");
                        exists *= 0;
                    }
                    else
                        exists *= 1;
                }
                rd2.Close();
                if (exists == 1 && textBox4.Text.Equals(textBox5.Text) && textBox4.Text.All(Char.IsLetterOrDigit) && textBox4.Text.Length > 5 && textBox4.Text.Length < 16)
                {
                    card_no = card_no + 9001;
                    MySqlCommand cmd3 = new MySqlCommand("INSERT INTO BORROWER(card_no,fname,lname,address,phone,password) VALUES ('" + card_no + "','" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + maskedTextBox1.Text + "','"+textBox4.Text+"')", conn);
                    if (!String.IsNullOrEmpty(textBox1.Text) && !String.IsNullOrEmpty(textBox2.Text) && !String.IsNullOrEmpty(textBox3.Text))
                    {
                        cmd3.ExecuteNonQuery();
                        MessageBox.Show("Sign Up Successful! \n Your Card_No : " + card_no + "");
                    }
                    else
                        MessageBox.Show("First name,Last name and Address is necessary to Sign Up.");
                }
                else if (exists == 1 && textBox4.Text == textBox5.Text && !textBox4.Text.All(Char.IsLetterOrDigit))
                    MessageBox.Show("Passwords must contain alphanumeric characters only and Password must be of length between 6 to 15.");
                else if(textBox4.Text!=textBox5.Text)
                    MessageBox.Show("Passwords do not match.");
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error:" + ex.ToString() + "");
            }
        }
       
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 firstForm = new Form1(false);
            firstForm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
