using ClassLibrary;
using ClassLibrary.SF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.SF
{
    public partial class FormUserLpuRRList : Form
    {
        private MyStatusStrip _myStatusStrip;

        private UserLpuRRListController _userLpuRRListController;

        public FormUserLpuRRList()
        {
            InitializeComponent();

            _myStatusStrip = new MyStatusStrip(dgv, statusStrip1);

            _userLpuRRListController = new UserLpuRRListController(dgv);
        }

        private void FormUserLpuRRList_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void WriteStatus()
        {
            _myStatusStrip.writeStatus();
        }

        private void cbSDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            int id = Convert.ToInt32(cbSDiv.SelectedIndex);

            if (id > 0)
            {
                SDiv sdiv = (SDiv)id;

                dgv = _userLpuRRListController.ToDataGridView(sdiv);
            }
            else
            {
                dgv = _userLpuRRListController.ToDataGridView();
            }

            WriteStatus();
        }

        private void btnDeleteFilter_Click(object sender, EventArgs e)
        {
            _userLpuRRListController.DeleteFilter();

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
            _userLpuRRListController.Search(tbSearch.Text);
        }

        private void dgv_Sorted(object sender, EventArgs e)
        {
            _userLpuRRListController.ApplyFilter();
        }

        private void fiterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _userLpuRRListController.CreateFilter();

            btnDeleteFilter.Visible = true;
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _userLpuRRListController.Sort();
        }

        private void btnExportInExcel_Click(object sender, EventArgs e)
        {
            _userLpuRRListController.ExportInExcel();
        }

        private void dgv_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex >= 0))
                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }
    }
}
