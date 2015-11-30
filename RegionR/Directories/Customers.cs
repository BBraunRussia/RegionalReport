using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using RegionR.addedit;
using DataLayer;


namespace RegionR.Directories
{
    public partial class Customers : Form
    {
        Color bbgreen3 = Color.FromArgb(115, 214, 186);
        int new1 = 0;

        public Customers()
        {
            InitializeComponent();

            if (globalData.UserAccess == 1)
                btnAdd.Visible = true;

            loadData();
        }

        private void loadData()
        {
            setButtonColor();

            sql sql1 = new sql();
            DataTable dt1 = sql1.GetRecords("exec SelCustomers @p1", new1);

            if (dt1.Rows.Count > 0)
            {
                SortOrder s1 = SortOrder.None;
                SortOrder s2 = SortOrder.None;
                SortOrder s3 = SortOrder.None;
                SortOrder s4 = SortOrder.None;
                SortOrder s5 = SortOrder.None;
                SortOrder s6 = SortOrder.None;

                if (_dgv1.Columns.Count > 0)
                {
                    s1 = _dgv1.Columns["cust_code"].HeaderCell.SortGlyphDirection;
                    s2 = _dgv1.Columns["cust_name"].HeaderCell.SortGlyphDirection;
                    s3 = _dgv1.Columns["cust_ShipTo"].HeaderCell.SortGlyphDirection;
                    s4 = _dgv1.Columns["cust_payer"].HeaderCell.SortGlyphDirection;
                    s5 = _dgv1.Columns["cust_plant"].HeaderCell.SortGlyphDirection;
                    s6 = _dgv1.Columns["cust_DistChan"].HeaderCell.SortGlyphDirection;
                }

                _dgv1.DataSource = dt1;

                if (s1 != SortOrder.None)
                {
                    _dgv1.Columns["cust_code"].HeaderCell.SortGlyphDirection = s1;
                    if (s1 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["cust_code"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["cust_code"], ListSortDirection.Descending);
                }

                if (s2 != SortOrder.None)
                {
                    _dgv1.Columns["cust_name"].HeaderCell.SortGlyphDirection = s2;
                    if (s2 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["cust_name"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["cust_name"], ListSortDirection.Descending);
                }

                if (s3 != SortOrder.None)
                {
                    _dgv1.Columns["cust_ShipTo"].HeaderCell.SortGlyphDirection = s3;
                    if (s3 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["cust_ShipTo"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["cust_ShipTo"], ListSortDirection.Descending);
                }

                if (s4 != SortOrder.None)
                {
                    _dgv1.Columns["cust_payer"].HeaderCell.SortGlyphDirection = s4;
                    if (s4 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["cust_payer"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["cust_payer"], ListSortDirection.Descending);
                }

                if (s5 != SortOrder.None)
                {
                    _dgv1.Columns["cust_plant"].HeaderCell.SortGlyphDirection = s5;
                    if (s5 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["cust_plant"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["cust_plant"], ListSortDirection.Descending);
                }

                if (s6 != SortOrder.None)
                {
                    _dgv1.Columns["cust_DistChan"].HeaderCell.SortGlyphDirection = s6;
                    if (s6 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["cust_DistChan"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["cust_DistChan"], ListSortDirection.Descending);
                }

                format();
            }
        }

        private void setButtonColor()
        {
            if (new1 == 0)
            {
                btnAll.BackColor = bbgreen3;
                btnNew.BackColor = Control.DefaultBackColor;
            }
            else
            {
                btnAll.BackColor = Control.DefaultBackColor;
                btnNew.BackColor = bbgreen3;   
            }
        }

        private void format()
        {
            _dgv1.Columns["cust_id"].Visible = false;
            _dgv1.Columns["reg_id"].Visible = false;

            _dgv1.Columns["cust_code"].HeaderText = "Номер";
            _dgv1.Columns["cust_name"].HeaderText = "Название";
            _dgv1.Columns["cust_ShipTo"].HeaderText = "ShipTo";
            _dgv1.Columns["cust_payer"].HeaderText = "payer";
            _dgv1.Columns["cust_plant"].HeaderText = "plant";
            _dgv1.Columns["cust_DistChan"].HeaderText = "DistChan";
            _dgv1.Columns["reg_nameRus"].HeaderText = "Регион";

            _dgv1.Columns["cust_code"].Width = 100;
            _dgv1.Columns["cust_name"].Width = 300;
            _dgv1.Columns["cust_ShipTo"].Width = 70;
            _dgv1.Columns["cust_payer"].Width = 70;
            _dgv1.Columns["cust_plant"].Width = 70;
            _dgv1.Columns["cust_DistChan"].Width = 70;
            _dgv1.Columns["reg_nameRus"].Width = 100;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            new1 = 0;
            loadData();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            new1 = 1;
            loadData();
        }

        private void tbNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                FindMat();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FindMat();
        }

        private void FindMat()
        {
            if (tbNum.Text == "")
            {
                MessageBox.Show("Введите номер.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (DataGridViewRow row in _dgv1.Rows)
            {
                if (row.Cells[1].Value.ToString() == tbNum.Text)
                {
                    _dgv1.ClearSelection();
                    _dgv1.Rows[row.Index].Cells[1].Selected = true;
                    _dgv1.CurrentCell = _dgv1.Rows[row.Index].Cells[1];
                    return;
                }
            }
            MessageBox.Show("Покупатель не найден.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportInExcel(DataGridView dgv)
        {
            object misValue = System.Reflection.Missing.Value;
            Excel.Application xlApp;
            Excel.Workbook xlWB;
            Excel.Worksheet xlSh;

            xlApp = new Excel.Application();
            xlWB = xlApp.Workbooks.Add(misValue);
            xlSh = (Excel.Worksheet)xlWB.Worksheets.get_Item(1);

            try
            {
                int i = 1;

                for (int j = 1; j < dgv.ColumnCount; j++)
                {
                    if (dgv.Columns[j].Visible)
                    {
                        xlSh.Cells[1, i] = dgv.Columns[j].HeaderText;
                        i++;
                    }
                }

                ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 10;
                ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 45;
                ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 15;
                ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 15;
                ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 15;
                ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 15;

                i = 2;

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    int k = 1;
                    for (int j = 1; j < dgv.ColumnCount; j++)
                    {
                        if (dgv.Columns[j].Visible)
                        {
                            xlSh.Cells[i, k] = row.Cells[j].Value.ToString();
                            k++;
                        }
                    }
                    i++;
                }
                xlApp.Visible = true;
            }
            catch (Exception err)
            {
                xlWB.Close(false, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlSh);
                releaseObject(xlWB);
                releaseObject(xlApp);
                MessageBox.Show("Ошибка при выгрузке данных. Системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Не удалось удалить объект. " + ex.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                GC.Collect();
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExportInExcel(_dgv1);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _dgv1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (globalData.UserAccess == 1)
            {
                AddEditCust cust = new AddEditCust(_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["cust_id"].Value.ToString());
                cust.ShowDialog();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditCust cust = new AddEditCust();
            cust.ShowDialog();
        }

        private void Customers_Activated(object sender, EventArgs e)
        {
            if (globalData.update)
            {
                loadData();
                globalData.update = false;
            }
        }
    }
}
