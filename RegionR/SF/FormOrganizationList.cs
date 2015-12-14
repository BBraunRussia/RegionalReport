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
            LpuList lpuList = new LpuList();
            UserList userList = UserList.GetUniqueInstance();
            User user = userList.GetItem(globalData.UserID) as User;

            dgv.DataSource = lpuList.ToDataTable(user);
            if (dgv.Columns.Count > 0)
                dgv.Columns[0].Visible = false;
        }

        private void btnAddOrganization_Click(object sender, EventArgs e)
        {
            FormFirstStepAddOrganization formFirstStepAddOrganization = new FormFirstStepAddOrganization();
            formFirstStepAddOrganization.ShowDialog();
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int idOrganization;
            int.TryParse(dgv.Rows[dgv.CurrentCell.RowIndex].Cells[0].Value.ToString(), out idOrganization);

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            Organization organization = organizationList.GetItem(idOrganization);

            if (organization is LPU)
            {
                FormAddLPU FormAddLPU = new FormAddLPU(organization as LPU);
                FormAddLPU.ShowDialog();
            }
        }
    }
}
