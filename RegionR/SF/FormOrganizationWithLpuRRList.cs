using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.SF
{
    public partial class FormOrganizationWithLpuRRList : Form
    {
        private MyStatusStrip _myStatusStrip;

        private OrganizationWithLpuRRController _organizationWithLpuRRController;

        public FormOrganizationWithLpuRRList()
        {
            InitializeComponent();

            _myStatusStrip = new MyStatusStrip(dgv, statusStrip1);

            _organizationWithLpuRRController = new OrganizationWithLpuRRController(dgv);
        }

        private void FormOrganizationWithLpuRRList_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgv = _organizationWithLpuRRController.ToDataGridView();
        }

        private void NotImpliment_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDeleteFilter_Click(object sender, EventArgs e)
        {
            _organizationWithLpuRRController.DeleteFilter();

            btnDeleteFilter.Visible = false;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Search();
            }
        }

        private void Search()
        {
            _organizationWithLpuRRController.Search(tbSearch.Text);
        }

        private void dgv_Sorted(object sender, EventArgs e)
        {
            _organizationWithLpuRRController.ApplyFilter();
        }

        private void fiterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _organizationWithLpuRRController.CreateFilter();

            btnDeleteFilter.Visible = true;
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _organizationWithLpuRRController.Sort();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            _organizationWithLpuRRController.ReLoad();

            LoadData();
        }

        private void btnExportInExcel_Click(object sender, EventArgs e)
        {
            
        }
    }
}
