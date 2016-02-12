using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.other
{
    public partial class UpdRepDist : Form
    {
        public UpdRepDist()
        {
            InitializeComponent();

            DataTable dt1 = new DataTable();
            Sql sql1 = new Sql();

            dt1 = sql1.GetRecords("exec SelCustRepDist");

            globalData.load = false;
            cbRepD.DataSource = dt1;
            cbRepD.DisplayMember = "cust_name";
            cbRepD.ValueMember = "cust_id";
           

            cbDivRep.SelectedIndex = 0;
            dt1 = sql1.GetRecords("exec selDistMatUpd @p1, @p2, @p3", dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month.ToString() + "-01", cbRepD.SelectedValue, cbDivRep.SelectedItem);

            cbMatRep.DataSource = dt1;
            cbMatRep.DisplayMember = "mat_name";
            cbMatRep.ValueMember = "mat_id";
            globalData.load = true;
        }


        private void Mat()
        {
            DataTable dt1 = new DataTable();
            Sql sql1 = new Sql();

            globalData.load = false;
            dt1 = sql1.GetRecords("exec selDistMatUpd @p1, @p2, @p3, @p4", 
                                            dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month.ToString() + "-01", 
                                            dateTimePicker2.Value.Year.ToString() + "-" + dateTimePicker2.Value.Month.ToString() + "-01",  
                                            cbRepD.SelectedValue, cbDivRep.SelectedItem);

            cbMatRep.DataSource = dt1;
            cbMatRep.DisplayMember = "mat_name";
            cbMatRep.ValueMember = "mat_id";
            globalData.load = true;

        }


        private void searchRepDist()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();
            
            dt1 = sql1.GetRecords("exec SelRepDist @p1, @p2, @p3", 
                cbRepD.SelectedValue, 
                dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month.ToString() + "-01", 
                dateTimePicker2.Value.Year.ToString() + "-" + dateTimePicker2.Value.Month.ToString() + "-01");

            dgvRepDistUpd.DataSource = dt1;

            if (dt1 == null)
            {
                dgvRepDistUpd.Rows.Clear();
                return;
            }

            if (globalData.UserID != 1 && globalData.UserID != 258)
            {
                dgvRepDistUpd.Columns["rd_id"].Visible = false;
                dgvRepDistUpd.Columns["db_id"].Visible = false;
            }
            else
            {
                dgvRepDistUpd.Columns["rd_id"].Visible = true;
                dgvRepDistUpd.Columns["db_id"].Visible = true;
            }

            dgvRepDistUpd.Columns["rd_date"].HeaderText = "Дата";
            dgvRepDistUpd.Columns["cust_code"].HeaderText = "Код дистрибьютора";
            dgvRepDistUpd.Columns["cust_name"].HeaderText = "Название дистрибьютора";
            dgvRepDistUpd.Columns["subreg_nameRus"].HeaderText = "Субрегион";
            dgvRepDistUpd.Columns["mat_code"].HeaderText = "Артикул";
            dgvRepDistUpd.Columns["mat_name"].HeaderText = "Название продукции";
            dgvRepDistUpd.Columns["rd_lpu"].HeaderText = "Покупатель";
            dgvRepDistUpd.Columns["rd_count"].HeaderText = "Количество";
        }

        private void UpdCount( int flag, string text )
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();
            
            dt1 = sql1.GetRecords("exec UpdRepDistCount @p1, @p2, @p3, @p4, @p5, @p6",  
                                    cbRepD.SelectedValue,
                                    cbMatRep.SelectedValue,
                                    text,
                                    dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month.ToString() + "-01",
                                    dateTimePicker2.Value.Year.ToString() + "-" + dateTimePicker2.Value.Month.ToString() + "-01", flag);

            if (dt1 == null)
                MessageBox.Show("Не удалось обновить количество!","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
            else
                MessageBox.Show("Количество успешно обновленно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void UpdMat()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec UpdRepDistMat @p1, @p2, @p3, @p4, @p5",
                                    cbRepD.SelectedValue, textBox2.Text, textBox3.Text,
                                    dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month.ToString() + "-01", 
                                    dateTimePicker2.Value.Year.ToString() + "-" + dateTimePicker2.Value.Month.ToString() + "-01");

            if (dt1 == null)
                MessageBox.Show("Не удалось обновить количество!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("Количество успешно обновленно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        //найти
        private void button2_Click(object sender, EventArgs e)
        {
            searchRepDist();
        }

        //обновить
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != String.Empty)
                UpdCount(0, textBox1.Text);
            
            if (textBox4.Text != String.Empty)
                UpdCount(1, textBox4.Text);

            if (textBox2.Text != String.Empty && textBox3.Text != String.Empty)
                UpdMat();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //Mat();
        }

        private void cbDivRep_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Mat();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Mat();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //DialogResult dr = MessageBox.Show("Удалить выбранные строки из отчёта?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //if (dr == System.Windows.Forms.DialogResult.Yes)
            //{
                if (dgvRepDistUpd.Rows.Count != 0)
                {
                    Sql sql1 = new Sql();

                    foreach (DataGridViewCell cell in dgvRepDistUpd.SelectedCells)
                    {
                        sql1.GetRecords("exec DelStringRepDist @p1, @p2", dgvRepDistUpd.Rows[cell.RowIndex].Cells[0].Value, dgvRepDistUpd.Rows[cell.RowIndex].Cells[1].Value);
                        dgvRepDistUpd.Rows.RemoveAt(cell.RowIndex);
                    }
                }
            //}
        }
    }
}
