using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace KENTPLUS_PROJECT
{
    public partial class InstallationForm : Form
    {
        private string connectionString = "Data Source=DESKTOP-N4O5ADP;Initial Catalog=Admin;Integrated Security=True";
        private object enteredUsername;
        private object recipientName;
        private string recipientEmail;

        public object Email { get; private set; }
        public object UserInfo { get; private set; }

        public InstallationForm(string address)
        {
            InitializeComponent();
            LoadProductsIntoComboBox();

        }

        private void LoadProductsIntoComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "select productname from product";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        cmdproduct.DisplayMember = "productname";
                        cmdproduct.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void cmdproduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmdproduct.Text != null)
            {
                string selectProductName = cmdproduct.Text.ToString();

                string serialNumber = GetSerialNumberForProduct(selectProductName);

                txtSerialnumber.Text = serialNumber;
                txtAddress.Text = UserInformation.address;

            }
        }

        private string GetSerialNumberForProduct(string productName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "select serialnumber from product where productname = @productname";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productname", productName);

                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            return result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching serial number: " + ex.Message, "Error", MessageBoxButtons.OK);
            }

            return string.Empty;
           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO tbl_requestinstallation (productname, serialnumber, installationdate, address) " +
                                   "VALUES (@productname, @serialnumber, @installationdate, @address)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productname", cmdproduct.Text.ToString());
                        cmd.Parameters.AddWithValue("@serialnumber", txtSerialnumber.Text);
                        cmd.Parameters.AddWithValue("@installationdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        

                        if (rowsAffected > 0)
                        {
                            GetRecipientDetailsFromDatabase();
                            SendInstallationRequestEmail(cmdproduct.Text.ToString(), txtSerialnumber.Text, txtAddress.Text);
                           
                            MessageBox.Show("Request Installation", "WaterGuardPro-InstallationForm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                          
                            txtSerialnumber.Text = string.Empty;
                            txtAddress.Text = string.Empty;
                            

                        }
                        else
                        {
                            MessageBox.Show("Request not submitted");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void SendInstallationRequestEmail(string productName, string serialNumber, string address)
        {
            try
            {
                
                GetRecipientDetailsFromDatabase();


                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("mylavaramsirisha91@gmail.com", "zfkb rxac lpkl utry");
                smtpClient.EnableSsl = true;

                // Replace with the sender and recipient email addresses
                MailMessage mailMessage = new MailMessage("mylavaramsirisha91@gmail.com", UserInformation.Email);

                mailMessage.Subject = "Welcome to[ WaterGuardPro] -Water Purifier Installation Request";

                // Declare and define the 'body' variable before using it
                string body = $"Dear {UserInformation.FullName},\n"
                    + "We received a Installation Request for your Water Purifier account.Please find the detail below\n\n"

                    + $"Product: {productName}\nSerial Number: {serialNumber}\nAddress: {address}\n\n"

                    + "If you have any questions or concerns, please contact our support team at [WaterGuardPro@gmail.com Email] or call us at [1800-001-896].\n\n"

                    + "Best Regards,\n\n"
                    + "The Water Purifier Team\n"
                    + "[WaterGuardPro]\n"
                    + "[1800-001-896]";

                mailMessage.Body = body;
                smtpClient.Send(mailMessage);

                MessageBox.Show("Installation request email sent successfully.", "WaterGuardPro-Installation",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void GetRecipientDetailsFromDatabase()
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-N4O5ADP;Initial Catalog=Admin;Integrated Security=True"))
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT fullname, email, address FROM Admin WHERE username=@username", con);
                    cmd.Parameters.AddWithValue("@username", UserInformation.Username);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserInformation.Email = reader["email"].ToString();
                            UserInformation.address = reader["address"].ToString();
                            UserInformation.FullName = reader["fullname"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting user information from the database: " + ex.Message);
            }
        }
    }
    
}