using DataLayer;
using RegionR.addedit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.Directories
{
    public partial class RefDoc : Form
    {
        public RefDoc()
        {
            InitializeComponent();

            button2.BackColor = Color.FromArgb(115, 214, 186);

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            
            dt1 = sql1.GetRecords("exec SelSubRegion");
            comboBox1.DataSource = dt1;
            comboBox1.DisplayMember = "subreg_nameRus";
            comboBox1.ValueMember = "subreg_id";

            dt1 = sql1.GetRecords("exec SelSDiv 1");
            cbSDiv.DataSource = dt1;
            cbSDiv.DisplayMember = "sdiv_code";
            cbSDiv.ValueMember = "sdiv_id";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SearchRef();
        }


        private void SearchRef()
        {
            Sql sql1 = new Sql();

            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Вы не ввели счет-фактуру!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec SearchRefDoc @p1", textBox1.Text);

            _dgvRefDoc.DataSource = dt1;
            loadDgv(_dgvRefDoc);
        }

        private void loadDgv(DataGridView dgv)
        {
            dgv.Columns["rep_date2"].HeaderText = "Дата отображения";
            dgv.Columns["reg_nameRus"].HeaderText = "Регион";
            dgv.Columns["subreg_nameRus"].HeaderText = "Субрегион";
            dgv.Columns["sdiv_id_ap"].HeaderText = "Дивизион";
            dgv.Columns["rep_SalesQuanBum"].HeaderText = "Количество (Остаток)";
            dgv.Columns["rep_SalesRev"].HeaderText = "Цена (евро)";
            dgv.Columns["ps_date"].HeaderText = "Дата распределения";
            dgv.Columns["user_name"].HeaderText = "Региональный представитель";
            dgv.Columns["ps_count"].HeaderText = "Распр. кол-во";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Вы не ввели счет-фактуру!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataTable dt1 = new DataTable();
            if (MessageBox.Show("Вы уверены, что хотите удалить с-ф:" + textBox1.Text+"? Удаление будет безвозвратным!", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                dt1 = sql1.GetRecords("exec DelRefDoc @p1", textBox1.Text);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SearchRef();
        }

        /* Перенести с-ф в другой регион */
        private void button4_Click(object sender, EventArgs e)
        {
            if (globalData.UserAccess == 1)
            {
                try
                {
                    globalData.update = false;
                    Sql sql1 = new Sql();
                    string res = String.Empty;
                    int err1 = 0;

                    foreach (DataGridViewCell cell in _dgvRefDoc.SelectedCells)
                    {
                        if ((_dgvRefDoc.Rows[cell.RowIndex].Cells[0].Value == null && _dgvRefDoc.Rows[cell.RowIndex].Cells[1].Value == null) ||
                            (_dgvRefDoc.Rows[cell.RowIndex].Cells[0].Value.ToString() == "" && _dgvRefDoc.Rows[cell.RowIndex].Cells[1].Value.ToString() == ""))
                            res = sql1.GetRecordsOne("exec UpdReportRefDoc @p1, @p2, @p3, @p4, @p5, @p6", _dgvRefDoc.Rows[cell.RowIndex].Cells[2].Value.ToString(), _dgvRefDoc.Rows[cell.RowIndex].Cells[3].Value.ToString(), comboBox1.SelectedValue, globalData.UserID, dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month.ToString() + "-01", cbSDiv.SelectedValue);

                        if (res == "1")
                            err1++;
                    }

                    if (err1 != 0)
                        MessageBox.Show("Кол-во ошибок " + err1.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    
                }
                catch (Exception err)
                {
                    MessageBox.Show("Не удалось обновить продажу. Системная ошибка: " + err.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }

        }
    }
}
