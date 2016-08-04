using ClassLibrary;
using DataLayer;
using RegionR.other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.HT
{
    public partial class HT_Add_LPU : Form
    {
        private LpuRRController _lpuRRController;
        private LpuRRController _lpuHTController;

        private RegionRRList _regionRRList;
        private LpuRRList _lpuRRList;
        private LpuRRList _lpuHTList;
        
        Color bbgreen3 = Color.FromArgb(115, 214, 186);
        Color bbgray4 = Color.FromArgb(150, 150, 150);

        public HT_Add_LPU(bool flag_edit = true)
        {
            InitializeComponent();

            _regionRRList = RegionRRList.GetUniqueInstance();
            _lpuRRList = LpuRRList.GetUniqueInstance();
            _lpuHTList = LpuRRList.GetUniqueInstance();
            
            _lpuRRController = new LpuRRController(_dgv1);
            _lpuHTController = new LpuRRController(_dgv2);

            btnAddUserLPU.Visible = flag_edit;
            btnDelUserLPU.Visible = flag_edit;
            
            globalData.update = false;
            fillRegion();
            globalData.update = true;
            loadData1();
            loadData2();  
        }

        private void btnAddUserLPU_Click(object sender, EventArgs e)
        {
            LpuRR lpuRR = GetLpuRR();

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec HT_Insert_LPU @p1", lpuRR.ID);         

            loadData2();
        }

        private void btnDelUserLPU_Click(object sender, EventArgs e)
        {
            LpuRR lpuHT = GetLpuHT();

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec HT_Delete_LPU @p1", lpuHT.ID);

            loadData2();
        }

        private void cbRegions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
            {                
                loadData1();
                loadData2();                    
            }
        }

        private void loadData1()
        {
            RegionRR regionRR = GetRegionRR();

            _dgv1 = _lpuRRController.ToDataGridView(regionRR);
        }

        private void loadData2()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec HT_Select_LPU @p1", cbRegions.SelectedValue);

            _dgv2.DataSource = dt1;
                  
        }

        private void fillRegion()
        {
            globalData.load = false;

            DataTable dt = _regionRRList.ToDataTable();

            cbRegions.DataSource = dt;
            cbRegions.DisplayMember = dt.Columns[1].ColumnName;
            cbRegions.ValueMember = dt.Columns[0].ColumnName;

            globalData.load = true;
        }

        private RegionRR GetRegionRR()
        {
            int idRegionRR;
            int.TryParse(cbRegions.SelectedValue.ToString(), out idRegionRR);
            return _regionRRList.GetItem(idRegionRR) as RegionRR;
        }

        private LpuRR GetLpuRR()
        {
            int idLPU;
            int.TryParse(_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out idLPU);
            return _lpuRRList.GetItem(idLPU) as LpuRR;
        }

        private LpuRR GetLpuHT()
        {
            int idLPU;
            int.TryParse(_dgv2.Rows[_dgv2.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out idLPU);
            return _lpuHTList.GetItem(idLPU) as LpuRR;
        }

        private void _dgv1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.RowIndex < 0) || (e.ColumnIndex < 0))
                return;

            LpuRR lpuRR = GetLpuRR();

            InputDialog ind = new InputDialog("Номер ЛПУ", "Введите номер главного ЛПУ", true);
            ind.ShowDialog();

            if (ind.DialogResult == DialogResult.Cancel)
                return;

            String lpuID = globalData.input;
            
            Sql sql1 = new Sql();
            
            sql1.GetRecords("exec HT_Update_LPU @p1, @p2", lpuRR.ID, lpuID);

            loadData1();
            loadData2();
        }
    }
}
