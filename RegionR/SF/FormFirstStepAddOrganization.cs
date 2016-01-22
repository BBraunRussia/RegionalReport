using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF;

namespace RegionR.SF
{
    public partial class FormFirstStepAddOrganization : Form
    {
        OrganizationListController _organizationListController;

        public FormFirstStepAddOrganization(OrganizationListController organizationListController)
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
