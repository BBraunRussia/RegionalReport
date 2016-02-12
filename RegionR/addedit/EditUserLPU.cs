using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataLayer;

namespace RegionR.addedit
{
    public partial class EditUserLPU : Form
    {
        public EditUserLPU(string ulpu, string sdiv, string reg, string user)
        {
            InitializeComponent();

            ulpu_id = ulpu;

            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelUserLPUByID @p1", ulpu);

            tbReg.Text = dt1.Rows[0].ItemArray[1].ToString();
            tbLPU.Text = dt1.Rows[0].ItemArray[2].ToString();
            tbYear1.Text = dt1.Rows[0].ItemArray[3].ToString();
            tbYear2.Text = dt1.Rows[0].ItemArray[4].ToString();

            dt1 = sql1.GetRecords("exec SelUsersAP @p1, @p2", sdiv, reg);
            cbUsers.DataSource = dt1;
            cbUsers.DisplayMember = "user_name";
            cbUsers.ValueMember = "user_id";

            cbUsers.SelectedValue = user;
        }

        string ulpu_id;

        private void btnOK_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();
            string res = sql1.GetRecordsOne("exec UpdUserLPU @p1, @p2, @p3, @p4", ulpu_id, tbYear1.Text, tbYear2.Text, cbUsers.SelectedValue);

            if (res == "1")
            {
                globalData.update = true;
                Close();
            }
            else
            {
                MessageBox.Show("Перемещение не возможно, так как " + res, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }
    }
}
