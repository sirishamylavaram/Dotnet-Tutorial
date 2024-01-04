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
    public partial class MyproofileForm : Form
    {
        private string connectionString = "Data Source=DESKTOP-N4O5ADP;Initial Catalog=Admin;Integrated Security=True";

        public MyproofileForm()
        {
            InitializeComponent();
            LoadmyprofileData();
            UpdatemyProfileLabel();
        }

        private void UpdatemyProfileLabel()
        {
            lblmyprofile.Text = $"{UserInformation.FullName}";
        }
        private void LoadmyprofileData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "select username,email,moblie,address from Admin";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        txtusername.Text = UserInformation.Username;
                        txtemail.Text = UserInformation.Email;
                        txtmoblie.Text = UserInformation.moblie;
                        txtaddress.Text = UserInformation.address;



                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "UPDATE Admin SET address = @address WHERE username = @username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", txtusername.Text);
                        cmd.Parameters.AddWithValue("@address", txtaddress.Text);
                        

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Address updated successfully", "WaterGuardPro-Myprofile", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadmyprofileData();
                        }
                        else
                        {
                            MessageBox.Show("No records updated. User not found or address is the same.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating address and mobile number: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
        }

    }
}
