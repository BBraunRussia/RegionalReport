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
    public partial class Name : Form
    {
        public Name()
        {
            InitializeComponent();
        }

        public Name(String sel1, String ins1, String upd1)
        {
            InitializeComponent();
            ins = ins1;
            upd = upd1;
            sel = sel1;
        }
        String ins, upd, sel;

        private void button1_Click(object sender, EventArgs e)
        {
            AEName ae = new AEName(ins);
            ae.ShowDialog();
        }

        private void Regions_Activated(object sender, EventArgs e)
        {            
            Sql sql1 = new Sql();
            dataGridView1.DataSource = sql1.GetRecords(sel);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 200;
            //dataGridView1.FirstDisplayedScrollingRowIndex = clsData.selRow;
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AEName ae = new AEName(dataGridView1.CurrentRow.Cells[0].Value.ToString(), dataGridView1.CurrentRow.Cells[1].Value.ToString(), upd);
            ae.ShowDialog();
            //clsData.selRow = dataGridView1.SelectedRows[0].Index;
        }
    }
}
