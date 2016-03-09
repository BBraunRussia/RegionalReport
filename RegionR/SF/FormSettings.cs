using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF;

namespace RegionR.SF
{
    public partial class FormSettings : Form
    {
        private Settings _settings;

        public FormSettings()
        {
            InitializeComponent();

            _settings = new Settings();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            chbEditMode.Checked = _settings.GetEditMode();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _settings.SetEditMode(chbEditMode.Checked);
        }
    }
}
