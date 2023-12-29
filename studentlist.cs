using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace studentmangement_project
{
    public partial class studentlist : Form
    {
        private object selectAllChecked;

        public studentlist()
        {
            InitializeComponent();
            btnclear.Click += btnclear_Click;
            LoadStudentData(); 
        }

        private void LoadStudentData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-N4O5ADP;database=tbl_login;Trusted_Connection=true;"))
                {
                    connection.Open();

                    // Assuming your table name is "YourStudentTable"
                    string query = "SELECT * FROM studenttbl";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Set the DataGridView's DataSource to the DataTable
                        dataGridViewStudent.DataSource = dataTable;
                        lblTotalRecords.Text =": " + dataTable.Rows.Count;
                        lblTotalRecords.ForeColor = Color.DarkGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-N4O5ADP;database=tbl_login;Trusted_Connection=true;"))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dataGridViewStudent.Rows)
                    {
                        DataGridViewCheckBoxCell chk = row.Cells["Select"] as DataGridViewCheckBoxCell;

                        if (chk != null && chk.Value != null && (bool)chk.Value)
                        {
                            int id = Convert.ToInt32(row.Cells["student_id"].Value);

                            using (SqlCommand cmd = new SqlCommand("usp_deletestddata", connection))
                            {
                                cmd.CommandType = CommandType.StoredProcedure; // Assuming it's a stored procedure

                                cmd.Parameters.AddWithValue("@std_id", id);

                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // If you want to remove the row from the DataGridView after deletion
                                    dataGridViewStudent.Rows.Remove(row);

                                    MessageBox.Show($"Student with ID {id} deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show($"Failed to delete student with ID {id}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }

                    // Reload the student data after deletion
                    LoadStudentData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting students: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewStudent_MouseClick(object sender, MouseEventArgs e)
        {

            setstudentvalues(e);

        }

        private void dataGridViewStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       private void btnsearch_Click(object sender, EventArgs e)
       {
    try
    {
        // Check if the search term is empty
        if (string.IsNullOrWhiteSpace(txtsearch.Text))
        {
            MessageBox.Show("Please enter a search term.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return; // Exit the method if the search term is empty
        }

        using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-N4O5ADP;database=tbl_login;Trusted_Connection=true;"))
        {
            connection.Open();

            using (SqlCommand cmd = new SqlCommand("SearchStudents", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@searchTerm", txtsearch.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable studenttbl = new DataTable();
                adapter.Fill(studenttbl);

                // Set the DataGridView's DataSource to the DataTable
                dataGridViewStudent.DataSource = studenttbl;
               lblTotalRecords.Text = ":" + studenttbl.Rows.Count;
               lblTotalRecords.ForeColor = Color.Red;
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error searching students: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

        private void setstudentvalues(MouseEventArgs e)
        {
            if (dataGridViewStudent.Rows.Count > 0)
            {
                if (dataGridViewStudent.SelectedRows.Count > 0)
                {
                    DataGridViewRow dr = dataGridViewStudent.SelectedRows[0];
                    this.Hide();
                    Addstudent addstudent = new Addstudent();
                    addstudent.lblstudentid.Text = dr.Cells["Student_id"].Value.ToString();
                    addstudent.txtfirstname.Text = dr.Cells["First_name"].Value.ToString();
                    addstudent.txtlastname.Text = dr.Cells["Last_name"].Value.ToString();
                    addstudent.txtcmbob.Text = dr.Cells["Gender"].Value.ToString();
                    addstudent.txtcollege.Text = dr.Cells["College"].Value.ToString();
                    addstudent.txtemail.Text = dr.Cells["Email"].Value.ToString();
                    addstudent.txtmoblie.Text = dr.Cells["Moblie"].Value.ToString();
                    addstudent.txtaddress.Text = dr.Cells["Address"].Value.ToString();
                    addstudent.dateTimePicker1.Text = dr.Cells["Date_birth"].Value.ToString();
                    addstudent.dateTimePicker2.Text = dr.Cells["Date_joining"].Value.ToString();
                    addstudent.txtanygap.Text = dr.Cells["AnyGap"].Value.ToString();
                    addstudent.txtpassofyear.Text = dr.Cells["PassofYear"].Value.ToString();
                    addstudent.txtcmbob2.Text = dr.Cells["Laptop"].Value.ToString();
                    addstudent.btnupdate.Enabled = true;
                    addstudent.ShowDialog();
                }
            }
        }

        private void dataGridViewStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridViewStudent_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewStudent.Rows)
            {
                DataGridViewCheckBoxCell checkBoxCell = row.Cells["select"] as DataGridViewCheckBoxCell;

                if (checkBoxCell != null && !Convert.ToBoolean(checkBoxCell.Value))
                {
                    int id = Convert.ToInt32(row.Cells[0].Value);
                }

            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
           
            txtsearch.Text = "";

           Selectall.Checked = false;

            LoadStudentData();
        }

        private void studentlist_Load(object sender, EventArgs e)
        {
           
        }

        private void Selectall_CheckedChanged_1(object sender, EventArgs e)
        {
            bool selectAllChecked = Selectall.Checked;

            foreach (DataGridViewRow row in dataGridViewStudent.Rows)
            {
                DataGridViewCheckBoxCell chk = row.Cells["Select"] as DataGridViewCheckBoxCell;

                if (chk != null)
                {
                    chk.Value = selectAllChecked;
                }
            }
        }

    }
}


