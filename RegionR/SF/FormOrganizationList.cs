using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF;
using RegionR.SF;

namespace RegionR
{
    public partial class FormOrganizationList : Form
    {
        public FormOrganizationList()
        {
            InitializeComponent();
        }

        private void formOrganizationList_Load(object sender, EventArgs e)
        {
            LPUList lpuList = LPUList.GetUniqueInstance();

            dgv.DataSource = lpuList.ToDataTable();
        }

        private void btnAddOrganization_Click(object sender, EventArgs e)
        {
            FormFirstStepAddOrganization formFirstStepAddOrganization = new FormFirstStepAddOrganization();
            formFirstStepAddOrganization.ShowDialog();
        }
    }
}
