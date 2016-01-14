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
        private User _user;

        public FormSecondStepAddOrganization(LPU lpu)
        {
            InitializeComponent();

            _userLpuList = UserLpuRRList.GetUniqueInstance();
            _lpuCompetitorsList = LpuCompetitorsList.GetUniqueInstance();

            _lpu = lpu;

            UserList userList = UserList.GetUniqueInstance();
            _user = userList.GetItem(globalData.UserID) as User;

            _seacher = new SearchInDgv(dgvLPUCompetitors);
        }

        private void FormSecondStepAddOrganization_Load(object sender, EventArgs e)
        {
            LoadFirstTable();
            LoadSecondTable();
        }

        private void LoadFirstTable()
        {
            dgvLpuRR.DataSource = _userLpuList.ToDataTable(_user);
            dgvLpuRR.Columns[0].Visible = false;

            dgvLpuRR.Columns[1].Width = Convert.ToInt32(dgvLpuRR.Width / 2);
            dgvLpuRR.Columns[2].Width = Convert.ToInt32(dgvLpuRR.Width / 2);
        }

        private void LoadSecondTable()
        {
            DataTable dt = _lpuCompetitorsList.ToDataTable(_user);
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
                _lpu.RegionCompetitors = lpuCompetitor.RegionCompetitors;
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
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
