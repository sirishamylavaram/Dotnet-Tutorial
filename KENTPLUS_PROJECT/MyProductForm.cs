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
    public partial class MyProductForm : Form
    {
        public MyProductForm()
        {
            InitializeComponent();
        }

        private void MyProductForm_Load(object sender, EventArgs e)
        {
            LoadMyProductData();
        }

        private void LoadMyProductData()
        {
            string connectionString = "Data Source=DESKTOP-N4O5ADP;Initial Catalog=Admin;Integrated Security=True";
            string query = "SELECT * FROM Myproducts";

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
    }
}
