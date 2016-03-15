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
using ClassLibrary;

namespace RegionR
{
    public partial class FormOrganizationList : Form
    {
        private MyStatusStrip _myStatusStrip;

        private OrganizationListController _organizationListController;

        public FormOrganizationList()
        {
            InitializeComponent();

            _myStatusStrip = new MyStatusStrip(dgv, statusStrip1);

            _organizationListController = new OrganizationListController(dgv);

            settingsToolStripMenuItem.Visible = (UserLogged.Get().RoleSF == RolesSF.Администратор);
            importToolStripMenuItem.Visible = (UserLogged.Get().RoleSF == RolesSF.Администратор);
            exportToolStripMenuItem.Visible = (UserLogged.Get().RoleSF == RolesSF.Администратор);
            toolStripMenuItem4.Visible = (UserLogged.Get().RoleSF == RolesSF.Администратор);
            toolStripMenuItem5.Visible = (UserLogged.Get().RoleSF == RolesSF.Администратор);
        }

        private void formOrganizationList_Load(object sender, EventArgs e)
        {
            LoadData();

            SetEnabledComponent();
        }

        private void LoadData()
        {
            dgv = _organizationListController.ToDataGridView();
        }

        private void SetEnabledComponent()
        {
            ControlEditMode controlEditMode = new ControlEditMode(this.Controls, btnReload);
            controlEditMode.SetEnableValue(btnContinue, true);
            controlEditMode.SetEnableValue(btnDeleteFilter, true);
            controlEditMode.SetEnableValue(tbSearch, true);
            controlEditMode.SetEnableValue(dgv, true);
            controlEditMode.SetEnableValue(menuStrip1, true);
            addOrganizationToolStripMenuItem.Enabled = controlEditMode.IsEditMode();
            deleteToolStripMenuItem.Enabled = controlEditMode.IsEditMode();
            addPersonToolStripMenuItem.Enabled = controlEditMode.IsEditMode();
            addPersonToolStripMenuItem1.Enabled = controlEditMode.IsEditMode();
        }

        private void btnAddOrganization_Click(object sender, EventArgs e)
        {
            if (_organizationListController.AddOrganization())
                LoadData();
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex < 0) || (e.RowIndex < 0))
                return;

            EditOrganization();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditOrganization();
        }

        private void EditOrganization()
        {
            if (_organizationListController.EditOrganization())
                LoadData();
        }

        private void btnDeleteOrganization_Click(object sender, EventArgs e)
        {
            if (_organizationListController.DeleteOrganization())
                LoadData();
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
            _organizationListController.CreateFilter();

            btnDeleteFilter.Visible = true;
        }

        private void btnDeleteFilter_Click(object sender, EventArgs e)
        {
            _organizationListController.DeleteFilter();

            btnDeleteFilter.Visible = false;
        }

        private void dgv_Sorted(object sender, EventArgs e)
        {
            _organizationListController.ApplyFilter();
        }
        
        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _organizationListController.Sort();
        }

        private void cityDictionaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCityList formCityList = new FormCityList();
            formCityList.ShowDialog();
        }

        private void addPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _organizationListController.AddPerson();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            _organizationListController.ReLoad();

            LoadData();
        }

        private void dgv_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex >= 0))
                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
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
            _organizationListController.Search(tbSearch.Text);
        }
        
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings();
            formSettings.ShowDialog();
        }

        private void excelAllFieldsRusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportOrganization exportOrganization = new ExportOrganization();
            exportOrganization.Export();
        }
    }
}
