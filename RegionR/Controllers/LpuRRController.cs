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

        private Color _bbgreen3 = Color.FromArgb(115, 214, 186);

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

            SetStyle();

            return _dgv;
        }

        public void SetStyle()
        {
            SetStyleLpuUsed();

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
            _dgv.Columns[8].Width = 150;
            _dgv.Columns[9].Width = 70;
            _dgv.Columns["color"].Visible = false;
            
            SetStyle();

            return _dgv;
        }

        private void SetStyleLpuUsed()
        {
            if (!_dgv.Columns.Contains("color"))
                return;

            foreach (DataGridViewRow row in _dgv.Rows)
            {
                if (row.Cells["color"].Value.ToString().ToLower() == "false")
                    row.DefaultCellStyle.BackColor = Color.Silver;

                if (row.Cells["Использование"].Value.ToString().ToLower() == "не используется")
                {
                    if ((row.Cells["Статус"].Value.ToString() == StatusLPU.Активен.ToString()) && (string.IsNullOrEmpty(row.Cells["Сокр. название ЛПУ-SF"].Value.ToString())))
                        row.DefaultCellStyle.ForeColor = Color.Green;
                    else
                        row.Cells["Использование"].Style.ForeColor = Color.Green;
                }
            }
        }

        public void ReLoad()
        {
            _lpuRRList.Reload();
        }
    }
}
