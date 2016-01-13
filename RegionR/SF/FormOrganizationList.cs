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
        public static TypeOrg typeOrg = TypeOrg.ЛПУ;
        private MyStatusStrip _myStatusStrip;
        private SearchInDgv _seacher;

        public FormOrganizationList()
        {
            InitializeComponent();

            _seacher = new SearchInDgv(dgv);
            _myStatusStrip = new MyStatusStrip(dgv, statusStrip1);
        }

        private void formOrganizationList_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            LpuList lpuList = new LpuList();
            UserList userList = UserList.GetUniqueInstance();
            User user = userList.GetItem(globalData.UserID) as User;
            
            DataTable dt = (user.RoleSF == RolesSF.Администратор) ? lpuList.ToDataTable() : lpuList.ToDataTable(user);
            
            dgv.DataSource = dt;

            dgv.Columns[0].Visible = false;
            dgv.Columns[1].Width = 80;
            dgv.Columns[2].Width = 300;
            dgv.Columns[3].Width = 80;
            dgv.Columns[4].Width = 80;
            dgv.Columns[5].Width = 120;
        }

        private void btnAddOrganization_Click(object sender, EventArgs e)
        {
            FormFirstStepAddOrganization formFirstStepAddOrganization = new FormFirstStepAddOrganization();
            if (formFirstStepAddOrganization.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Organization organization = Organization.CreateItem(typeOrg);
                if (organization is LPU)
                {
                    FormSecondStepAddOrganization formSecondStepAddOrganization = new FormSecondStepAddOrganization(organization as LPU);
                    if (formSecondStepAddOrganization.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        FormAddLPU formAddLPU = new FormAddLPU(organization as LPU);
                        if (formAddLPU.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            LoadData();
                    }
                }
                else if (organization is OtherOrganization)
                {
                    FormAddOtherOrganization formAddOtherOrganization = new FormAddOtherOrganization(organization as OtherOrganization);
                    if (formAddOtherOrganization.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        LoadData();
                }
                else
                {
                    MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditOrganization();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditOrganization();
        }

        private void EditOrganization()
        {
            Organization organization = GetOrganization();

            if (organization is LPU)
            {
                FormAddLPU FormAddLPU = new FormAddLPU(organization as LPU);
                if (FormAddLPU.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    LoadData();
            }
            else if (organization is OtherOrganization)
            {
                FormAddOtherOrganization formAddOtherOrganization = new FormAddOtherOrganization(organization as OtherOrganization);
                if (formAddOtherOrganization.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    LoadData();
            }
        }

        private void btnDeleteOrganization_Click(object sender, EventArgs e)
        {
            Organization organization = GetOrganization();

            if (ClassForForm.DeleteOrganization(organization))
                LoadData();
        }

        private Organization GetOrganization()
        {
            int id;
            int.TryParse(dgv.Rows[dgv.CurrentCell.RowIndex].Cells[0].Value.ToString(), out id);

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            return organizationList.GetItem(id);
        }

        private void NotImpliment_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            _myStatusStrip.writeStatus();
        }

        private void fiterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentCell == null)
                return;

            string columnName = dgv.Columns[dgv.CurrentCell.ColumnIndex].HeaderText;

            string value = dgv.CurrentCell.Value.ToString();

            ApplyFilter(columnName, value);
        }

        private void ApplyFilter(string columnName, string value)
        {
            foreach (DataGridViewRow row in dgv.Rows)
                row.Visible = (row.Cells[columnName].Value.ToString() == value);

            btnDeleteFilter.Visible = true;
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedCells.Count == 0)
                return;

            int rowIndex = dgv.CurrentCell.RowIndex;
            int columnIndex = dgv.CurrentCell.ColumnIndex;

            DataGridViewColumn column = dgv.Columns[dgv.CurrentCell.ColumnIndex];
            System.ComponentModel.ListSortDirection sortDirection;

            if ((dgv.SortedColumn == null) || (dgv.SortedColumn != column))
                sortDirection = System.ComponentModel.ListSortDirection.Ascending;
            else if (dgv.SortOrder == SortOrder.Ascending)
                sortDirection = System.ComponentModel.ListSortDirection.Descending;
            else
                sortDirection = System.ComponentModel.ListSortDirection.Ascending;

            dgv.Sort(column, sortDirection);

            dgv.CurrentCell = dgv.Rows[rowIndex].Cells[columnIndex];
        }

        private void cityDictionaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCityList formCityList = new FormCityList();
            formCityList.ShowDialog();
        }

        private void addPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Organization organization = GetOrganization();

            Person person = new Person();
            person.Organization = organization;

            FormSecondStepAddPerson formSecondStepAddPerson = new FormSecondStepAddPerson(person);
            if (formSecondStepAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FormAddPerson formAddPerson = new FormAddPerson(person);
                formAddPerson.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            organizationList.Reload();

            LoadData();
        }

        private void dgv_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex >= 0))
                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }

        private void btnDeleteFilter_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv.Rows)
                row.Visible = true;

            btnDeleteFilter.Visible = false;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Search();
            }
        }

        private void Search()
        {
            _seacher.Find(tbSearch.Text);
        }
    }
}
