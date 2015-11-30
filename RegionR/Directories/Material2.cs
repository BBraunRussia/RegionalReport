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
    public partial class Material2 : Form
    {
        public Material2()
        {
            InitializeComponent();

            sql sql1 = new sql();
            DataTable dt1 = sql1.GetRecords("exec SelCustMat2");

            globalData.load = false;
            cbCustomer.DataSource = dt1;
            cbCustomer.DisplayMember = "cust_name";
            cbCustomer.ValueMember = "cust_id";
            globalData.load = true;

            if (cbCustomer.Items.Count > 0)
                loadData();
        }

        private void loadData()
        {
            sql sql1 = new sql();

            _dgv1.DataSource = sql1.GetRecords("exec SelMaterial2 @p1", cbCustomer.SelectedValue);

            format();
        }

        private void format()
        {
            _dgv1.Columns["mat2_id"].Visible = false;

            _dgv1.Columns["mat2_code"].HeaderText = "Артикул";
            _dgv1.Columns["mat2_name"].HeaderText = "Название";
            _dgv1.Columns["mat2_count"].HeaderText = "Количество шт в уп";
            _dgv1.Columns["mat_code"].HeaderText = "Артикул в RR";
            _dgv1.Columns["mat_name"].HeaderText = "Название в RR";

            _dgv1.Columns["mat2_code"].Width = 100;
            _dgv1.Columns["mat2_name"].Width = 300;
            _dgv1.Columns["mat2_count"].Width = 100;
            _dgv1.Columns["mat_code"].Width = 100;
            _dgv1.Columns["mat_name"].Width = 300;

        }

        private void FindMat()
        {
            if (tbMat.Text == "")
            {
                ss1.Items.Clear();
                ss1.Items.Add("Введите артикул");
                ss1.Items[0].ForeColor = Color.Red;
                timer1.Enabled = true;
                return;
            }
            foreach (DataGridViewRow row in _dgv1.Rows)
            {
                if (row.Cells[1].Value.ToString() == tbMat.Text)
                {
                    _dgv1.ClearSelection();
                    _dgv1.Rows[row.Index].Cells[1].Selected = true;
                    _dgv1.CurrentCell = _dgv1.Rows[row.Index].Cells[1];
                    return;
                }
            }
            ss1.Items.Clear();
            ss1.Items.Add("Артикул не найден");
            ss1.Items[0].ForeColor = Color.Red;
            timer1.Enabled = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FindMat();
        }

        private void tbMat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                FindMat();
        }

        private void cbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
                loadData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ss1.Items.Clear();
            timer1.Enabled = false;
        }
    }
}
