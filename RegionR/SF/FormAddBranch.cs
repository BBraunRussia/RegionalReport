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
    public partial class FormAddBranch : Form
    {
        private bool _isLPU;

        public FormAddBranch(bool isLPU)
        {
            InitializeComponent();

            _isLPU = isLPU;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (rbBranch.Checked)
                FormAddLPU.typeOrg = TypeOrg.ЛПУ;
            else if (rbDepartment.Checked)
                FormAddLPU.typeOrg = TypeOrg.Отделение;
            else if (rbDivision.Checked)
                FormAddLPU.typeOrg = TypeOrg.Отдел;
            else if (rbPharmacy.Checked)
                FormAddLPU.typeOrg = TypeOrg.Аптека;
        }

        private void FormAddBranch_Load(object sender, EventArgs e)
        {
            rbBranch.Enabled = _isLPU;
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            btnNext.Enabled = true;
        }
    }
}
