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
    public partial class Trackers : Form
    {
        public Trackers()
        {
            InitializeComponent();

            loadData();
        }

        private void loadData()
        {
            Sql sql1 = new Sql();

            _dgv1.DataSource = sql1.GetRecords("exec SelTrackers");

            _dgv1.Columns["trac_id"].Visible = false;
            _dgv1.Columns["user_id"].Visible = false;
            _dgv1.Columns["trac_number"].HeaderText = "Номер";
            _dgv1.Columns["user_name"].HeaderText = "Пользователь";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            aeTracker aeTr = new aeTracker("0", "", "0");
            aeTr.ShowDialog();
        }

        private void _dgv1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            aeTracker aeTr = new aeTracker(_dgv1[0, e.RowIndex].Value.ToString(), _dgv1[1, e.RowIndex].Value.ToString(), _dgv1[2, e.RowIndex].Value.ToString());
            aeTr.ShowDialog();
        }

        private void Trackers_Activated(object sender, EventArgs e)
        {
            if (globalData.update)
            {
                loadData();
                globalData.update = false;
            }
        }
    }
}
