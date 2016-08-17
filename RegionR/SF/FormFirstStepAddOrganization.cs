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
using RegionReport.Domain;

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
            TypeOrg typeOrg;

            if (rbLPU.Checked)
            {
                typeOrg = TypeOrg.ЛПУ;
            }
            else if (rbPharmacy.Checked)
            {
                typeOrg = TypeOrg.Аптека;
            }
            else if (rbAdminOrganization.Checked)
            {
                typeOrg = TypeOrg.Административное_Учреждение;
            }
            else if (rbVeterinary.Checked)
            {
                typeOrg = TypeOrg.Ветеренарная_клиника;
            }
            else if (rbDenistry.Checked)
            {
                typeOrg = TypeOrg.Стоматология;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            _organizationListController.SetTypeOrg(typeOrg);
        }
    }
}
