using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RegionR.addedit;
using RegionR.other;
using DataLayer;

namespace RegionR.Directories
{
    public partial class UserLpuAccess : Form
    {
        public UserLpuAccess()
        {
            InitializeComponent();

            globalData.load = false;

            cbSDiv.SelectedIndex = 0;

            globalData.Div = cbSDiv.SelectedItem.ToString();

            globalData.load = true;

            fillRegion();
            fillUsers();

            loadData1();
            loadData2();
        }

        Color bbgreen3 = Color.FromArgb(115, 214, 186);
        Color bbgray4 = Color.FromArgb(150, 150, 150);

        private void fillRegion()
        {
            globalData.load = false;

            Sql sql1 = new Sql();
            DataTable dt = sql1.GetRecords("exec Region_Select '', @p1", cbSDiv.SelectedItem);

            if (dt != null)
            {
                cbRegions.DataSource = dt;
                cbRegions.DisplayMember = "reg_nameRus";
                cbRegions.ValueMember = "reg_id";
            }

            globalData.load = true;
        }

        private void fillUsers()
        {
            globalData.load = false;

            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelUsersAP @p1, @p2", cbSDiv.SelectedItem, cbRegions.SelectedValue);

            if (dt1 != null)
            {
                cbUsers.DataSource = dt1;
                cbUsers.DisplayMember = "user_name";
                cbUsers.ValueMember = "user_id";
            }

            globalData.load = true;
        }

        private void loadData1()
        {
            Sql sql1 = new Sql();
            _dgv1.DataSource = sql1.GetRecords("exec SelLPUbyRegID @p1", cbRegions.SelectedValue);
            
            _dgv1.Columns["lpu_id"].Visible = false;
            _dgv1.Columns["lpu_sname"].Width = 88;
            _dgv1.Columns["lpu_name"].Width = 335;
            

            _dgv1.Columns["lpu_sname"].HeaderText = "Сокращенное наименование";
        _dgv1.Columns["lpu_name"].HeaderText = "Полное наименование";
        }

        private void loadData2()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelLPU @p1, @p2, @p3", cbUsers.SelectedValue, cbRegions.SelectedValue, cbSDiv.SelectedItem);            

            if (_dgv2.Columns.Count == 0)
            {
                _dgv2.Columns.Add("ulpu_id", "");
                _dgv2.Columns.Add("lpu_sname", "");
                _dgv2.Columns.Add("lpu_name", "");
                _dgv2.Columns.Add("ulpu_year1", "");
                _dgv2.Columns.Add("ulpu_year2", "");

                _dgv2.Columns["ulpu_id"].Visible = false;
                _dgv2.Columns["lpu_sname"].Width = 88;
                _dgv2.Columns["lpu_name"].Width = 200;
                _dgv2.Columns["ulpu_year1"].Width = 65;
                _dgv2.Columns["ulpu_year2"].Width = 65;

                _dgv2.Columns["lpu_sname"].HeaderText = "Сокращенное наименование";
                _dgv2.Columns["lpu_name"].HeaderText = "Полное наименование";

                _dgv2.Columns["ulpu_year1"].HeaderText = "Начало отчётности";
                _dgv2.Columns["ulpu_year2"].HeaderText = "Окончание отчётности";
            }

            int i = 0;

            _dgv2.Rows.Clear();
            
            foreach (DataRow row in dt1.Rows)
            {
                _dgv2.Rows.Add(row.ItemArray);
                if(Convert.ToInt32(row.ItemArray[4].ToString()) < globalData.CurDate.Year)
                    _dgv2.Rows[i].DefaultCellStyle.BackColor = bbgray4;
                i++;
            }
        }

        private void cbSDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                globalData.Div = cbSDiv.SelectedItem.ToString();
                fillUsers();
                loadData2();
            }
        }

        private void cbRegions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                fillUsers();
                loadData1();
                loadData2();
            }
        }

        private void cbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                loadData2();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditLPU aeLPU = new AddEditLPU(cbRegions.SelectedValue.ToString());
            aeLPU.ShowDialog();
        }

        private void _dgv1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["lpu_id"].Value.ToString() == "0")
                {
                    MessageBox.Show("Для редактирования выделите ЛПУ.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                AddEditLPU aeLPU = new AddEditLPU(_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["lpu_id"].Value.ToString(), _dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["lpu_sname"].Value.ToString(), _dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["lpu_name"].Value.ToString());
                aeLPU.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Не удалось войти в режим редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LPU_Activated(object sender, EventArgs e)
        {
            if (globalData.update)
            {
                loadData2();
                globalData.update = false;
            }
        }

        private void _dgv1_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyData == Keys.Delete)
            {
                deleteLPU();
            }
            */
        }
        
        private void deleteLPU()
        {
            Sql sql1 = new Sql();

            if (globalData.UserAccess == 1)
            {
                DialogResult dr = MessageBox.Show("Удалить выделенные строки?", "Удаление ЛПУ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (DataGridViewCell cell in _dgv1.SelectedCells)
                    {
                        if (_dgv1.Rows[cell.RowIndex].Cells[0].Value.ToString() != String.Empty)
                        {
                            sql1.GetRecords("exec DelLPU @p1", _dgv1.Rows[cell.RowIndex].Cells[0].Value.ToString());
                        }
                    }
                    loadData1();
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
                    sql1.GetRecords("exec InsUserLPU 0, @p1, @p2, @p3, @p4", cbUsers.SelectedValue, _dgv1.Rows[cell.RowIndex].Cells[0].Value.ToString(), cbSDiv.SelectedItem, globalData.UserID);
                }
            }

            loadData2();
        }

        private void _dgv2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (_dgv2.Rows[_dgv2.SelectedCells[0].RowIndex].Cells["ulpu_id"].Value.ToString() == "0")
                {
                    MessageBox.Show("Для редактирования выделите ЛПУ.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                EditUserLPU eULPU = new EditUserLPU(_dgv2.Rows[_dgv2.SelectedCells[0].RowIndex].Cells["ulpu_id"].Value.ToString(), cbSDiv.SelectedItem.ToString(), cbRegions.SelectedValue.ToString(), cbUsers.SelectedValue.ToString());
                eULPU.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Не удалось войти в режим редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelUserLPU_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            foreach (DataGridViewCell cell in _dgv2.SelectedCells)
            {
                if (_dgv2.Rows[cell.RowIndex].Cells[0].Value.ToString() != String.Empty)
                {
                    sql1.GetRecords("exec DelUserLPU @p1", _dgv2.Rows[cell.RowIndex].Cells[0].Value.ToString());
                }
            }

            loadData2();
        }

        private void btnMoveSales_Click(object sender, EventArgs e)
        {
            MoveSales ms = new MoveSales(_dgv2.Rows[_dgv2.SelectedCells[0].RowIndex].Cells["ulpu_id"].Value.ToString(), _dgv2.Rows[_dgv2.SelectedCells[0].RowIndex].Cells["lpu_sname"].Value.ToString(), cbUsers.SelectedValue.ToString(), cbRegions.SelectedValue.ToString(), cbSDiv.SelectedItem.ToString());
            ms.ShowDialog();
        }
    }
}
