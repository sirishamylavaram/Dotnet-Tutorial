using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace studentmangement_project
{
    public partial class Addstudent : Form
    {
        private int student_id;

        public Addstudent(int studentId)
        {
            InitializeComponent();
            this.student_id = studentId;
            btnupdate.Enabled = true;
            LoadStudentData();  // Load data when initializing for an existing student
        }

        public Addstudent()
        {
            InitializeComponent();
        }

        private void LoadStudentData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-N4O5ADP;database=tbl_login;Trusted_Connection=true;"))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("usp_GetStudentData", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Student_id", student_id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate form fields with existing data
                                txtfirstname.Text = reader["First_name"].ToString();
                                txtlastname.Text = reader["Last_name"].ToString();
                                txtemail.Text = reader["Email"].ToString();
                                txtaddress.Text = reader["Address"].ToString();
                                txtmoblie.Text = reader["Mobile"].ToString();
                                txtcollege.Text = reader["College"].ToString();
                                txtcmbob.Text = reader["Gender"].ToString();
                                dateTimePicker1.Value = (DateTime)reader["Date_birth"];
                                dateTimePicker2.Value = (DateTime)reader["Date_joining"];
                                txtcmbob2.Text = reader["Laptop"].ToString();
                                txtpassofyear.Text = reader["PassofYear"].ToString();
                                txtanygap.Text = reader["AnyGap"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       private void btnupdate_Click(object sender, EventArgs e)
       {
    try
    {
        using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-N4O5ADP;database=tbl_login;Trusted_Connection=true;"))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand("usp_UpdateStudentData", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Use parameters to avoid SQL injection
                        cmd.Parameters.AddWithValue("@Student_id", lblstudentid.Text);
                        cmd.Parameters.AddWithValue("@First_name", txtfirstname.Text);
                        cmd.Parameters.AddWithValue("@Last_name", txtlastname.Text);
                        cmd.Parameters.AddWithValue("@Email", txtemail.Text);
                        cmd.Parameters.AddWithValue("@Gender", txtcmbob.Text);
                        cmd.Parameters.AddWithValue("@Moblie", txtmoblie.Text);
                        cmd.Parameters.AddWithValue("@Address", txtaddress.Text);
                        cmd.Parameters.AddWithValue("@Date_joining", dateTimePicker2.Text);
                        cmd.Parameters.AddWithValue("@College", txtcollege.Text);
                        cmd.Parameters.AddWithValue("@Date_birth", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@PassofYear", txtpassofyear.Text);
                        cmd.Parameters.AddWithValue("@AnyGap", txtanygap.Text);
                        cmd.Parameters.AddWithValue("@Laptop", txtcmbob2.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                conn.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show($"Student information updated successfully. {rowsAffected} row(s) affected.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally, you can reload the updated data
                    LoadStudentData();
                }
                else
                {
                    MessageBox.Show("No changes made to the student information", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
        private void btninsert_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if any of the text fields is empty
                if (string.IsNullOrWhiteSpace(txtfirstname.Text) ||
                    string.IsNullOrWhiteSpace(txtlastname.Text) ||
                    string.IsNullOrWhiteSpace(txtemail.Text) ||
                    string.IsNullOrWhiteSpace(txtaddress.Text) ||
                    string.IsNullOrWhiteSpace(txtmoblie.Text) ||
                    string.IsNullOrWhiteSpace(txtcollege.Text))
                {
                    MessageBox.Show("Please fill in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method
                }

                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-N4O5ADP;database=tbl_login;Trusted_Connection=true;"))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("usp_InsertStudentData", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Use parameters to avoid SQL injection
                        cmd.Parameters.AddWithValue("@First_name", txtfirstname.Text);
                        cmd.Parameters.AddWithValue("@Last_name", txtlastname.Text);
                        cmd.Parameters.AddWithValue("@Email", txtemail.Text);
                        cmd.Parameters.AddWithValue("@Gender", txtcmbob.Text);
                        cmd.Parameters.AddWithValue("@Moblie", txtmoblie.Text);
                        cmd.Parameters.AddWithValue("@Address", txtaddress.Text);
                        cmd.Parameters.AddWithValue("@Date_joining", dateTimePicker2.Text);
                        cmd.Parameters.AddWithValue("@College", txtcollege.Text);
                        cmd.Parameters.AddWithValue("@Date_birth", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@PassofYear", txtpassofyear.Text);
                        cmd.Parameters.AddWithValue("@AnyGap", txtanygap.Text);
                        cmd.Parameters.AddWithValue("@Laptop", txtcmbob2.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Student information saved successfully", "Addstudent-Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to save student information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtmoblie_TextChanged(object sender, EventArgs e)
        {
            string text = txtmoblie.Text;
            txtmoblie.Text = new string(text.Where(char.IsDigit).ToArray());
            txtmoblie.SelectionStart = txtmoblie.Text.Length;
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            clearfields();
        }
        private void clearfields()
        {
            lblstudentid.Text = string.Empty;
            txtfirstname.Text = string.Empty;
            txtlastname.Text = string.Empty;
            txtcmbob.Text = string.Empty;
            txtmoblie.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtcollege.Text = string.Empty;
            txtaddress.Text = string.Empty;


        }
    }

}
