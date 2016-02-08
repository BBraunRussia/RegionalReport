using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using ClassLibrary.SF;
using System.Drawing;

namespace RegionR.SF
{
    public class LpuController : BaseOperations, IController
    {
        private DataGridView _dgv;
        private LpuRRList _lpuRRList;

        public LpuController(DataGridView dgv)
            : base(dgv)
        {
            _dgv = dgv;
            _lpuRRList = LpuRRList.GetUniqueInstance();
        }

        public DataGridView ToDataGridView()
        {
            DataTable dt = GetDataTable();

            _dgv.DataSource = dt;

            _dgv.Columns[0].Width = 70;
            _dgv.Columns[1].Width = 150;
            _dgv.Columns[2].Width = 150;
            _dgv.Columns[3].Width = 100;
            _dgv.Columns[4].Width = 150;
            _dgv.Columns[5].Width = 150;
            _dgv.Columns[6].Width = 150;
            _dgv.Columns[7].Width = 70;
            _dgv.Columns[8].Visible = false;

            foreach (DataGridViewRow row in _dgv.Rows)
            {
                if (row.Cells[8].Value.ToString().ToLower() == "false")
                    row.DefaultCellStyle.BackColor = Color.Silver;
            }

            return _dgv;
        }

        public void ReLoad()
        {
            _lpuRRList.Reload();
        }

        public void ExportInExcel()
        {
            DataTable dt = GetDataTable();

            CreateExcel excel = new CreateExcel(dt);
            excel.Show();
        }

        private DataTable GetDataTable()
        {
            return _lpuRRList.ToDataTableWithLpuSF(UserLogged.Get());
        }
    }
}
