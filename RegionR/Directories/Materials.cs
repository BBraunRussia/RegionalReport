using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RegionR.addedit;
using Excel = Microsoft.Office.Interop.Excel;
using DataLayer;

namespace RegionR.Directories
{
    public partial class Materials : Form
    {
        Color bbgreen3 = Color.FromArgb(115, 214, 186);
        string sdiv = "HC";
        string btnCur = "";

        public Materials()
        {
            InitializeComponent();

            if (globalData.UserAccess != 1)
            {
                btnAdd.Visible = false;
                btnNull.Visible = false;
            }

            loadData();
        }

        private void loadData()
        {
            setButtonColor();

            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelAllMaterials @p1, @p2", sdiv, btnCur);

            if (dt1.Rows.Count > 0)
            {
                SortOrder s1 = SortOrder.None;
                SortOrder s2 = SortOrder.None;
                SortOrder s3 = SortOrder.None;
                SortOrder s4 = SortOrder.None;
                SortOrder s5 = SortOrder.None;
                SortOrder s6 = SortOrder.None;
                SortOrder s7 = SortOrder.None;
                SortOrder s8 = SortOrder.None;

                if (_dgv1.Columns.Count > 0)
                {
                    s1 = _dgv1.Columns["mat_code"].HeaderCell.SortGlyphDirection;
                    s2 = _dgv1.Columns["mat_name"].HeaderCell.SortGlyphDirection;
                    s3 = _dgv1.Columns["pdiv_code"].HeaderCell.SortGlyphDirection;
                    s4 = _dgv1.Columns["sba_code"].HeaderCell.SortGlyphDirection;
                    s5 = _dgv1.Columns["mmg_code"].HeaderCell.SortGlyphDirection;
                    s6 = _dgv1.Columns["msg_code"].HeaderCell.SortGlyphDirection;
                    s7 = _dgv1.Columns["nom_name"].HeaderCell.SortGlyphDirection;
                    s8 = _dgv1.Columns["btn_name"].HeaderCell.SortGlyphDirection;
                }

                _dgv1.DataSource = dt1;

                if (s1 != SortOrder.None)
                {
                    _dgv1.Columns["mat_code"].HeaderCell.SortGlyphDirection = s1;
                    if (s1 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["mat_code"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["mat_code"], ListSortDirection.Descending);
                }

                if (s2 != SortOrder.None)
                {
                    _dgv1.Columns["mat_name"].HeaderCell.SortGlyphDirection = s2;
                    if (s2 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["mat_name"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["mat_name"], ListSortDirection.Descending);
                }

                if (s3 != SortOrder.None)
                {
                    _dgv1.Columns["pdiv_code"].HeaderCell.SortGlyphDirection = s3;
                    if (s3 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["pdiv_code"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["pdiv_code"], ListSortDirection.Descending);
                }

                if (s4 != SortOrder.None)
                {
                    _dgv1.Columns["sba_code"].HeaderCell.SortGlyphDirection = s4;
                    if (s4 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["sba_code"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["sba_code"], ListSortDirection.Descending);
                }

                if (s5 != SortOrder.None)
                {
                    _dgv1.Columns["mmg_code"].HeaderCell.SortGlyphDirection = s5;
                    if (s5 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["mmg_code"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["mmg_code"], ListSortDirection.Descending);
                }

                if (s6 != SortOrder.None)
                {
                    _dgv1.Columns["msg_code"].HeaderCell.SortGlyphDirection = s6;
                    if (s6 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["msg_code"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["msg_code"], ListSortDirection.Descending);
                }

                if (s7 != SortOrder.None)
                {
                    _dgv1.Columns["nom_name"].HeaderCell.SortGlyphDirection = s7;
                    if (s7 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["nom_name"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["nom_name"], ListSortDirection.Descending);
                }

                if (s8 != SortOrder.None)
                {
                    _dgv1.Columns["btn_name"].HeaderCell.SortGlyphDirection = s8;
                    if (s8 == SortOrder.Ascending)
                        _dgv1.Sort(_dgv1.Columns["btn_name"], ListSortDirection.Ascending);
                    else
                        _dgv1.Sort(_dgv1.Columns["btn_name"], ListSortDirection.Descending);
                }

                format();
            }
        }

        private void setButtonColor()
        {
            btn1.BackColor = Control.DefaultBackColor;
            btn2.BackColor = Control.DefaultBackColor;
            btn3.BackColor = Control.DefaultBackColor;
            btn4.BackColor = Control.DefaultBackColor;
            btn5.BackColor = Control.DefaultBackColor;
            btn6.BackColor = Control.DefaultBackColor;
            btn7.BackColor = Control.DefaultBackColor;
            btn8.BackColor = Control.DefaultBackColor;

            if (btnCur == "")
            {
                btnHC.BackColor = Control.DefaultBackColor;
                btnAE.BackColor = Control.DefaultBackColor;
                btnOM.BackColor = Control.DefaultBackColor;
                btnNull.BackColor = Control.DefaultBackColor;
                ButtonsVisible();

                switch (sdiv)
                {
                    case "HC":
                        {
                            btnHC.BackColor = bbgreen3;
                            break;
                        }
                    case "AE":
                        {
                            btnAE.BackColor = bbgreen3;
                            break;
                        }
                    case "OM":
                        {
                            btnOM.BackColor = bbgreen3;
                            break;
                        }
                    case "(нет)":
                        {
                            btnNull.BackColor = bbgreen3;
                            break;
                        }
                }
                return;
            }

            switch (btnCur)
            {
                case "BC":
                    {
                        btn1.BackColor = bbgreen3;
                        break;
                    }
                case "CC":
                    {
                        btn2.BackColor = bbgreen3;
                        break;
                    }
                case "CN":
                    {
                        btn3.BackColor = bbgreen3;
                        break;
                    }
                case "ICU":
                    {
                        btn4.BackColor = bbgreen3;
                        break;
                    }
                case "ST":
                    {
                        btn1.BackColor = bbgreen3;
                        break;
                    }
                case "NE":
                    {
                        btn2.BackColor = bbgreen3;
                        break;
                    }
                case "PS":
                    {
                        btn3.BackColor = bbgreen3;
                        break;
                    }
                case "OT":
                    {
                        btn4.BackColor = bbgreen3;
                        break;
                    }
                case "SP":
                    {
                        btn5.BackColor = bbgreen3;
                        break;
                    }
                case "CT":
                    {
                        btn6.BackColor = bbgreen3;
                        break;
                    }
                case "VS":
                    {
                        btn7.BackColor = bbgreen3;
                        break;
                    }
                case "AM":
                    {
                        btn8.BackColor = bbgreen3;
                        break;
                    }
                case "Incontinence Care":
                    {
                        btn1.BackColor = bbgreen3;
                        break;
                    }
                case "Stoma Care":
                    {
                        btn2.BackColor = bbgreen3;
                        break;
                    }
                case "Wound Management":
                    {
                        btn3.BackColor = bbgreen3;
                        break;
                    }
                case "Infection Control":
                    {
                        btn4.BackColor = bbgreen3;
                        break;
                    }
            }
        }

        private void ButtonsVisible()
        {
            switch (sdiv)
            {
                case "HC":
                    {
                        btn1.Text = "BC";
                        btn2.Text = "CC";
                        btn3.Text = "CN";
                        btn4.Text = "ICU";
                        btn1.Visible = true;
                        btn2.Visible = true;
                        btn3.Visible = true;
                        btn4.Visible = true;
                        btn5.Visible = false;
                        btn6.Visible = false;
                        btn7.Visible = false;
                        btn8.Visible = false;
                        break;
                    }
                case "AE":
                    {
                        btn1.Text = "ST";
                        btn2.Text = "NE";
                        btn3.Text = "PS";
                        btn4.Text = "OT";
                        btn5.Text = "SP";
                        btn6.Text = "CT";
                        btn7.Text = "VS";
                        btn8.Text = "AM";
                        btn1.Visible = true;
                        btn2.Visible = true;
                        btn3.Visible = true;
                        btn4.Visible = true;
                        btn5.Visible = true;
                        btn6.Visible = true;
                        btn7.Visible = true;
                        btn8.Visible = true;
                        break;
                    }
                case "OM":
                    {
                        btn1.Text = "Incontinence Care";
                        btn2.Text = "Stoma Care";
                        btn3.Text = "Wound Management";
                        btn4.Text = "Infection Control";
                        btn1.Visible = true;
                        btn2.Visible = true;
                        btn3.Visible = true;
                        btn4.Visible = true;
                        btn5.Visible = false;
                        btn6.Visible = false;
                        btn7.Visible = false;
                        btn8.Visible = false;
                        break;
                    }
                case "(нет)":
                    {
                        btn1.Visible = false;
                        btn2.Visible = false;
                        btn3.Visible = false;
                        btn4.Visible = false;
                        btn5.Visible = false;
                        btn6.Visible = false;
                        btn7.Visible = false;
                        btn8.Visible = false;
                        break;
                    }
            }
        }

        private void format()
        {
            _dgv1.Columns["mat_id"].Visible = false;

            if (btnOM.BackColor == bbgreen3)
            {
                _dgv1.Columns["nom_name"].Visible = false;
                _dgv1.Columns["btn_name"].Width = 150;
            }
            else
            {
                _dgv1.Columns["nom_name"].Visible = true;
                _dgv1.Columns["nom_name"].HeaderText = "Номенклатура";
                _dgv1.Columns["nom_name"].Width = 250;
                _dgv1.Columns["btn_name"].Width = 50;
            }

            _dgv1.Columns["mat_code"].HeaderText = "Артикул";
            _dgv1.Columns["mat_name"].HeaderText = "Название";
            _dgv1.Columns["pdiv_code"].HeaderText = "PDiv";
            _dgv1.Columns["sba_code"].HeaderText = "SBA";
            _dgv1.Columns["mmg_code"].HeaderText = "MMG";
            _dgv1.Columns["msg_code"].HeaderText = "MSG";
            _dgv1.Columns["btn_name"].HeaderText = "Кнопка";

            _dgv1.Columns["mat_code"].Width = 100;
            _dgv1.Columns["mat_name"].Width = 300;
            _dgv1.Columns["pdiv_code"].Width = 50;
            _dgv1.Columns["sba_code"].Width = 50;
            _dgv1.Columns["mmg_code"].Width = 50;
            _dgv1.Columns["msg_code"].Width = 50;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditMat aemat = new AddEditMat();
            aemat.ShowDialog();
        }

        private void _dgv1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (globalData.UserAccess == 1)
            {
                AddEditMat aemat = new AddEditMat(_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["mat_id"].Value.ToString());
                aemat.ShowDialog();
            }
        }

        private void Materials_Activated(object sender, EventArgs e)
        {
            if (globalData.update)
            {
                int indexRow = _dgv1.CurrentCell.RowIndex;
                int indexCol = _dgv1.CurrentCell.ColumnIndex;

                loadData();

                if (indexRow >= _dgv1.Rows.Count)
                    _dgv1.CurrentCell = _dgv1[indexCol, _dgv1.Rows.Count - 1];
                else
                    _dgv1.CurrentCell = _dgv1[indexCol, indexRow];
                
                globalData.update = false;
            }
        }

        private void btnHC_Click(object sender, EventArgs e)
        {
            sdiv = "HC";
            btnCur = "";
            loadData();
        }

        private void btnAE_Click(object sender, EventArgs e)
        {
            sdiv = "AE";
            btnCur = "";
            loadData();
        }

        private void btnOM_Click(object sender, EventArgs e)
        {
            sdiv = "OM";
            btnCur = "";
            loadData();
        }

        private void btnNull_Click(object sender, EventArgs e)
        {
            sdiv = "(нет)";
            btnCur = "";
            loadData();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            btnCur = btn1.Text;
            loadData();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            btnCur = btn2.Text;
            loadData();
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            btnCur = btn3.Text;
            loadData();
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            btnCur = btn4.Text;
            loadData();
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            btnCur = btn5.Text;
            loadData();
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            btnCur = btn6.Text;
            loadData();
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            btnCur = btn7.Text;
            loadData();
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            btnCur = btn8.Text;
            loadData();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FindMat();
        }

        private void tbMat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                FindMat();
        }

        private void FindMat()
        {
            if (tbMat.Text == "")
            {
                MessageBox.Show("Введите артикул.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (DataGridViewRow row in _dgv1.Rows)
            {
                if (row.Cells[1].Value.ToString() == tbMat.Text)
                {
                    _dgv1.ClearSelection();
                    _dgv1.Rows[row.Index].Cells[1].Selected = true;
                    _dgv1.CurrentCell = _dgv1.Rows[row.Index].Cells[1];
                    return;
                }
            }
            MessageBox.Show("Артикул не найден.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExportInExcel(_dgv1);
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
                ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 6;
                ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 6;
                ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 6;
                ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 6;
                ((Excel.Range)xlSh.Columns[7]).ColumnWidth = 35;
                ((Excel.Range)xlSh.Columns[8]).ColumnWidth = 8;

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
    }
}