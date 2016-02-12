using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using DataLayer;

namespace RegionR.Directories
{
    public partial class Buttons : Form
    {
        public Buttons()
        {
            InitializeComponent();

            setButtonColor(true, 1);
        }

        Color bbgreen3 = Color.FromArgb(115, 214, 186);
        int btnCur = 0;

        private void setButtonColor(bool mainbutton, int index)
        {
            btn1.BackColor = Control.DefaultBackColor;
            btn2.BackColor = Control.DefaultBackColor;
            btn3.BackColor = Control.DefaultBackColor;
            btn4.BackColor = Control.DefaultBackColor;
            btn5.BackColor = Control.DefaultBackColor;
            btn6.BackColor = Control.DefaultBackColor;
            btn7.BackColor = Control.DefaultBackColor;
            btn8.BackColor = Control.DefaultBackColor;

            if (mainbutton)
            {
                btnHC.BackColor = Control.DefaultBackColor;
                btnAE.BackColor = Control.DefaultBackColor;
                btnOM.BackColor = Control.DefaultBackColor;
                ButtonsVisible(index);

                switch (index)
                {
                    case 1:
                        {
                            btnHC.BackColor = bbgreen3;
                            break;
                        }
                    case 2:
                        {
                            btnAE.BackColor = bbgreen3;
                            break;
                        }
                    case 3:
                        {
                            btnOM.BackColor = bbgreen3;
                            break;
                        }
                }
                loadData(true, index);
                return;
            }

            switch (index)
            {
                case 1:
                    {
                        btn1.BackColor = bbgreen3;
                        break;
                    }
                case 2:
                    {
                        btn2.BackColor = bbgreen3;
                        break;
                    }
                case 3:
                    {
                        btn3.BackColor = bbgreen3;
                        break;
                    }
                case 4:
                    {
                        btn4.BackColor = bbgreen3;
                        break;
                    }
                case 5:
                    {
                        btn5.BackColor = bbgreen3;
                        break;
                    }
                case 6:
                    {
                        btn6.BackColor = bbgreen3;
                        break;
                    }
                case 7:
                    {
                        btn7.BackColor = bbgreen3;
                        break;
                    }
                case 8:
                    {
                        btn8.BackColor = bbgreen3;
                        break;
                    }
            }
            loadData(false, index);
        }

        private void ButtonsVisible(int mainButton)
        {
            switch (mainButton)
            {
                case 1:
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
                case 2:
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
                case 3:
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
            }
        }

        private void loadData(bool mainindex, int index)
        {
            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();

            if (!mainindex)
            {
                if (btnAE.BackColor == bbgreen3)
                    index += 4;
                else if (btnOM.BackColor == bbgreen3)
                    index += 12;
            }
            
            dt1 = sql1.GetRecords("exec selMatForBtn @p1, @p2", Convert.ToInt16(mainindex), index);

            _dgv1.Columns.Clear();

            if (dt1.Rows.Count > 0)
            {
                formatDGV(mainindex);
                foreach (DataRow row in dt1.Rows)
                {
                    _dgv1.Rows.Add(row.ItemArray);
                }
            }
        }

        private void formatDGV(bool main)
        {
            _dgv1.Columns.Add("mat_id", "Material");
            _dgv1.Columns["mat_id"].Visible = false;
            _dgv1.Columns.Add("mat_code", "Код материала");
            _dgv1.Columns["mat_code"].Width = 100;
            _dgv1.Columns["mat_code"].ReadOnly = true;
            _dgv1.Columns.Add("mat_name", "Название материала");
            _dgv1.Columns["mat_name"].Width = 300;
            _dgv1.Columns["mat_name"].ReadOnly = true;

            if (main)
            {
                _dgv1.Columns.Add("btn_id", "Material");
                _dgv1.Columns["btn_id"].Visible = false;
                _dgv1.Columns.Add("btn_name", "Кнопка");
                _dgv1.Columns["btn_name"].Width = 50;
                _dgv1.Columns["btn_name"].ReadOnly = true;
            }

            _dgv1.Columns.Add("nom_id", "nom_id");
            _dgv1.Columns["nom_id"].Visible = false;
            _dgv1.Columns.Add("nom_name", "Иерархия асс. плана");
            _dgv1.Columns["nom_name"].Width = 250;
            _dgv1.Columns["nom_name"].ReadOnly = true;
            _dgv1.Columns.Add("sba_id", "sba_id");
            _dgv1.Columns["sba_id"].Visible = false;
            _dgv1.Columns.Add("sba_code", "SBA code");
            _dgv1.Columns["sba_code"].Width = 50;
            _dgv1.Columns["sba_code"].ReadOnly = true;
            _dgv1.Columns.Add("sba_name", "SBA");
            _dgv1.Columns["sba_name"].Width = 120;
            _dgv1.Columns["sba_name"].ReadOnly = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnHC_Click(object sender, EventArgs e)
        {
            setButtonColor(true, 1);
            btnCur = 0;
        }

        private void btnAE_Click(object sender, EventArgs e)
        {
            setButtonColor(true, 2);
            btnCur = 0;
        }

        private void btnOM_Click(object sender, EventArgs e)
        {
            setButtonColor(true, 3);
            btnCur = 0;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            setButtonColor(false, 1);
            btnCur = 1;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            setButtonColor(false, 2);
            btnCur = 2;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            setButtonColor(false, 3);
            btnCur = 3;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            setButtonColor(false, 4);
            btnCur = 4;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            setButtonColor(false, 5);
            btnCur = 5;
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            setButtonColor(false, 6);
            btnCur = 6;
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            setButtonColor(false, 7);
            btnCur = 7;
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            setButtonColor(false, 8);
            btnCur = 8;
        }

        private int getCurBtn()
        {
            if (btnCur == 0)
                return -1;

            int index = 0;

            if (btnAE.BackColor == bbgreen3)
            {
                index = 4;
            }
            if (btnOM.BackColor == bbgreen3)
            {
                index = 12;
            }
            return btnCur + index;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int cur = 0;

            cur = getCurBtn();
            if (cur == -1)
            {
                MessageBox.Show("Необходимо выбрать подкатегорию.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Sql sql1 = new Sql();
            String s1 = sql1.GetRecordsOne("exec SetMatBtn @p1, @p2", tbMat.Text, cur);

            if (s1 == "notFound")
            {
                MessageBox.Show("Артикул не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Артикул обновлён.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbMat.Text = "";
                loadData(false, btnCur);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
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

                ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 17;
                ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 45;
                ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 8;
                ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 48;
                ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 8;
                ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 20;

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
