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
    public partial class FormSecondStepAddOrganization : Form
    {
        private LPU _lpu;
        private LpuCompetitorsList _lpuCompetitorsList;
        private UserLpuRRList _userLpuList;
        private SearchInDgv _seacher;
        private RealRegionList _realRegionList;

        public FormSecondStepAddOrganization(LPU lpu)
        {
            InitializeComponent();

            _userLpuList = UserLpuRRList.GetUniqueInstance();
            _realRegionList = RealRegionList.GetUniqueInstance();
            _lpuCompetitorsList = LpuCompetitorsList.GetUniqueInstance();            

            _lpu = lpu;

            _seacher = new SearchInDgv(dgvLPUCompetitors);
        }

        private void FormSecondStepAddOrganization_Load(object sender, EventArgs e)
        {
            LoadFirstTable();
            LoadSecondTable();
        }

        private void LoadFirstTable()
        {
            dgvLpuRR.DataSource = _userLpuList.ToDataTable(UserLogged.Get());
            dgvLpuRR.Columns[0].Visible = false;

            dgvLpuRR.Columns[1].Width = Convert.ToInt32(dgvLpuRR.Width / 2);
            dgvLpuRR.Columns[2].Width = Convert.ToInt32(dgvLpuRR.Width / 2);
        }

        private void LoadSecondTable()
        {
            DataTable dt = _lpuCompetitorsList.ToDataTable(UserLogged.Get());
            dgvLPUCompetitors.DataSource = dt;

            if (dt != null)
            {
                dgvLPUCompetitors.Columns[0].Visible = false;

                dgvLPUCompetitors.CurrentCell = dgvLPUCompetitors.Rows[0].Cells[1];

                dgvLPUCompetitors.Columns[2].Width = 150;
                dgvLPUCompetitors.Columns[3].Width = 100;

                ResizeDGV();
            }
        }

        private void FormSecondStepAddOrganization_Resize(object sender, EventArgs e)
        {
            ResizeDGV();
        }

        private void ResizeDGV()
        {
            dgvLPUCompetitors.Columns[1].Width = dgvLPUCompetitors.Width - dgvLPUCompetitors.Columns[2].Width - dgvLPUCompetitors.Columns[3].Width;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CopyInfo(true);
        }

        private void btnNextNotCopyLPU_Click(object sender, EventArgs e)
        {
            CopyInfo(false);
        }

        private void CopyInfo(bool isNeedCopyLPUFromCONAN)
        {
            if (isNeedCopyLPUFromCONAN)
            {
                int idLpuCompetitor;
                int.TryParse(dgvLPUCompetitors.Rows[dgvLPUCompetitors.CurrentCell.RowIndex].Cells[0].Value.ToString(), out idLpuCompetitor);
                LpuCompetitors lpuCompetitor = _lpuCompetitorsList.GetItem(idLpuCompetitor);

                _lpu.Name = lpuCompetitor.Name;
                _lpu.INN = lpuCompetitor.INN;
                _lpu.KPP = lpuCompetitor.KPP;
                _lpu.RealRegion = lpuCompetitor.RealRegion;
            }

            int idLpuRR;
            int.TryParse(dgvLpuRR.Rows[dgvLpuRR.CurrentCell.RowIndex].Cells[0].Value.ToString(), out idLpuRR);

            if (idLpuRR != 0)
            {
                UserLpuRR userLpuRR = _userLpuList.GetItem(idLpuRR) as UserLpuRR;
                _lpu.LpuRR = userLpuRR.LpuRR;
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLPUCompetitors.CurrentCell == null)
                return;

            string columnName = dgvLPUCompetitors.Columns[dgvLPUCompetitors.CurrentCell.ColumnIndex].HeaderText;

            string value = dgvLPUCompetitors.CurrentCell.Value.ToString();

            ApplyFilter(columnName, value);
        }

        private void ApplyFilter(string columnName, string value)
        {
            foreach (DataGridViewRow row in dgvLPUCompetitors.Rows)
                row.Visible = (row.Cells[columnName].Value.ToString() == value);

            btnDeleteFilter.Visible = true;
        }

        private void btnDeleteFilter_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvLPUCompetitors.Rows)
                row.Visible = true;

            btnDeleteFilter.Visible = false;
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLPUCompetitors.SelectedCells.Count == 0)
                return;

            int rowIndex = dgvLPUCompetitors.CurrentCell.RowIndex;
            int columnIndex = dgvLPUCompetitors.CurrentCell.ColumnIndex;

            DataGridViewColumn column = dgvLPUCompetitors.Columns[dgvLPUCompetitors.CurrentCell.ColumnIndex];
            System.ComponentModel.ListSortDirection sortDirection;

            if ((dgvLPUCompetitors.SortedColumn == null) || (dgvLPUCompetitors.SortedColumn != column))
                sortDirection = System.ComponentModel.ListSortDirection.Ascending;
            else if (dgvLPUCompetitors.SortOrder == SortOrder.Ascending)
                sortDirection = System.ComponentModel.ListSortDirection.Descending;
            else
                sortDirection = System.ComponentModel.ListSortDirection.Ascending;

            dgvLPUCompetitors.Sort(column, sortDirection);

            dgvLPUCompetitors.CurrentCell = dgvLPUCompetitors.Rows[rowIndex].Cells[columnIndex];
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

        private void dgvLPUCompetitors_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex >= 0))
                dgvLPUCompetitors.CurrentCell = dgvLPUCompetitors.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }
    }
}
