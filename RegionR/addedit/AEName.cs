using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DataLayer;

namespace RegionR.addedit
{
    public partial class AEName : Form
    {
        public AEName(String ins1)
        {
            InitializeComponent();
            this.Text = "Добавление";
            ins = ins1;
        }

        public AEName(String id, String name, String upd1)
        {
            InitializeComponent();
            textBox1.Text = name;
            this.Text = "Изменение";
            mid = id;
            upd = upd1;
        }

        String mid, upd, ins;

        private void button1_Click(object sender, EventArgs e)
        {
            sql sql1 = new sql();

            if (this.Text == "Добавление")
                sql1.GetRecords(ins + " @p1 ", textBox1.Text);
            else
                sql1.GetRecords(upd + " @p1, @p2 ", textBox1.Text, mid);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
