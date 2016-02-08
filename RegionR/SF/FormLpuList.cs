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

namespace RegionR
{
    public partial class FormLpuList : Form
    {
        private MyStatusStrip _myStatusStrip;

        private LpuController _lpuController;

        public FormLpuList()
        {
            InitializeComponent();

            _myStatusStrip = new MyStatusStrip(dgv, statusStrip1);

            _lpuController = new LpuController(dgv);

            btnShowLPUForEdit.Visible = (UserLogged.Get().RoleSF == ClassLibrary.SF.RolesSF.Администратор);
        }

        private void FormOrganizationWithLpuRRList_Load(object sender, EventArgs e)
        {
            LoadData();

            WriteStatus();
        }
        
        private void WriteStatus()
        {
            _myStatusStrip.writeStatus();
        }

        private void LoadData()
        {
            dgv = _lpuController.ToDataGridView();
        }

        private void NotImpliment_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDeleteFilter_Click(object sender, EventArgs e)
        {
            _lpuController.DeleteFilter();

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
            _lpuController.Search(tbSearch.Text);
        }

        private void dgv_Sorted(object sender, EventArgs e)
        {
            _lpuController.ApplyFilter();
        }

        private void fiterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _lpuController.CreateFilter();

            btnDeleteFilter.Visible = true;
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _lpuController.Sort();
        }

        private void btnExportInExcel_Click(object sender, EventArgs e)
        {
            _lpuController.ExportInExcel();
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
    }
}
