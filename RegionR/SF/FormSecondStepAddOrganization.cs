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
        public FormSecondStepAddOrganization()
        {
            InitializeComponent();
            LoadSecondTable();
        }

        private void FormSecondStepAddOrganization_Load(object sender, EventArgs e)
        {
            LoadFirstTable();
        }

        private void LoadFirstTable()
        {
            UserLpuRRList userLpuList = UserLpuRRList.GetUniqueInstance();

            UserList userList = UserList.GetUniqueInstance();
            User user = userList.GetItem(globalData.UserID) as User;

            dgvLpuRR.DataSource = userLpuList.ToDataTable(user);
            dgvLpuRR.Columns[0].Visible = false;
        }

        private void LoadSecondTable()
        {
            LpuCompetitorsList lpuCompetitorsList = LpuCompetitorsList.GetUniqueInstance();

            dgvLPUCompetitors.DataSource = lpuCompetitorsList.ToDataTable();

            ResizeDGV();
        }

        private void FormSecondStepAddOrganization_Resize(object sender, EventArgs e)
        {
            ResizeDGV();
        }

        private void ResizeDGV()
        {
            dgvLPUCompetitors.Columns[0].Width = Convert.ToInt32(dgvLPUCompetitors.Width * 0.5);
            dgvLPUCompetitors.Columns[1].Width = Convert.ToInt32(dgvLPUCompetitors.Width * 0.22);
            dgvLPUCompetitors.Columns[2].Width = Convert.ToInt32(dgvLPUCompetitors.Width * 0.22);
        }
    }
}
