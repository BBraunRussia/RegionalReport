using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using ClassLibrary.SF;

namespace RegionR.SF
{
    public class UserLpuRRListController : BaseOperations, IController
    {
        private DataGridView _dgv;
        private UserLpuRRList _userLpuRRList;

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
