using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF;

namespace RegionR
{
    public partial class FormUserRoleSFList : Form
    {
        public FormUserRoleSFList()
        {
            InitializeComponent();
        }

        private void FormUserRoleSFList_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            UserRoleSFList userRoleSFList = UserRoleSFList.GetUniqueInstance();

            DataTable dt = userRoleSFList.ToDataTable();

            dgv.DataSource = dt;

            ResizeDGV();
        }

        public void ResizeDGV()
        {
            dgv.Columns[0].Width = dgv.Width / 2;
            dgv.Columns[1].Width = dgv.Width / 2;
        }
    }
}
