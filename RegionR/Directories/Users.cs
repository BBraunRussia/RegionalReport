using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RegionR.addedit;
using DataLayer;

namespace RegionR.Directories
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();

            cbSDiv.SelectedIndex = 0;

            globalData.load = false;

            globalData.Div = cbSDiv.SelectedItem.ToString();

            Sql sql1 = new Sql();
            DataTable dt = sql1.GetRecords("exec SelRole");

            if (dt != null)
            {
                cbRoles.DataSource = dt;
                cbRoles.DisplayMember = "role_name";
                cbRoles.ValueMember = "role_id";
                cbRoles.SelectedIndex = 4;
            }

            dt = sql1.GetRecords("exec Region_Select");
            if (dt != null)
            {
                cbRegions.DataSource = dt;
                cbRegions.DisplayMember = "reg_nameRus";
                cbRegions.ValueMember = "reg_id";
            }

            loadData1();
            loadData2();

            globalData.load = true;
        }

        Color bbgray4 = Color.FromArgb(150, 150, 150);

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditUser aeu = new AddEditUser(cbSDiv.SelectedItem.ToString());
            aeu.ShowDialog();
        }

        private void EditUser(DataGridView dgv)
        {
            string uid = dgv.Rows[dgv.SelectedCells[0].RowIndex].Cells["user_id"].Value.ToString();

            string urid = "0";

            if(dgv.Name == "_dgv2")
                urid = dgv.Rows[dgv.SelectedCells[0].RowIndex].Cells["ur_id"].Value.ToString();

            globalData.update = false;

            AddEditUser aeu = new AddEditUser(uid, urid, cbSDiv.SelectedItem.ToString());
            aeu.ShowDialog();            
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EditUser(_dgv1);
        }

        private void Users_Activated(object sender, EventArgs e)
        {
            if (globalData.update)
            {   
                loadData1();
                loadData2();
            }
        }

        private void loadData1()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            if (_dgv1.Columns.Count == 0)
            {
                _dgv1.Columns.Add("user_id", "");
                _dgv1.Columns.Add("user_name", "ФИО");
                _dgv1.Columns.Add("user_year1", "");
                _dgv1.Columns.Add("user_year2", "");

                _dgv1.Columns["user_id"].Visible = false;
                _dgv1.Columns["user_name"].Width = 220;
                _dgv1.Columns["user_year1"].Visible = false;
                _dgv1.Columns["user_year2"].Visible = false;
            }

            dt1 = sql1.GetRecords("exec SelUsers2 @p1", cbRoles.SelectedValue);

            int i = 0;

            _dgv1.Rows.Clear();
            
            foreach (DataRow row in dt1.Rows)
            {
                _dgv1.Rows.Add(row.ItemArray);
                if(Convert.ToInt32(row.ItemArray[3].ToString()) < globalData.CurDate.Year)
                    _dgv1.Rows[i].DefaultCellStyle.BackColor = bbgray4;
                i++;
            }
        }

        private void loadData2()
        {
            if (_dgv2.Columns.Count == 0)
            {
                _dgv2.Columns.Add("user_id", "");
                _dgv2.Columns.Add("user_name", "ФИО");
                _dgv2.Columns.Add("ur_id", "");
                _dgv2.Columns.Add("role_name", "Роль");
                _dgv2.Columns.Add("user_year1", "");
                _dgv2.Columns.Add("user_year2", "");

                _dgv2.Columns["user_id"].Visible = false;
                _dgv2.Columns["ur_id"].Visible = false;
                _dgv2.Columns["user_name"].Width = 150;
                _dgv2.Columns["role_name"].Width = 200;
                _dgv2.Columns["user_year1"].Visible = false;
                _dgv2.Columns["user_year2"].Visible = false;
            }

            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelUsers2 @p1, @p2, @p3", cbRoles.SelectedValue, cbRegions.SelectedValue, cbSDiv.SelectedItem);

            int i = 0;

            _dgv2.Rows.Clear();
            if (dt1 != null)
            {
                foreach (DataRow row in dt1.Rows)
                {
                    _dgv2.Rows.Add(row.ItemArray);
                    if (Convert.ToInt32(row.ItemArray[5].ToString()) < globalData.CurDate.Year)
                        _dgv2.Rows[i].DefaultCellStyle.BackColor = bbgray4;
                    i++;
                }
            }
        }

        private void btnAddUserLPU_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            foreach (DataGridViewCell cell in _dgv1.SelectedCells)
            {
                if (_dgv1.Rows[cell.RowIndex].Cells[0].Value.ToString() != String.Empty)
                {
                    sql1.GetRecords("exec InsUserRight @p1, @p2, @p3", _dgv1.Rows[cell.RowIndex].Cells[0].Value.ToString(), cbRegions.SelectedValue, cbSDiv.SelectedItem);
                }
            }

            loadData2();
        }

        private void _dgv2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditUser(_dgv2);
        }

        private void cbRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
                loadData1();
        }

        private void cbSDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
                loadData2();
        }

        private void cbRegions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
                loadData2();
        }

        private void btnDelUserLPU_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            foreach (DataGridViewCell cell in _dgv2.SelectedCells)
            {
                if (_dgv2.Rows[cell.RowIndex].Cells[0].Value.ToString() != String.Empty)
                {
                    sql1.GetRecords("exec DelUserRight @p1", _dgv2.Rows[cell.RowIndex].Cells["ur_id"].Value.ToString());
                }
            }

            loadData2();
        }
    }
}
