using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.addedit
{
    public partial class Merge : Form
    {
        public Merge()
        {
            InitializeComponent();
        }

        private void btnSearchMat_Click(object sender, EventArgs e)
        {            
            Sql sql1 = new Sql();

            DataTable dt = sql1.GetRecords("exec SelMergeMat @p1, @p2, @p3", textBox1.Text, textBox2.Text, textBox3.Text);

            _dgvMerge.DataSource = dt;
           
        }

        private void btnSaveMat_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            DataTable dt = sql1.GetRecords("exec InsMergeMat @p1, @p2, @p3", textBox1.Text, textBox2.Text, textBox3.Text);
        }

        private void btnSearchCust_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            DataTable dt = sql1.GetRecords("exec SelMergeCust @p1, @p2", textBox6.Text, textBox5.Text);

            _dgvMerge.DataSource = dt;
        }

        private void btnSaveCust_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            DataTable dt = sql1.GetRecords("exec InsMergeCust @p1, @p2", textBox6.Text, textBox5.Text);

        }
    }
}
