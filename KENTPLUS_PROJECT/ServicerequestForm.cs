using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace KENTPLUS_PROJECT
{
    public partial class ServicerequestForm : Form
    {
        private string connectionString = "Data Source=DESKTOP-N4O5ADP;Initial Catalog=Admin;Integrated Security=True";
        private object recipientName;



        public ServicerequestForm(string address)
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

                    string query = "select productname from servicerequest";

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
                string selectproductname = cmdproduct.Text.ToString();

                string serialnumber = GetSerialNumberForProduct(selectproductname);
                txtSerialnumber.Text = serialnumber;
                txtAddress.Text = UserInformation.address;
            }
        }


        private string GetSerialNumberForProduct(string productname)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "select serialnumber from servicerequest where productname= @productname";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productname", productname);

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
                MessageBox.Show("Error fetching serialnumber: " + ex.Message, "Error", MessageBoxButtons.OK);
            }

            return string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO tbl_servicehistory (productname, serialnumber,serviceproblem, installationdate, address,Remarks) " +
                                   "VALUES (@productname, @serialnumber,@serviceproblem, @installationdate, @address,@Remarks)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productname", cmdproduct.Text.ToString());
                        cmd.Parameters.AddWithValue("@serialnumber", txtSerialnumber.Text);
                        cmd.Parameters.AddWithValue("@serviceproblem", txtserviceproblem.Text);
                        cmd.Parameters.AddWithValue("@installationdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {

                            SendInstallationRequestEmail(cmdproduct.Text.ToString(), txtSerialnumber.Text, txtAddress.Text, txtRemarks.Text);

                            MessageBox.Show("Service Request submitted", "WaterGuardPro-ServiceRequest", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            cmdproduct.SelectedIndex = -1;
                            txtSerialnumber.Text = string.Empty;
                            txtAddress.Text = string.Empty;
                            
                        }
                        else
                        {
                            MessageBox.Show("Service Request not submitted", "WaterGuardPro-ServiceRequest", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void SendInstallationRequestEmail(string productName, string serialNumber, string address, string text)
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

                mailMessage.Subject = "Welcome to[ WaterGuardPro] - Water Purifier Service Request Account";

                // Declare and define the 'body' variable before using it
                string body = $"Dear {UserInformation.FullName},\n"
                    + "We received a Service Request for your Water Purifier account.Please find the detail below\n\n"

                    + $"Product: {productName}\nSerial Number: {serialNumber}\nAddress: {address}\n\n"

                    + "If you have any questions or concerns, please contact our support team at [WaterGuardPro@gmail.com Email] or call us at [1800-001-896].\n\n"

                    + "Best Regards,\n\n"
                    + "The Water Purifier Team\n"
                    + "[WaterGuardPro]\n"
                    + "[1800-001-896]";

                mailMessage.Body = body;

                smtpClient.Send(mailMessage);

                MessageBox.Show("Service request email sent successfully.", "WaterGuardPro-ServiceRequest", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
