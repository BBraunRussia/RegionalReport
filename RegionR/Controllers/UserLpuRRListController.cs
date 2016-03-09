using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using ClassLibrary;
using ClassLibrary.SF;
using System.Drawing;

namespace RegionR
{
    public class UserLpuRRListController : BaseOperations, IController
    {
        private DataGridView _dgv;
        private UserLpuRRList _userLpuRRList;

        Color bbgray4 = Color.FromArgb(150, 150, 150);

        public UserLpuRRListController(DataGridView dgv)
            : base(dgv)
        {
            _dgv = dgv;
            _userLpuRRList = UserLpuRRList.GetUniqueInstance();
        }

        public DataGridView ToDataGridView(SDiv sdiv)
        {
            DataTable dt = _userLpuRRList.ToDataTableWithSF(sdiv);

            return GetDataGridView(dt);
        }

        public DataGridView ToDataGridView()
        {
            DataTable dt = _userLpuRRList.ToDataTableWithSF();

            return GetDataGridView(dt);
        }

        public DataGridView ToDataGridView(User user, RegionRR regionRR, SDiv sdiv)
        {
            DataTable dt = _userLpuRRList.ToDataTable(user, regionRR, sdiv);

            _dgv.DataSource = dt;

            _dgv.Columns[0].Visible = false;
            _dgv.Columns[1].Width = 50;
            _dgv.Columns[2].Width = 88;
            _dgv.Columns[3].Width = 200;
            _dgv.Columns[4].Width = 65;
            _dgv.Columns[5].Width = 65;

            SetStyle();
                        
            return _dgv;
        }

        public void SetStyle()
        {
            foreach (DataGridViewRow row in _dgv.Rows)
            {
                if (Convert.ToInt32(row.Cells["Окончание отчётности"].Value.ToString()) < globalData.CurDate.Year)
                    row.DefaultCellStyle.BackColor = bbgray4;

                int idLPU;
                int.TryParse(row.Cells["Номер"].Value.ToString(), out idLPU);

                LpuRRList lpuRRList = LpuRRList.GetUniqueInstance();
                LpuRR lpuRR = lpuRRList.GetItem(idLPU) as LpuRR;

                if (lpuRR.StatusLPU == StatusLPU.Неактивен)
                    row.DefaultCellStyle.ForeColor = Color.Red;
            }
        }

        private DataGridView GetDataGridView(DataTable dt)
        {
            _dgv.DataSource = dt;

            _dgv.Columns[0].Width = 70;
            _dgv.Columns[1].Width = 150;
            _dgv.Columns[2].Width = 150;
            _dgv.Columns[3].Width = 100;
            _dgv.Columns[4].Width = 150;
            _dgv.Columns[5].Width = 150;
            _dgv.Columns[6].Width = 150;

            return _dgv;
        }

        public void ReLoad()
        {
            _userLpuRRList.Reload();
        }
    }
}
