using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataLayer;

namespace RegionR.Directories
{
    public partial class userBC : Form
    {
        public userBC()
        {
            InitializeComponent();

            loadData();
            loadUserBC();
        }

        private void loadData()
        {
            sql sql1 = new sql();

            _dgv1.DataSource = sql1.GetRecords("exec SelUsersByRole");
            _dgv1.Columns["user_id"].Visible = false;
            _dgv1.Columns["user_name"].HeaderText = "Пользователь";
            _dgv1.Columns["user_name"].Width = _dgv1.Width;
        }

        private void loadUserBC()
        {
            sql sql1 = new sql();

            _dgv2.DataSource = sql1.GetRecords("exec selUserBC");
            _dgv2.Columns["user_id"].Visible = false;
            _dgv2.Columns["user_name"].HeaderText = "Пользователь";
            _dgv2.Columns["user_name"].Width = _dgv2.Width;
        }

        private void btnAddUserBC_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells[0].Value);

            sql sql1 = new sql();

            sql1.GetRecords("exec insUserBC @p1", userID);

            loadUserBC();
        }

        private void btnDelUserBC_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(_dgv2.Rows[_dgv2.SelectedCells[0].RowIndex].Cells[0].Value);

            sql sql1 = new sql();

            sql1.GetRecords("exec delUserBC @p1", userID);

            loadUserBC();
        }
    }
}
