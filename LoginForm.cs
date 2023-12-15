using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KENTPLUS_PROJECT
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtusername.Text) || string.IsNullOrWhiteSpace(txtpassword.Text))
            {
                MessageBox.Show("Enter both username and password for login");
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-N4O5ADP;Initial Catalog=Admin;Integrated Security=True"))
                    {
                        con.Open();

                        // Hash the entered password
                        string hashedPassword = (txtpassword.Text);

                        SqlCommand cmd = new SqlCommand("SELECT * FROM Admin WHERE username=@username AND password=@password", con);
                        cmd.Parameters.AddWithValue("@username", txtusername.Text);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            string loggedInUserName = GetUserNameFromDatabase(txtusername.Text); // Assuming a method to get the username

                            MessageBox.Show("Login Successfully");
                            this.Hide();

                            // Show the main form with the logged-in username
                            MainForm mainForm = new MainForm(loggedInUserName);
                            mainForm.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password");
                            txtusername.Clear();
                            txtpassword.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private string GetUserNameFromDatabase(string enteredUsername)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-N4O5ADP;Initial Catalog=Admin;Integrated Security=True"))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT username FROM Admin WHERE username=@username", con);
                    cmd.Parameters.AddWithValue("@username", enteredUsername);

                    object result = cmd.ExecuteScalar();

                    return result != null ? result.ToString() : null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting username from the database: " + ex.Message);
                return null;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.Hide();

            // Show the registration form
            RegistrationForm registration = new RegistrationForm();
            registration.ShowDialog();

            // After the registration form is closed, show the login form again
            this.Show();
        }

        private void btnForgot_Click(object sender, EventArgs e)
        {
            this.Hide();
            ForgotForm forgot = new ForgotForm();
            forgot.ShowDialog();
        }
    }
}

