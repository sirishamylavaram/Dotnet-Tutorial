using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace KENTPLUS_PROJECT
{
    public partial class MainForm : Form
    {
        public object Userinformation { get; private set; }

        public MainForm()
        {
            InitializeComponent();
           
            UpdateWelcomeLabel();
        }

       

        private void UpdateWelcomeLabel()
        {
           
            lblWelcome.Text = $"Welcome to WaterGuardPro App";
            lblWelcome1.Text = $"Hi";
            lblwelcome2.Text = $"{UserInformation.FullName} !";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //this.Hide();
            InstallationForm InstallationForm = new InstallationForm(UserInformation.address);
            InstallationForm.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //this.Hide();
            ServicerequestForm servicerequestForm = new ServicerequestForm(UserInformation.address); 
            servicerequestForm.ShowDialog();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            
            ServicehistoryForm servicehistoryForm = new ServicehistoryForm();
            servicehistoryForm.ShowDialog();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            //this.Hide();
            MyproofileForm myproofile = new MyproofileForm();
            myproofile.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            //this.Hide();
            MyProductForm myProductForm = new MyProductForm();
            myProductForm.ShowDialog();
        }
    }
}
