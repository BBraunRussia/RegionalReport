using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataLayer;

namespace RegionR.other
{
    public partial class MoveSales : Form
    {
        string user_id, reg_id, sdiv_id, ulpu_id;

        public MoveSales(string ulpu_id1, string lpu_name, string user_id1, string reg_id1, string sdiv_id1)
        {
            InitializeComponent();

            lbLPU.Text = lpu_name;

            ulpu_id = ulpu_id1;
            user_id = user_id1;
            reg_id = reg_id1;
            sdiv_id = sdiv_id1;

            sql sql1 = new sql();

            cbUserLPU.DataSource = sql1.GetRecords("exec SelLPU @p1, @p2, @p3", user_id, reg_id, sdiv_id);
            cbUserLPU.DisplayMember = "lpu_sname";
            cbUserLPU.ValueMember = "ulpu_id";
            cbUserLPU.SelectedIndex = 0;
        }

        private void btnMoveSales_Click(object sender, EventArgs e)
        {
            sql sql1 = new sql();
            sql1.GetRecords("exec MoveSales @p1, @p2", ulpu_id, cbUserLPU.SelectedValue);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
