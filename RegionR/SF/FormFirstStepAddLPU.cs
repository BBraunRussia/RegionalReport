using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF;
using ClassLibrary;

namespace RegionR.SF
{
    public partial class FormFirstStepAddLPU : Form
    {
        OrganizationListController _organizationListController;

        public FormFirstStepAddLPU(OrganizationListController organizationListController)
        {
            InitializeComponent();

            _organizationListController = organizationListController;
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _organizationListController.SetTypeOrg((rbLPU.Checked) ? TypeOrg.ЛПУ : (rbPharmacy.Checked) ? TypeOrg.Аптека : TypeOrg.Административное_Учреждение);
        }
    }
}
