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
    public partial class userManager : Form
    {
        private int rent = 0;

        public userManager()
        {
            InitializeComponent();

            loadData();
        }

        public userManager( int flag )
        {
            InitializeComponent();

            rent = 1;
            loadData2();
        }


        private void loadData()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelUsersByRole");

            _dgv1.DataSource = dt1;
            _dgv1.Columns["user_id"].Visible = false;
            _dgv1.Columns["user_name"].HeaderText = "Пользователь";
            _dgv1.Columns["user_name"].Width = _dgv1.Width;

            DataTable dt2 = new DataTable();
            dt2 = sql1.GetRecords("exec SelUsersByRole");

            globalData.load = false;
            manager.DataSource = dt2;
            manager.ValueMember = "user_id";
            manager.DisplayMember = "user_name";
            globalData.load = true;

            int managerID = Convert.ToInt32(manager.SelectedValue);

            loadUserManager(managerID);
        }
        /* добавление людей в маркетинг 1*/
        private void loadData2()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelUsersByRole");

            _dgv1.DataSource = dt1;
            _dgv1.Columns["user_id"].Visible = false;
            _dgv1.Columns["user_name"].HeaderText = "Пользователь";
            _dgv1.Columns["user_name"].Width = _dgv1.Width;

            DataTable dt2 = new DataTable();
            dt2 = sql1.GetRecords("exec SelUsersByRole");

            globalData.load = false;
            manager.DataSource = dt2;
            manager.ValueMember = "user_id";
            manager.DisplayMember = "user_name";
            globalData.load = true;

            int managerID = Convert.ToInt32(manager.SelectedValue);

            loadUserManager2(managerID);
        }

        private void btnAddUserLPU_Click(object sender, EventArgs e)
        {
            int managerID = Convert.ToInt32(manager.SelectedValue);
            int userID = Convert.ToInt32(_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells[0].Value);

            Sql sql1 = new Sql();
            if (rent == 0)
            {
                sql1.GetRecords("exec insUserManager @p1, @p2", managerID, userID);

                loadUserManager(managerID);
            }
            else
            {
                sql1.GetRecords("exec insUserRent2 @p1, @p2", managerID, userID);

                loadUserManager2(managerID);
            }
        }

        private void btnDelUserLPU_Click(object sender, EventArgs e)
        {
            int managerID = Convert.ToInt32(manager.SelectedValue);
            int userID = Convert.ToInt32(_dgv2.Rows[_dgv2.SelectedCells[0].RowIndex].Cells[0].Value);

            Sql sql1 = new Sql();
            sql1.GetRecords("exec delUserManager @p1, @p2", managerID, userID);

            loadUserManager(managerID);
        }

        private void manager_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                int managerID = Convert.ToInt32(manager.SelectedValue);
                
                if (rent == 0)
                    loadUserManager(managerID);
                else
                    loadUserManager2(managerID);
            }
        }

        private void loadUserManager(int managerID)
        {
            Sql sql1 = new Sql();
            _dgv2.DataSource = sql1.GetRecords("exec SelUserManager @p1", managerID);

            if (_dgv2 != null)
            {
                _dgv2.Columns["user_id"].Visible = false;
                _dgv2.Columns["user_name"].HeaderText = "Пользователь";
                _dgv2.Columns["user_name"].Width = _dgv2.Width;
            }
        }
        
        private void loadUserManager2(int managerID)
        {
            Sql sql1 = new Sql();
            _dgv2.DataSource = sql1.GetRecords("exec SelUserRent2 @p1", managerID);

            if (_dgv2 != null)
            {
                _dgv2.Columns["user2_id"].Visible = false;
                _dgv2.Columns["user_name"].HeaderText = "Пользователь";
                _dgv2.Columns["user_name"].Width = _dgv2.Width;
            }
        }
    }
}
