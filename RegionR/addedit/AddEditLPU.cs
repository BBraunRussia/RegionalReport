using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;
using ClassLibrary.SF;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Entities;
using RegionR.SF;
using RegionR.SF.Controllers;
using RegionR.Controllers;
using RegionReport.Domain;

namespace RegionR.addedit
{
    public partial class AddEditLPU : Form
    {
        private LpuRR lpuRR;
        private LpuList lpuList;
        
        public AddEditLPU(LpuRR lpuRR)
        {
            InitializeComponent();

            this.lpuRR = lpuRR;

            lpuList = new LpuList();
            
            loadSFLPU();
        }

        private void loadSFLPU()
        {
            IController<RegionRR> controller = new LPUController<RegionRR>();
            DataGridView dgv = controller.GetDataGridView(lpuRR.RegionRR);
            dgv.CellDoubleClick += (object sender, DataGridViewCellEventArgs e) =>
            {
                LPU lpu = lpuList.GetItem(Convert.ToInt32(dgv[0, e.RowIndex].Value));

                tbSName.Text = lpu.ShortName;
                tbName.Text = lpu.Name;
            };

            this.Controls.Add(dgv);
        }

        private void AddEditLPU_Load(object sender, EventArgs e)
        {
            tbSName.Text = lpuRR.Name;
            tbName.Text = lpuRR.FullName;
            rbActive.Checked = (lpuRR.StatusLPU == StatusLPU.Активен);
            rbNonActive.Checked = (lpuRR.StatusLPU == StatusLPU.Неактивен);
            rbGroup.Checked = (lpuRR.StatusLPU == StatusLPU.Групповой);
            lbLpuID.Text = lpuRR.ID.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (CopyFields())
            {
                lpuRR.Save();
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private bool CopyFields()
        {
            if ((tbName.Text == "") || (tbSName.Text == ""))
                return false;

            if ((rbNonActive.Checked) || (rbGroup.Checked))
            {
                LPU lpu = lpuList.GetItem(lpuRR);
                if (lpu != null)
                {
                    MessageBox.Show("Установить статус " + ((rbNonActive.Checked) ? "\"неактивен\"" : "\"групповой\"") + " нельзя - имеется сопоставление с ЛПУ-SF № " + lpu.ID, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            lpuRR.Name = tbSName.Text;
            lpuRR.FullName = tbName.Text;
            lpuRR.StatusLPU = (rbActive.Checked) ? StatusLPU.Активен : (rbNonActive.Checked) ? StatusLPU.Неактивен : StatusLPU.Групповой;

            return true;
        }
    }
}
