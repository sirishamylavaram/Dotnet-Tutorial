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
        private object enteredUsername;
        private object fullName;
        private object email;
        private object address;
        private object username;
        private string loggedInUsername;
        private string loggedInFullName;
        private string loggedInEmail;
        private string loggedInAddress;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
           if (string.IsNullOrWhiteSpace(txtUsername.Text) && string.IsNullOrWhiteSpace(txtpassword.Text))
            {
                MessageBox.Show("Must be Enter both username and password for login", "WaterGuardPro-LoginForm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Must be Enter username for login", "WaterGuardPro-LoginForm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrWhiteSpace(txtpassword.Text))
            {
                MessageBox.Show("Must be Enter password for login", "WaterGuardPro-LoginForm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {

                            GetUserInformationFromDatabase();

                            this.Hide();
                            MainForm mainForm = new MainForm();
                            mainForm.ShowDialog();
                        }


                        else
                        {
                            MessageBox.Show("Invalid username or password", "WaterGuardPro-LoginForm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtUsername.Clear();
                            txtpassword.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "WaterGuardPro-LoginForm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GetUserInformationFromDatabase()
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-N4O5ADP;Initial Catalog=Admin;Integrated Security=True"))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT fullname, email, address, moblie, username FROM Admin WHERE username=@username", con);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserInformation.Email = reader["email"].ToString();
                            UserInformation.address = reader["address"].ToString();
                            UserInformation.FullName = reader["fullname"].ToString();
                            UserInformation.moblie = reader["moblie"].ToString();
                            UserInformation.Username = reader["username"].ToString(); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting user information from the database: " + ex.Message);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            //this.Hide();

            // Show the registration form
            RegistrationForm registration = new RegistrationForm();
            registration.ShowDialog();

            // After the registration form is closed, show the login form again
            this.Show();
        }

        private void btnForgot_Click(object sender, EventArgs e)
        {
            //this.Hide();
            ForgotForm forgot = new ForgotForm();
            forgot.ShowDialog();
        }
    }
}

