using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataLayer;

namespace RegionR.addedit
{
    public partial class AddEditLPU : Form
    {
        string idLPU;
        string idReg;

        public AddEditLPU(string reg_id)
        {
            InitializeComponent();

            idLPU = "0";
            idReg = reg_id;
        }

        public AddEditLPU(string lpu_id, string sname, string name)
        {
            InitializeComponent();

            tbSName.Text = sname;
            tbName.Text = name;

            idReg = "0";
            idLPU = lpu_id;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((tbName.Text != "") && (tbSName.Text != ""))
            {
                sql sql1 = new sql();

                if (idLPU != "0")
                    sql1.GetRecords("exec UpdLPU @p1, @p2, @p3", idLPU, tbSName.Text, tbName.Text);
                else
                    sql1.GetRecords("exec InsLPU @p1, @p2, @p3", tbSName.Text, tbName.Text, idReg);

                globalData.update = true;
            }
        }
    }
}
