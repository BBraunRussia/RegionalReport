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
    public partial class FormFirstStepAddPerson : Form
    {
        private SearchInDgv _seacher;
        private Person _person;

        public FormFirstStepAddPerson(Person person)
        {
            InitializeComponent();

            _person = person;

            _seacher = new SearchInDgv(dgv);
        }

        private void FormFirstStepAddPerson_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            LpuList lpuList = new LpuList();
            UserList userList = UserList.GetUniqueInstance();
            User user = userList.GetItem(globalData.UserID) as User;

            DataTable dt = (user.RoleSF == RolesSF.Администратор) ? lpuList.ToDataTableWithBranch() : lpuList.ToDataTableWithBranch(user);

            dgv.DataSource = dt;

            dgv.Columns[0].Visible = false;
            dgv.Columns[1].Visible = false;
            dgv.Columns[2].Width = 300;
            dgv.Columns[3].Width = 80;
            dgv.Columns[4].Width = 80;
            dgv.Columns[5].Width = 150;
            dgv.Columns[7].Visible = false;
            dgv.Columns[8].Visible = false;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            _seacher.Find(tbSearch.Text);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            SelectOrganization();
        }

        private Organization GetOrganization()
        {
            int id;
            int.TryParse(dgv.Rows[dgv.CurrentCell.RowIndex].Cells[0].Value.ToString(), out id);

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            return organizationList.GetItem(id);
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectOrganization();
        }

        private void SelectOrganization()
        {
            if (dgv.CurrentCell == null)
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            else
            {
                _person.Organization = GetOrganization();
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
