using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.other
{
    public partial class ViewText : Form
    {        
        public ViewText(string text, string name)
        {
            InitializeComponent();

            tbViewText.Text = text;
            tbViewText.ReadOnly = true;
            this.Text = name;
        }
    }
}
