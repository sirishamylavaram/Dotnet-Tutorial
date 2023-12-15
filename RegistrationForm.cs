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
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }
       
        private void btnregister_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtusername.Text) || string.IsNullOrWhiteSpace(txtpassword.Text))
            {
                MessageBox.Show("Please fill in all the fields");
            }
            else
            {
                try
                {
                    SqlConnection con = new SqlConnection("Data Source=DESKTOP-N4O5ADP;Initial Catalog=Admin;Integrated Security=True");

                    // Check if the email already exists in the database
                    if (IsEmailAlreadyRegistered(txtEmail.Text.Trim(), con))
                    {
                        MessageBox.Show("This email address is already registered. Please use a different email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Stop the registration process if email is already registered
                    }

                    // Proceed with registration if the email is not already registered
                    SqlCommand cmd = new SqlCommand(@"insert into Admin ([email],[username],[password], [firstname], [lastname]) values ('" + txtEmail.Text + "','" + txtusername.Text + "','" + txtpassword.Text + "','" + txtFirstName.Text + "','" + txtLastName.Text + "')", con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Registered Successfully");
                    con.Close();

                    // Concatenate first name and last name to get the full name
                    string fullName = $"{txtFirstName.Text} {txtLastName.Text}";

                    // Call SendConfirmationEmail with the full name
                    SendConfirmationEmail(txtEmail.Text.Trim(), fullName, txtmobile.Text.Trim(), txtAddress.Text.Trim());

                    MessageBox.Show("Registration successful. Confirmation email sent.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open the login form
                    OpenLoginForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void OpenLoginForm()
        {
            // Close the registration form or navigate to the login form
            this.Hide();

            // Create an instance of Form1 (or your login form) and show it
            LoginForm form1 = new LoginForm();
            form1.ShowDialog();

            // Close the RegistrationForm when Form1 is closed
            this.Close();
        }


        // Function to check if the email is already registered
        private bool IsEmailAlreadyRegistered(string email, SqlConnection connection)
        {
            try
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Admin WHERE email=@Email";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            finally
            {
                connection.Close();
            }
        }
        private void SendConfirmationEmail(string to, string fullName, string mobile, string address)
        {
            try
            {
                string from = "mylavaramsirisha91@gmail.com"; // Replace with your email address
                string emailPassword = "zfkb rxac lpkl utry"; // Replace with your email password

                MailMessage message = new MailMessage();
                message.To.Add(to);
                message.From = new MailAddress(from);

                // Construct the email body with registration details
                string body = $"Dear {fullName},\n" +

                    $"**Thank you for choosing [KENT GRAND PLUS PURIFIER]!**\n\n" 

                   + "We are delighted to welcome you as a valued customer and appreciate your trust in our water purification solutions.\n\n" 
                   + "Here are the details of your water purifier registration:\n\n" 

                   + $"**Customer Information:**\n\n" 

                   + $"- Full Name: {fullName}\n" 
                   + $"- Email Address: {to}\n" 
                   + $"- Contact Number: {mobile}\n" 
                   + $"- Address: {address}\n\n" 

                   + $"-**Thank you once again for choosing [Your Company Name]. We are committed to providing you with clean and safe drinking water. If you have any further questions or concerns, don't hesitate to reach out.**\n\n" 

                    + $"**Best Regards,\n\n"
                   + $"[KENT GRAND PLUS PURIFIER]\n\n" 
                   + $"[KENT GRAND PLUS PURIFIER Contact Information :1800-001-896]";

                message.Body = body;
                message.Subject = "Welcome to [KENT GRAND PLUS PURIFIER] - Water Purifier Registration Successful!";

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Credentials = new NetworkCredential("mylavaramsirisha91@gmail.com", "zfkb rxac lpkl utry");
                    smtpClient.Send(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while sending the confirmation email: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtusername.Clear();
            txtpassword.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtmobile.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            OpenLoginForm();
        }

        private void txtmobile_TextChanged(object sender, EventArgs e)
        {
            // Allow only numeric input for the mobile number
            string text = txtmobile.Text;
            txtmobile.Text = new string(text.Where(char.IsDigit).ToArray());
            txtmobile.SelectionStart = txtmobile.Text.Length;
        }
    }

}
