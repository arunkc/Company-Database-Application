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
    public partial class Form2 : Form
    {
        int button2_clicks = 1;
        String books;
        String book_id;
        String branch_id;
        DateTime due_date;
        TimeSpan days;
        Boolean error = false;
        Boolean logged_in;
        public Form2(String text1,String text2,String text3,String text4,String text5,Boolean b)
        {
            InitializeComponent();
            textBox4.Text = text1;
            textBox5.Text = text2;
            textBox6.Text = text3;
            textBox1.Text = text5;
            books = text4;
            logged_in = b;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("HOST=localhost; USER=root; DATABASE=library; PASSWORD=1111;");
            MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM BORROWER", conn);
            MySqlCommand cmd2 = new MySqlCommand("SELECT title FROM book b,book_loans bl WHERE b.book_id=bl.book_id AND card_no='"+textBox1.Text+"'", conn);
            try
            {
                conn.Open();
                MySqlDataReader rd1 = cmd1.ExecuteReader();             
                while (rd1.Read())
                {
                   if(textBox1.Text==rd1.GetString("card_no"))
                   {
                       textBox2.Text = rd1.GetString("fname");
                       textBox3.Text = rd1.GetString("lname");
                   }
                }
                rd1.Close();
                MySqlDataReader rd2 = cmd2.ExecuteReader();
                comboBox1.Items.Clear();
                comboBox1.Text = "";
                while(rd2.Read())
                {
                    comboBox1.Items.Add(rd2.GetString("title"));
                    comboBox1.SelectedIndex = 0;
                }
                rd2.Close();
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error:" + ex.ToString() + "");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("HOST=localhost; USER=root; DATABASE=library; PASSWORD=1111;");
            try
            {
                DateTime today = DateTime.Now;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM book_loans WHERE card_no ='"+textBox1.Text+"' AND book_id='"+book_id+"' AND branch_id='"+branch_id+"'", conn);
                MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM book_loans WHERE card_no ='" + textBox1.Text + "' AND book_id='" + book_id + "' AND branch_id='" + branch_id + "'", conn);
                MySqlDataReader rd = cmd1.ExecuteReader();
                while(rd.Read())
                    due_date=rd.GetDateTime("due_date");
                days = due_date.Date - DateTime.Now.Date;
                rd.Close();
                if (comboBox1.SelectedIndex!=-1)
                {
                    cmd.ExecuteNonQuery();
                    if(days.Days<0)
                        MessageBox.Show("This book is overdue by " + -(days.Days) + " days.\n Fine: 10¢ for each day overdue. \n Number of days overdue :\t " + -(days.Days) + " \n Fine per day :\t\t 10¢ \n Total fine :\t\t " + -(days.Days) * 10 + "¢");
                    MessageBox.Show("Check In successful.");
                }
                else
                    MessageBox.Show("You have not selected any book to check in.");
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error:"+ex.ToString()+"");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2_clicks == 1||error)
            {
                button2_clicks += 1;
                int books_available;
                int books_rented=0;
                if (!String.IsNullOrEmpty(books))
                    books_available = int.Parse(books);
                else
                    books_available = 0;
                MySqlConnection conn = new MySqlConnection("HOST=localhost; USER=root; DATABASE=library; PASSWORD=1111;");
                MySqlCommand cmd1 = new MySqlCommand("INSERT INTO BOOK_LOANS(book_id,branch_id,card_no,date_out,due_date) VALUES ('"+textBox5.Text+"','"+textBox6.Text+"','"+textBox1.Text+"',curdate(),curdate()+ INTERVAL 14 DAY)", conn);
                MySqlCommand cmd2 = new MySqlCommand("SELECT count(*) AS books_rented FROM book_loans WHERE card_no="+textBox1.Text+"",conn);
                try
                {
                    conn.Open();
                    MySqlDataReader rd;
                    if (textBox1.TextLength != 0)
                    {
                        rd = cmd2.ExecuteReader();
                        while (rd.Read())
                            books_rented = rd.GetInt32("books_rented");
                        rd.Close();
                    }
                    else
                    {
                        MessageBox.Show("Book ID,Branch ID and Card Number are necessary fields for Check Out.");
                        error = true;
                    }
                    if (textBox1.TextLength != 0 && textBox5.TextLength != 0 && textBox6.TextLength != 0)
                    {
                        DateTime today = DateTime.Now;
                        DateTime date_out = today.Date;
                        DateTime due_date = date_out.AddDays(14);
                        if(books_rented < 3 && books_available > 0)
                            cmd1.ExecuteNonQuery();
                        else if (books_rented >= 3)
                            MessageBox.Show("Sorry! It seems you have rented 3 books already.\n A user can rent a maximum of 3 books.");
                        else if (books_available == 0 && !error)
                            MessageBox.Show("Sorry! The selected book is not available in this branch at this time.\n Check later or Check availability in other branches.");                        
                        else
                            MessageBox.Show("Check Out of your Book :" + textBox4.Text + " is Successful.\n Due date of the book is:" + due_date.Date + "");
                    }
                    else
                    {
                        if (!error)
                            MessageBox.Show("Book ID,Branch ID and Card Number are necessary fields for Check Out.");
                        error = true;
                    }                                        
                    conn.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error:" + ex.ToString() + "");;
                }
            }
            else
            {
                MessageBox.Show("Sorry:For security reasons checkout can be accessed only once. \nTo rent the book again press Change Selection and Select Your Book again.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 thirdForm = new Form3();
            thirdForm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 firstForm = new Form1(logged_in);
            firstForm.Show();
            this.Hide();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("HOST=localhost; USER=root; DATABASE=library; PASSWORD=1111;");
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM BORROWER WHERE fname LIKE '"+textBox2.Text+"' AND lname LIKE '"+textBox3.Text+"'", conn);
                MySqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    textBox1.Text = rd.GetString("card_no");
                    textBox2.Text = rd.GetString("fname");
                    textBox3.Text = rd.GetString("lname");
                }
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error:" + ex.ToString() + "");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("HOST=localhost; USER=root; DATABASE=library; PASSWORD=1111;");
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM BORROWER WHERE fname LIKE '"+textBox2.Text+"' AND lname LIKE '"+textBox3.Text+"'", conn);
                MySqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    textBox1.Text = rd.GetString("card_no");
                    textBox2.Text = rd.GetString("fname");
                    textBox3.Text = rd.GetString("lname");
                }
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error:" + ex.ToString() + "");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("HOST=localhost; USER=root; DATABASE=library; PASSWORD=1111;");
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT b.book_id,branch_id FROM book b,book_loans bl WHERE b.book_id=bl.book_id AND title='"+comboBox1.SelectedItem+"'", conn);
                MySqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    book_id = rd.GetString("book_id");
                    branch_id = rd.GetString("branch_id");
                }
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error:" + ex.ToString() + "");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            String card = textBox1.Text;
            textBox1.Clear();
            textBox1.Text = card;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("HOST=localhost; USER=root; DATABASE=library; PASSWORD=1111;");
            try
            {
                DateTime today = DateTime.Now;
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM book_loans WHERE card_no ='" + textBox1.Text + "' AND book_id='" + book_id + "' AND branch_id='" + branch_id + "'", conn);
                MySqlDataReader rd = cmd1.ExecuteReader();
                while (rd.Read())
                    due_date = rd.GetDateTime("due_date");
                days = due_date.Date - DateTime.Now.Date;
                if (comboBox1.SelectedIndex != -1 && textBox1.Text != "")
                {
                    if (days.Days < 0)
                        MessageBox.Show("This book is overdue by " + -(days.Days) + " days.");
                    else
                        MessageBox.Show("Due date for the selected book is : " + due_date.Date);
                }
                else if (textBox1.Text == "")
                    MessageBox.Show("Enter a card number to check books rented.");
                else
                    MessageBox.Show("You have not rented any book.");
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error:" + ex.ToString() + "");
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            logged_in = false;
            MessageBox.Show("Logged Out Successfully.");
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }
    }
}
