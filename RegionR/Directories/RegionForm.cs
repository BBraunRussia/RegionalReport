using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;
using RegionR.addedit;

namespace RegionR.Directories
{
    public partial class RegionForm : Form
    {
        public RegionForm()
        {
            InitializeComponent();

            loadRegion();
        }

        private void loadRegion()
        {
            dgvRegion.DataSource = Regions.getDataTable();

            formatRegion();
        }

        private void formatRegion()
        {
            dgvRegion.Columns[0].Visible = false;

            dgvRegion.Columns["reg_nameRus"].HeaderText = "Название";
            dgvRegion.Columns["reg_code"].HeaderText = "Код";
            dgvRegion.Columns["reg_name"].HeaderText = "Название по-английски";
        }

        private void add_Click(object sender, EventArgs e)
        {
            RegionAddEdit regAE = new RegionAddEdit();
            regAE.ShowDialog();
        }

        private void RegionForm_Activated(object sender, EventArgs e)
        {
            if (globalData.update)
            {
                loadRegion();
                globalData.update = false;
            }
        }

        private void dgvRegion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (isHeader(e.RowIndex))
                return;

            int idReg = Convert.ToInt32(dgvRegion.Rows[e.RowIndex].Cells[0].Value);

            Regions reg = new Regions(idReg);

            RegionAddEdit regAE = new RegionAddEdit(reg);
            regAE.ShowDialog();
        }

        private bool isHeader(int rowIndex)
        {
            return rowIndex < 0;
        }
    }
}
