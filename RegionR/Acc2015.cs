using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR
{
    public partial class Acc2015 : Form
    {
        public Acc2015()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            globalData.colReg = textBox6.Text.Trim();
            globalData.colMatCode = textBox7.Text.Trim();
            globalData.colMatName = textBox8.Text.Trim();
            globalData.colCount   = textBox9.Text.Trim();
            globalData.colLPU     = textBox10.Text.Trim();
            globalData.colLast    = textBox5.Text.Trim();
                             
            globalData.folderName = textBox3.Text.Trim();
            globalData.filePass   = textBox1.Text.Trim();
            globalData.rowBegin   = Convert.ToInt32(textBox2.Text.Trim());


            this.Close();
        }
    }
}
