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
    public partial class Komment : Form
    {
        public string komment;

        public Komment()
        {
            InitializeComponent();
        }

        private void Komment_Load(object sender, EventArgs e)
        {
            tbKom.Text = komment;
        }


    }
}
