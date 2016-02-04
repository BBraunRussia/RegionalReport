using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using ClassLibrary.SF;

namespace RegionR.SF
{
    public class OrganizationWithLpuRRController : BaseOperations, IController
    {
        private DataGridView _dgv;
        private LpuRRList _lpuRRList;

        public OrganizationWithLpuRRController(DataGridView dgv)
            : base(dgv)
        {
            _dgv = dgv;
            _lpuRRList = LpuRRList.GetUniqueInstance();
        }

        public DataGridView ToDataGridView()
        {
            DataTable dt = GetDataTable();

            _dgv.DataSource = dt;

            _dgv.Columns[1].Width = 100;
            _dgv.Columns[2].Width = 80;
            _dgv.Columns[3].Width = 100;
            _dgv.Columns[4].Width = 80;

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
            return _lpuRRList.ToDataTableWithLpuSF();
        }
    }
}
