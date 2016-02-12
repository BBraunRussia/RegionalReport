using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.Directories
{
    public partial class UserRent : Form
    {
        Color bbgreen3 = Color.FromArgb(115, 214, 186);
        Color bbgreen1 = Color.FromArgb(0, 180, 130);
        Color bbgray4 = Color.FromArgb(150, 150, 150);
        Color bbgray5 = Color.FromArgb(230, 230, 230);

        
         public UserRent()
         {
             InitializeComponent();

             dateRent.Value = globalData.CurDate;

             loadRD();
             loadData();
             loadUserBC();
      
         }

         private void loadRD()
         {
            Sql sql1 = new Sql();
            DataTable dt = new DataTable();
             
            if ((globalData.UserAccess == 1) || (globalData.UserID == 6) || (globalData.UserAccess == 13))
                dt = sql1.GetRecords("exec SelRegDir");
            else if (globalData.UserAccess == 4)
                dt = sql1.GetRecords("exec SelUserByID @p1", globalData.UserID);
            

             fillComboBox(dt, comboBox1, "user_name", "user_id");
         }

         private void fillComboBox(DataTable dt1, ComboBox cb, string Display, string Value)
         {
             globalData.load = false;
             cb.DataSource = dt1;
             cb.DisplayMember = Display;
             cb.ValueMember = Value;
             cb.SelectedIndex = 0;
             globalData.load = true;
         }


        private void loadData()
        {

            Sql sql1 = new Sql();

            _dgv1.DataSource = sql1.GetRecords("exec Rent_Select_UserRent @p1, @p2, @p3, @p4, @p5", 3, 0, 0, comboBox1.SelectedValue.ToString(), "0");
                
            if (_dgv1.DataSource != null)
            {
                _dgv1.Columns["user_id"].Visible = false;
                _dgv1.Columns["user_name"].HeaderText = "Пользователь";
                _dgv1.Columns["user_name"].Width = _dgv1.Width;
            }
        }

        private void loadUserBC()
        {
            if (_dgv2.RowCount > 0)
                _dgv2.Rows.Clear();

            Sql sql1 = new Sql();

            DataTable dt = sql1.GetRecords("exec Rent_Select_UserRent @p1, @p2, @p3, @p4, @p5", 0, 0, 0, comboBox1.SelectedValue.ToString(), "0");
                
            if (dt == null)
                return;

            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                _dgv2.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2]);

                if (Convert.ToInt32(row.ItemArray[2]) == 0)
                    _dgv2.Rows[i].DefaultCellStyle.BackColor = bbgray4;
                i++;
            }

            _dgv2.Columns["user_id"].Visible = false;
            _dgv2.Columns["user_name"].HeaderText = "Пользователь";
            _dgv2.Columns["user_name"].Width = _dgv2.Width;
            _dgv2.Columns["flag_enable"].Visible = false;
                  
        }

        private void btnAddUserBC_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells[0].Value);

            Sql sql1 = new Sql();

            sql1.GetRecords("exec Rent_Insert_UserRent @p1, @p2", userID, dateRent.Value);

            loadUserBC();
            loadData();
        }

        private void btnDelUserBC_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(_dgv2.Rows[_dgv2.SelectedCells[0].RowIndex].Cells[0].Value);

            Sql sql1 = new Sql();

            sql1.GetRecords("exec Rent_Delect_UserRent @p1", userID);

            loadUserBC();
            loadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();
                        
            sql1.GetRecords("exec Rent_Update_UrtDate @p1", dateRent.Value.Year.ToString() + dateRent.Value.Month.ToString() + "01" );

            MessageBox.Show("Дата отчета успешно установлена!", "Результат");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load == true)
            {
                loadData();
                loadUserBC();
            }
        }

        private void сделатьНеактивнымToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            string res = sql1.GetRecordsOne("exec Rent_Update_UserEnable @p1", _dgv2["user_id", _dgv2.CurrentCell.RowIndex].Value.ToString());

            if (res != "0")
            {
                MessageBox.Show("Успешно!", "Результат");
                if (res == "2")
                    _dgv2.Rows[_dgv2.CurrentCell.RowIndex].DefaultCellStyle.BackColor = bbgray4;
                else
                    _dgv2.Rows[_dgv2.CurrentCell.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
            }
            else
                MessageBox.Show("Не удалось!", "Ошибка");

           

        }

        private void _dgv2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
        }

        private void _dgv2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView dgv = sender as DataGridView;
                DataGridView.HitTestInfo hi = dgv.HitTest(e.X, e.Y);

                if (hi.Type == DataGridViewHitTestType.Cell)
                {
                    contextMenuStrip1.Show(MousePosition, ToolStripDropDownDirection.Right);
                    сделатьНеактивнымToolStripMenuItem.Visible = true;
                }
            }
        }

    }
}
