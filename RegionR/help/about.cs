using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.help
{
    public partial class about : Form
    {
        public about()
        {
            InitializeComponent();

            label2.Text = "Версия - " + Application.ProductVersion;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
