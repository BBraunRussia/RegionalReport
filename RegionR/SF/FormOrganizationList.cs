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
        private MyStatusStrip _myStatusStrip;

        private OrganizationListController _organizationListController;

        public FormOrganizationList()
        {
            InitializeComponent();

            _myStatusStrip = new MyStatusStrip(dgv, statusStrip1);

            _organizationListController = new OrganizationListController(dgv);
        }

        private void formOrganizationList_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgv = _organizationListController.ToDGV();
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

        private void button1_Click(object sender, EventArgs e)
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
    }
}
