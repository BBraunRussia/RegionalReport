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
    public partial class CreateUserMessage : Form
    {
        public CreateUserMessage()
        {
            InitializeComponent();
        }

        private void cbEnd_CheckedChanged(object sender, EventArgs e)
        {
            lbDate.Enabled = !cbEnd.Checked;
            dtpDate.Enabled = !cbEnd.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!(cbRP.Checked) && !(cbRM.Checked) && !(cbRD.Checked))
            {
                MessageBox.Show("Не выбраны получатели", "Отсутствуют получатели", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            sql sql1 = new sql();

            DateTime date = dtpDate.Value;

            if (cbEnd.Checked)
                date = Convert.ToDateTime("2023-01-01");

            if (cbRP.Checked)
                sql1.GetRecords("exec InsMessageByRoleID @p1, @p2, @p3, @p4, @p5", globalData.UserID, 5, tbHeader.Text, tbText.Text, date);
            if (cbRM.Checked)
                sql1.GetRecords("exec InsMessageByRoleID @p1, @p2, @p3, @p4, @p5", globalData.UserID, 6, tbHeader.Text, tbText.Text, date);
            if (cbRD.Checked)
                sql1.GetRecords("exec InsMessageByRoleID @p1, @p2, @p3, @p4, @p5", globalData.UserID, 4, tbHeader.Text, tbText.Text, date);

            this.Close();
        }
    }
}
