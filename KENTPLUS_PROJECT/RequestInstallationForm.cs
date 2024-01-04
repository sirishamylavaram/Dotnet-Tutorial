using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KENTPLUS_PROJECT
{
    public partial class RequestInstallationForm : Form
    {
      

        public RequestInstallationForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            InstallationForm installationForm = new InstallationForm(UserInformation.address);
            //installationForm.UserInformationAddress = address;
            installationForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Re_installationForm re_InstallationForm = new Re_installationForm();
            re_InstallationForm.ShowDialog();
        }
    }
}
