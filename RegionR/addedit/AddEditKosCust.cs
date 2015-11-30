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
    public partial class AddEditKosCust : Form
    {
        public AddEditKosCust()
        {
            InitializeComponent();

            sql sql1 = new sql();

            cbDiv.DataSource = sql1.GetRecords("exec SelSDiv");
            cbDiv.DisplayMember = "sdiv_code";
            cbDiv.ValueMember = "sdiv_id";

            cbReg.DataSource = sql1.GetRecords("exec Region_Select");
            cbReg.DisplayMember = "reg_nameRus";
            cbReg.ValueMember = "reg_id";

            cbCust.DataSource = sql1.GetRecords("exec SelCustNN");
            cbCust.DisplayMember = "cust_code";
            cbCust.ValueMember = "cust_id";

            cbRegCust.DataSource = sql1.GetRecords("exec Region_Select");
            cbRegCust.DisplayMember = "reg_nameRus";
            cbRegCust.ValueMember = "reg_id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sql sql1 = new sql();            
            sql1.GetRecords("exec InsKosCust @p1, @p2, @p3, @p4", cbReg.SelectedValue, cbCust.SelectedValue, cbRegCust.SelectedValue, cbDiv.SelectedValue);

            globalData.load = true;

            MessageBox.Show("Дистрибьютор добавлен");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
