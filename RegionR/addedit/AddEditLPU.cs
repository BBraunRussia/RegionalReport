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
            chbStatus.Checked = (_lpuRR.StatusLPU == StatusLPU.Активен);
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
            _lpuRR.StatusLPU = (chbStatus.Checked) ? StatusLPU.Активен : StatusLPU.Неактивен;

            return true;
        }
    }
}
