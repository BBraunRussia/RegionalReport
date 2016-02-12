using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RegionR.addedit;
using DataLayer;

namespace RegionR.Directories
{
    public partial class KosCust : Form
    {
        public KosCust()
        {
            InitializeComponent();

            Sql sql1 = new Sql();

            cbReg.DataSource = sql1.GetRecords("exec Region_Select");
            cbReg.DisplayMember = "reg_nameRus";
            cbReg.ValueMember = "reg_id";

            cbDiv.DataSource = sql1.GetRecords("exec SelSDiv");
            cbDiv.DisplayMember = "sdiv_code";
            cbDiv.ValueMember = "sdiv_id";

            load = true;

            loadKosCust();
        }

        bool load = false;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditKosCust aekc = new AddEditKosCust();
            aekc.ShowDialog();
        }

        private void KosCust_Activated(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                loadKosCust();
                globalData.load = false;
            }
        }

        private void loadKosCust()
        {
            Sql sql1 = new Sql();
            dataGridView1.DataSource = sql1.GetRecords("exec SelKosCustByID @p1, @p2", cbReg.SelectedValue, cbDiv.SelectedValue);

            dataGridView1.Columns["KosCust_id"].Visible = false;
            dataGridView1.Columns["cust_id"].Visible = false;
            dataGridView1.Columns["reg_id"].Visible = false;

            dataGridView1.Columns["cust_code"].HeaderText = "Номер";
            dataGridView1.Columns["cust_name"].HeaderText = "Дистрибьютор";
            dataGridView1.Columns["reg_nameRus"].HeaderText = "Регион дистрибьютора";

            dataGridView1.Columns["cust_code"].Width = 100;
            dataGridView1.Columns["cust_name"].Width = 200;
            dataGridView1.Columns["reg_nameRus"].Width = 200;

            dataGridView1.Columns["cust_code"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dataGridView1.Columns["cust_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dataGridView1.Columns["reg_nameRus"].SortMode = DataGridViewColumnSortMode.Programmatic;
        }

        private void cbReg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(load)
                loadKosCust();
        }

        private void cbDiv_SelectedValueChanged(object sender, EventArgs e)
        {
            if(load)
                loadKosCust();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();
            sql1.GetRecords("exec DelKosCust @p1", dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

            loadKosCust();
        }
    }
}
