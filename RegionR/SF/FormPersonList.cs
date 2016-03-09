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
    public partial class FormPersonList : Form
    {
        private MyStatusStrip _myStatusStrip;
        private PersonListController _personListController;

        public FormPersonList(Organization organization = null)
        {
            InitializeComponent();

            if (organization != null)
                this.Text = string.Concat("Справочник: Персоны-SF, Организация: ", organization.ShortName);

            
            _myStatusStrip = new MyStatusStrip(dgv, statusStrip1);

            _personListController = new PersonListController(dgv, organization);
        }
        
        private void FormPersonList_Load(object sender, EventArgs e)
        {
            LoadData();

            SetEnabledComponent();
        }

        private void LoadData()
        {
            dgv = _personListController.ToDataGridView();
        }

        private void SetEnabledComponent()
        {
            ControlEditMode controlEditMode = new ControlEditMode(this.Controls, btnRefresh);
            controlEditMode.SetEnableValue(btnContinue, true);
            controlEditMode.SetEnableValue(btnDeleteFilter, true);
            controlEditMode.SetEnableValue(tbSearch, true);
            controlEditMode.SetEnableValue(menuStrip1, true);
            addToolStripMenuItem.Enabled = controlEditMode.IsEditMode();
            deleteToolStripMenuItem.Enabled = controlEditMode.IsEditMode();
        }

        private void NotImpliment_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            _myStatusStrip.writeStatus();
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_personListController.Add())
                LoadData();
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex < 0) || (e.RowIndex < 0))
                return;

            EditPerson();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditPerson();
        }

        private void EditPerson()
        {
            if (_personListController.EditPerson())
                LoadData();
        }
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_personListController.DeletePerson())
                LoadData();
        }
        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDGV();
        }

        public void RefreshDGV()
        {
            _personListController.ReLoad();

            LoadData();
        }

        private void dgv_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex >= 0))
                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _personListController.Sort();
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _personListController.CreateFilter();

            btnDeleteFilter.Visible = true;
        }
        
        private void btnDeleteFilter_Click(object sender, EventArgs e)
        {
            _personListController.DeleteFilter();

            btnDeleteFilter.Visible = false;
        }

        private void dgv_Sorted(object sender, EventArgs e)
        {
            _personListController.ApplyFilter();
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
            _personListController.Search(tbSearch.Text);
        }
    }
}
