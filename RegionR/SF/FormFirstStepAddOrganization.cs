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
        public FormFirstStepAddOrganization()
        {
            InitializeComponent();
        }

        private void FormFirstStepAddOrganization_Load(object sender, EventArgs e)
        {

        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                LPU lpu = Organization.CreateItem(TypeOrg.ЛПУ) as LPU;

                FormSecondStepAddOrganization formSecondStepAddOrganization = new FormSecondStepAddOrganization(lpu);
                formSecondStepAddOrganization.ShowDialog();
                Close();
            }
        }
    }
}
