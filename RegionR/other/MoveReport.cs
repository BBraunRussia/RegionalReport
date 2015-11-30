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
    public partial class MoveReport : Form
    {
        int count = 0, idRep = 0, db = 0;

        public MoveReport(int idRep1, int db1, int count1)
        {
            InitializeComponent();

            sql sql1 = new sql();
            DataTable dt1 = sql1.GetRecords("exec SelSubRegionByUserID @p1", globalData.UserID);

            globalData.load = false;
            cbSubReg.DataSource = dt1;
            cbSubReg.DisplayMember = "subreg_nameRus";
            cbSubReg.ValueMember = "subreg_id";
            globalData.load = true;

            tbCount.Text = count1.ToString();
            count = count1;
            idRep = idRep1;
            db = db1;

            if (globalData.Div == "HC")
            {
                tbTail.Visible = true;
                label3.Visible = true;
                tbTail.Text = sql1.GetRecordsOne("exec TailForRaspNew @p1, @p2, @p3", idRep, db, cbSubReg.SelectedValue);
            }
            else
            {
                tbTail.Visible = false;
                label3.Visible = false;
            }
        }

        private void validation(KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void tbCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            validation(e);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (count < Convert.ToInt32(tbCount.Text))
            {
                MessageBox.Show("Количество не может быть больше, чем в продаже", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Convert.ToInt32(tbCount.Text) < 0)
            {
                MessageBox.Show("Количество не может быть отрицательным", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((globalData.Div == "HC") && (cbSubReg.SelectedValue.ToString() != "1"))
            {
                if (Convert.ToInt32(tbCount.Text) > Convert.ToInt32(tbTail.Text))
                {
                    MessageBox.Show("Количество не может быть больше остатка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            sql sql1 = new sql();
            string res = sql1.GetRecordsOne("exec MoveReport 0, @p1, @p2, @p3, @p4, @p5", idRep, db, cbSubReg.SelectedValue, globalData.UserID, tbCount.Text);

            if (res == "1")
            {
                globalData.update = true;
                Close();
            }
            else
                MessageBox.Show(res, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            globalData.update = false;
            Close();
        }

        private void cbSubReg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((globalData.load) && (globalData.Div == "HC"))
            {
                sql sql1 = new sql();
                tbTail.Text = sql1.GetRecordsOne("exec TailForRaspNew @p1, @p2, @p3", idRep, db, cbSubReg.SelectedValue);
            }
        }
    }
}
