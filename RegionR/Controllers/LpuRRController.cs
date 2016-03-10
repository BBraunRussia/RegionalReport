using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using ClassLibrary;
using ClassLibrary.SF;

namespace RegionR
{
    public class LpuRRController : BaseOperations, IController
    {
        private DataGridView _dgv;
        private LpuRRList _lpuRRList;

        public LpuRRController(DataGridView dgv)
            : base(dgv)
        {
            _dgv = dgv;
            _lpuRRList = LpuRRList.GetUniqueInstance();
        }
        
        public DataGridView ToDataGridView(RegionRR regionRR)
        {
            DataTable dt = _lpuRRList.ToDataTable(regionRR);

            _dgv.DataSource = dt;

            _dgv.Columns[0].Width = 50;
            _dgv.Columns[1].Width = 80;
            _dgv.Columns[2].Width = 260;
            _dgv.Columns[3].Width = 80;

            SetTextColor();

            return _dgv;
        }

        public void SetTextColor()
        {
            foreach (DataGridViewRow row in _dgv.Rows)
            {
                if (row.Cells["Статус"].Value.ToString() == StatusLPU.Неактивен.ToString())
                    row.DefaultCellStyle.ForeColor = Color.Red;
                else if (row.Cells["Статус"].Value.ToString() == StatusLPU.Групповой.ToString())
                    row.DefaultCellStyle.ForeColor = Color.Blue;
            }
        }
        
        public DataGridView ToDataGridView()
        {
            DataTable dt = _lpuRRList.ToDataTableWithLpuSF(UserLogged.Get());

            _dgv.DataSource = dt;

            _dgv.Columns[0].Width = 70;
            _dgv.Columns[1].Width = 150;
            _dgv.Columns[2].Width = 150;
            _dgv.Columns[3].Width = 100;
            _dgv.Columns[4].Width = 150;
            _dgv.Columns[5].Width = 150;
            _dgv.Columns[6].Width = 150;
            _dgv.Columns[7].Width = 150;
            _dgv.Columns[8].Width = 70;
            _dgv.Columns["color"].Visible = false;

            foreach (DataGridViewRow row in _dgv.Rows)
            {
                if (row.Cells["color"].Value.ToString().ToLower() == "false")
                    row.DefaultCellStyle.BackColor = Color.Silver;
            }

            SetTextColor();

            return _dgv;
        }

        public void ReLoad()
        {
            _lpuRRList.Reload();
        }
    }
}
