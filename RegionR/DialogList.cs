using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RegionR
{
    public partial class DialogList : Form
    {
        public DialogList()
        {
            InitializeComponent();
        }
        public DialogList(DataTable dt1, String NameCol, String lb)
        {
            InitializeComponent();
        }
    }
}
