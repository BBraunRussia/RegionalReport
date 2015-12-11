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

        public FormSecondStepAddOrganization(LPU lpu)
        {
            InitializeComponent();

            _userLpuList = UserLpuRRList.GetUniqueInstance();
            _lpuCompetitorsList = LpuCompetitorsList.GetUniqueInstance();

            _lpu = lpu;
        }

        private void FormSecondStepAddOrganization_Load(object sender, EventArgs e)
        {
            LoadFirstTable();
            LoadSecondTable();
        }

        private void LoadFirstTable()
        {
            UserList userList = UserList.GetUniqueInstance();
            User user = userList.GetItem(globalData.UserID) as User;

            dgvLpuRR.DataSource = _userLpuList.ToDataTable(user);
            dgvLpuRR.Columns[0].Visible = false;
        }

        private void LoadSecondTable()
        {
            dgvLPUCompetitors.DataSource = _lpuCompetitorsList.ToDataTable();
            dgvLPUCompetitors.Columns[0].Visible = false;

            dgvLPUCompetitors.CurrentCell = dgvLPUCompetitors.Rows[0].Cells[1];

            ResizeDGV();
        }

        private void FormSecondStepAddOrganization_Resize(object sender, EventArgs e)
        {
            ResizeDGV();
        }

        private void ResizeDGV()
        {
            dgvLPUCompetitors.Columns[1].Width = Convert.ToInt32(dgvLPUCompetitors.Width * 0.5);
            dgvLPUCompetitors.Columns[2].Width = Convert.ToInt32(dgvLPUCompetitors.Width * 0.22);
            dgvLPUCompetitors.Columns[3].Width = Convert.ToInt32(dgvLPUCompetitors.Width * 0.22);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int idLpuCompetitor;
            int.TryParse(dgvLPUCompetitors.Rows[dgvLPUCompetitors.CurrentCell.RowIndex].Cells[0].Value.ToString(), out idLpuCompetitor);
            LpuCompetitors lpuCompetitor = _lpuCompetitorsList.GetItem(idLpuCompetitor);

            _lpu.Name = lpuCompetitor.Name;
            _lpu.INN = lpuCompetitor.INN;
            _lpu.KPP = lpuCompetitor.KPP;

            int idLpuRR;
            int.TryParse(dgvLpuRR.Rows[dgvLpuRR.CurrentCell.RowIndex].Cells[0].Value.ToString(), out idLpuRR);

            if (idLpuRR != 0)
            {
                UserLpuRR userLpuRR = _userLpuList.GetItem(idLpuRR) as UserLpuRR;
                _lpu.LpuRR = userLpuRR.LpuRR;
            }

            FormAddLPU formAddLPU = new FormAddLPU(_lpu);
            formAddLPU.ShowDialog();
        }
    }
}
