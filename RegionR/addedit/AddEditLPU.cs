using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;

namespace RegionR.addedit
{
    public partial class AddEditLPU : Form
    {
        private LpuRR _lpuRR;
        
        public AddEditLPU(LpuRR lpuRR)
        {
            InitializeComponent();

            _lpuRR = lpuRR;
        }
        
        private void AddEditLPU_Load(object sender, EventArgs e)
        {
            tbSName.Text = _lpuRR.Name;
            tbName.Text = _lpuRR.FullName;
            rbActive.Checked = (_lpuRR.StatusLPU == StatusLPU.Активен);
            rbNonActive.Checked = (_lpuRR.StatusLPU == StatusLPU.Неактивен);
            rbGroup.Checked = (_lpuRR.StatusLPU == StatusLPU.Групповой);
            lbLpuID.Text = _lpuRR.ID.ToString();
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (CopyFields())
            {
                _lpuRR.Save();
            }
        }

        private bool CopyFields()
        {
            if ((tbName.Text == "") || (tbSName.Text == ""))
                return false;

            _lpuRR.Name = tbSName.Text;
            _lpuRR.FullName = tbName.Text;
            _lpuRR.StatusLPU = (rbActive.Checked) ? StatusLPU.Активен : (rbNonActive.Checked) ? StatusLPU.Неактивен : StatusLPU.Групповой;

            return true;
        }
    }
}
