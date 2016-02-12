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
    public partial class ULPUforUsers : Form
    {
        public ULPUforUsers()
        {
            InitializeComponent();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load == true)
            {
                fillULPU();
                fillUsersUlpu();
            }
        }

        private void fillRegion()
        {
            globalData.load = false;

            Sql sql1 = new Sql();
            DataTable dt = sql1.GetRecords("exec Region_Select");

            if (dt != null)
            {
                cbRegionULPU.DataSource = dt;
                cbRegionULPU.DisplayMember = "reg_nameRus";
                cbRegionULPU.ValueMember = "reg_id";
            }
            cbRegionULPU.SelectedIndex = 0;
            globalData.load = true;
        }

        private void fillULPU()
        {
            globalData.load = false;

            Sql sql1 = new Sql();
            DataTable dt = sql1.GetRecords("exec SelULPUbyRegID @p1", cbRegionULPU.SelectedValue);

            if (dt != null)
            {
                cbULPU.DataSource = dt;
                cbULPU.DisplayMember = "lpu_sname";
                cbULPU.ValueMember = "lpu_id";
            }
            globalData.load = true;
        }

        private void fillUsersUlpu()
        {
            if (dataGridView1.Rows != null)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
            }

            globalData.load = false;

            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelUserByLPUID @p1", cbULPU.SelectedValue);

            if (dt1 == null)
            {
                MessageBox.Show("Ошибка!", "ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                dataGridView1.Columns.Add("user_name", "Региональный представитель");

                foreach (DataRow row in dt1.Rows)
                {
                    dataGridView1.Rows.Add(row.ItemArray[1]);
                }

                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

            }
            globalData.load = true;
        }
        
        private void ULPUforUsers_Load(object sender, EventArgs e)
        {
            fillRegion();
            fillULPU();
            fillUsersUlpu();
        }
        
        private void cbULPU_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load == true)
                fillUsersUlpu();
        }
    }
}
