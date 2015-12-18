﻿using System;
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

            DataTable dt = (user.Role == Roles.Администратор) ? lpuList.ToDataTable() : lpuList.ToDataTable(user);

            dgv.DataSource = dt;

            if (dgv.Columns.Count > 0)
                dgv.Columns[0].Visible = false;
        }

        private void btnAddOrganization_Click(object sender, EventArgs e)
        {
            FormFirstStepAddOrganization formFirstStepAddOrganization = new FormFirstStepAddOrganization();
            if (formFirstStepAddOrganization.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Organization organization = Organization.CreateItem(typeOrg);
                FormSecondStepAddOrganization formSecondStepAddOrganization = new FormSecondStepAddOrganization(organization as LPU);
                if (formSecondStepAddOrganization.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FormAddLPU formAddLPU = new FormAddLPU(organization as LPU);
                    if (formAddLPU.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        LoadData();
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
        }

        private void btnDeleteOrganization_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить организацию?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                Organization organization = GetOrganization();
                organization.Delete();
                LoadData();
            }
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

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            _seacher.Find(tbSearch.Text);
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            _myStatusStrip.writeStatus();
        }

        private void fiterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;

            if (dgv.CurrentCell == null)
                return;

            string columnName = dgv.Columns[dgv.CurrentCell.ColumnIndex].HeaderText;

            Point point = new Point(dgv.CurrentCell.ColumnIndex, dgv.CurrentCell.RowIndex);

            //MyFilter myFilter = (dgv.Name == "_dgvCar") ? MyFilter.GetInstanceCars() : MyFilter.GetInstanceDrivers();
            //myFilter.SetFilterValue(string.Concat(columnName, ":"), point);
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;

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
    }
}