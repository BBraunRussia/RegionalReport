using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RegionR.other;
using DataLayer;

namespace RegionR.addedit
{
    public partial class AddEditUser : Form
    {
        public AddEditUser(string sdiv)
        {
            InitializeComponent();

            sdiv_id = sdiv;

            tbYear1.Text = globalData.CurDate.Year.ToString();
            tbYear2.Text = globalData.CurDate.Year.ToString();

            loadData();

            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            cbRD.Enabled = false;
            label1.Enabled = false;

            rbAPRead.Checked = true;
            rbAccRead.Checked = true;
            rbMARead.Checked = true;
        }

        public AddEditUser(string uid, string urid, string sdiv)
        {
            InitializeComponent();

            sdiv_id = sdiv;

            loadData();

            user_id = uid;
            userRight_id = urid;

            if (urid == "0")
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
                groupBox4.Enabled = false;
                cbRD.Enabled = false;
                label1.Enabled = false;
            }
            
            sql sql1 = new sql();
            DataTable dt1 = sql1.GetRecords("exec SelUserByID @p1, @p2", uid, urid);
                        
            tbUserName.Text = dt1.Rows[0].ItemArray[1].ToString();
            tbLogin.Text = dt1.Rows[0].ItemArray[2].ToString();

            cbRole.SelectedValue = Convert.ToInt32(dt1.Rows[0].ItemArray[3].ToString());

            tbEmail.Text = dt1.Rows[0].ItemArray[4].ToString();

            tbYear1.Text = dt1.Rows[0].ItemArray[5].ToString();
            tbYear2.Text = dt1.Rows[0].ItemArray[6].ToString();

            switch (dt1.Rows[0].ItemArray[7].ToString())
            {
                case "0":
                    {
                        rbAPRead.Checked = true;
                        break;
                    }
                case "1":
                    {
                        rbAPEdit.Checked = true;
                        break;
                    }
            }
            switch (dt1.Rows[0].ItemArray[8].ToString())
            {
                case "0":
                    {
                        rbAccRead.Checked = true;
                        break;
                    }
                case "1":
                    {
                        rbAccEdit.Checked = true;
                        break;
                    }
            }
            switch (dt1.Rows[0].ItemArray[9].ToString())
            {
                case "0":
                    {
                        rbMARead.Checked = true;
                        break;
                    }
                case "1":
                    {
                        rbMAEdit.Checked = true;
                        break;
                    }
                case "2":
                    {
                        rbMAAddEdit.Checked = true;
                        break;
                    }
            }

            if (dt1.Rows[0].ItemArray[10].ToString() == "0")
                chbPS.Checked = false;
            else
                chbPS.Checked = true;

            if (dt1.Rows[0].ItemArray[11].ToString() == "0")
                chbDyn.Checked = false;
            else
                chbDyn.Checked = true;

            cbRD.SelectedValue = Convert.ToInt32(dt1.Rows[0].ItemArray[12].ToString());

            if (dt1.Rows[0].ItemArray[13].ToString() == "0")
                chbDismissed.Checked = false;
            else
                chbDismissed.Checked = true;
        }

        string user_id = "0";
        string userRight_id = "0";
        string sdiv_id = "0";

        private void loadData()
        {
            try
            {
                sql sql1 = new sql();

                cbRole.DataSource = sql1.GetRecords("exec SelRole");
                cbRole.DisplayMember = "role_name";
                cbRole.ValueMember = "role_id";

                cbRD.DataSource = sql1.GetRecords("exec SelRegDir @p1", sdiv_id);
                cbRD.DisplayMember = "user_name";
                cbRD.ValueMember = "user_id";
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }        

        private void btnOK_Click(object sender, EventArgs e)
        {
            sql sql1 = new sql();

            int ap;
            if (rbAPRead.Checked)
                ap = 0;
            else if (rbAPEdit.Checked)
                ap = 1;
            else
                ap = -1;

            int acc;
            if (rbAccRead.Checked)
                acc = 0;
            else if (rbAccEdit.Checked)
                acc = 1;
            else
                acc = -1;

            int ma;
            if (rbMARead.Checked)
                ma = 0;
            else if (rbMAEdit.Checked)
                ma = 1;
            else if (rbMAAddEdit.Checked)
                ma = 2;
            else
                ma = -1;

            int ps = 0;
            if (chbPS.Checked)
                ps = 1;
            int dyn = 0;
            if (chbDyn.Checked)
                dyn = 1;

            int dis = 0;
            if (chbDismissed.Checked)
                dis = 1;

            if (user_id == "0")
            {
                sql1.GetRecords("exec InsUser @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9", tbUserName.Text, tbLogin.Text, cbRole.SelectedValue, tbEmail.Text, tbYear1.Text, tbYear2.Text, ap, acc, ma);
                MessageBox.Show("Пользователь добавлен", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbUserName.Text = "";
                tbLogin.Text = "";
            }
            else
            {
                sql1.GetRecords("exec UpdUser @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15", user_id, tbUserName.Text, tbLogin.Text, cbRole.SelectedValue, tbEmail.Text, cbRD.SelectedValue, tbYear1.Text, tbYear2.Text, userRight_id, ap, acc, ma, ps, dyn, dis);
                MessageBox.Show("Информация по пользователю обновлена", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            globalData.update = true;
        }
    }
}
