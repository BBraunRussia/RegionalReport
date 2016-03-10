using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RegionR.SF;
using RegionR.Directories;
using ClassLibrary;

namespace RegionR
{
    public partial class FormLpuList : Form
    {
        private MyStatusStrip _myStatusStrip;

        private IController _userLpuRRListController;
        private IController _lpuRRController;

        private IController _controller;

        private Color _bbgreen3 = Color.FromArgb(115, 214, 186);

        public FormLpuList()
        {
            InitializeComponent();

            _myStatusStrip = new MyStatusStrip(dgv, statusStrip1);

            _lpuRRController = new LpuRRController(dgv);
            _userLpuRRListController = new UserLpuRRListController(dgv);

            _controller = _lpuRRController;

            btnShowLPUForEdit.Visible = (UserLogged.Get().RoleSF == RolesSF.Администратор);
        }

        private void FormOrganizationWithLpuRRList_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void LoadData()
        {
            dgv = _controller.ToDataGridView();

            WriteStatus();
        }

        private void WriteStatus()
        {
            _myStatusStrip.writeStatus();
        }

        private void NotImpliment_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDeleteFilter_Click(object sender, EventArgs e)
        {
            _controller.DeleteFilter();

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
            _controller.Search(tbSearch.Text);
        }

        private void dgv_Sorted(object sender, EventArgs e)
        {
            _controller.ApplyFilter();

            _controller.SetStyle();

            WriteStatus();
        }

        private void fiterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controller.CreateFilter();

            btnDeleteFilter.Visible = true;

            WriteStatus();
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controller.Sort();
        }

        private void btnExportInExcel_Click(object sender, EventArgs e)
        {
            _controller.ExportInExcel();
        }

        private void btnShowLPUForEdit_Click(object sender, EventArgs e)
        {
            UserLpuAccess userLpuAccess = new UserLpuAccess();
            userLpuAccess.ShowDialog();
        }

        private void dgv_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex >= 0))
                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }

        private void btnShowUserLPU_Click(object sender, EventArgs e)
        {
            btnShowUserLPU.BackColor = (btnShowUserLPU.BackColor == Color.Transparent) ? _bbgreen3 : Color.Transparent;

            _controller = (btnShowUserLPU.BackColor == _bbgreen3) ? _userLpuRRListController : _lpuRRController;

            btnDeleteFilter.Visible = false;

            LoadData();
        }
    }
}
