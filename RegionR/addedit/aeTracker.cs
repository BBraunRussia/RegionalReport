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
    public partial class aeTracker : Form
    {
        string tracID = "0";

        public aeTracker(string tracID1, string tracName, string userID)
        {
            InitializeComponent();

            tracID = tracID1;

            sql sql1 = new sql();
            DataTable dt1 = sql1.GetRecords("exec SelUsersForTracker");

            fillComboBox(dt1, cbUser, "user_name", "user_id");

            if (tracID != "0")
            {
                tbNumber.Text = tracName;
                cbUser.SelectedValue = userID;
            }
        }

        private void fillComboBox(DataTable dt, ComboBox cb, string Display, string Value)
        {
            globalData.load = false;
            cb.DataSource = dt;
            cb.DisplayMember = Display;
            cb.ValueMember = Value;
            globalData.load = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            sql sql1 = new sql();

            string res = sql1.GetRecordsOne("exec InsTracker @p1, @p2, @p3", tracID, tbNumber.Text, cbUser.SelectedValue);

            if (res != "1")
                MessageBox.Show(res, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                globalData.update = true;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            globalData.update = false;
            Close();
        }
    }
}
