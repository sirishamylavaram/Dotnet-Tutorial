using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace KENTPLUS_PROJECT
{
    public partial class ServicehistoryForm : Form
    {
        public ServicehistoryForm()
        {
            InitializeComponent();
        }

        private void ServicehistoryForm_Load(object sender, EventArgs e)
        {

            LoadServiceHistoryData();
        }
        private void LoadServiceHistoryData()
        {
            // Connection string
            string connectionString = "Data Source=DESKTOP-N4O5ADP;Initial Catalog=Admin;Integrated Security=True";

            // SQL query to retrieve data
            string query = "SELECT * FROM tbl_servicehistory";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    // Create a SqlDataAdapter to fetch the data into a DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    // Fill the DataTable with the data from the database
                    adapter.Fill(dataTable);

                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = dataTable;

                    connection.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Get the selected start and end dates from the DateTimePickers
            DateTime startDate = dateTimePicker1.Value;
            DateTime endDate = dateTimePicker2.Value;

            DataView dv = new DataView(((DataTable)dataGridView1.DataSource));

            // Filter the DataView based on the selected start and end dates
            dv.RowFilter = string.Format("installationdate >= #{0}# AND installationdate <= #{1}#",
                                          startDate.ToString("yyyy-MM-dd"),
                                          endDate.ToString("yyyy-MM-dd"));

            // Bind the filtered DataView back to the DataGridView
            dataGridView1.DataSource = dv;

        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            LoadServiceHistoryData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }
    }
}
