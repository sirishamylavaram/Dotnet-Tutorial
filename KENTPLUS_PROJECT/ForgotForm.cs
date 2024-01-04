using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KENTPLUS_PROJECT
{
    public partial class ForgotForm : Form
    {
        private string connectionstring = "Data Source=DESKTOP-N4O5ADP;Initial Catalog=Admin;Integrated Security=True";
  

        public ForgotForm()
        {
            InitializeComponent();
        }

       
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string userEmail = txtEmail.Text.Trim();

            // Check if the email is empty
            if (string.IsNullOrEmpty(userEmail))
            {
                MessageBox.Show("Please provide an email address.", "WaterGuardPro-ForgotForm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Exit the method if email is not provided
            }

            if (UserExists(userEmail))
            {
                // Retrieve the user's current password from the database
                string currentPassword = GetCurrentPasswordFromDatabase(userEmail, out string firstname, out string lastname);
                string fullName = "mylavaramsirisha";
                if (!string.IsNullOrEmpty(currentPassword))
                {
                    // Send the current password to the user's email
                    SendCurrentPasswordEmail(userEmail, currentPassword, UserInformation.FullName);

                    //MessageBox.Show("The current password has been sent to your email. Please check your inbox.", "Password Retrieval", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Close the ForgotForm
                    this.Close();

                    // Open the login form
                    OpenLoginForm();
                }
                else
                {
                    MessageBox.Show("An error occurred providing email.", "WaterGuardPro-ForgotForm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("An error occurred This email address is not registered. Please check the email or register a new account.", "WaterGuardPro-ForgotForm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OpenLoginForm()
        {
           LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void SendCurrentPasswordEmail(string userEmail, string currentPassword, string fullName)
        {
            try
            {

                string from = "mylavaramsirisha91@gmail.com"; // Replace with your email address
                string emailPassword = "zfkb rxac lpkl utry";
                MailMessage message = new MailMessage();
                message.To.Add(userEmail);  // Use the provided userEmail parameter
                message.From = new MailAddress(from);

                // Construct the email body message using placeholders
                string body = $"Dear {UserInformation.FullName},\n"

                     + "We received a request to Retrieval the password for your Water Purifier account. If you did not make this request, please ignore this email.\n\n"
                     
                     + $"Your current password is: {currentPassword}\n\n"
                     
                     + "If you have any questions or concerns, please contact our support team at [WaterGuardPro@gmail.com Email] or call us at [1800-001-896].\n\n"

                     + "Best Regards,\n\n"
                     + "The Water Purifier Team\n"
                     + "[WaterGuardPro]\n"
                     + "[1800-001-896]";

                message.Body = body;
                message.Subject = "Password Retrieval Request for WaterGuardPro Water Purifier Account";

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Credentials = new NetworkCredential("mylavaramsirisha91@gmail.com", "zfkb rxac lpkl utry");
                    smtpClient.Send(message);
                }

                MessageBox.Show("Password sent successfully. Check your email inbox.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "WaterGuardPro-ForgotForm", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetCurrentPasswordFromDatabase(string userEmail, out string firstname, out string lastname)
        {
            firstname = string.Empty;
            lastname = string.Empty;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    string query = "SELECT Password, firstname, lastname FROM Admin WHERE email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Email", userEmail);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            firstname = reader["FirstName"].ToString();
                            lastname = reader["LastName"].ToString();
                            return reader["Password"].ToString();
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving current password from the database: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private bool UserExists(string userEmail)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Admin WHERE email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Email", userEmail);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"WaterGuardPro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.ShowDialog();
        }
    }
}

