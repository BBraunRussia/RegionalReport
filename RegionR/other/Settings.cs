using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataLayer;

namespace RegionR.other
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            getRateDate();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void getRateDate()
        {
            sql sql1 = new sql();

            dateTimePicker2.Value = Convert.ToDateTime(sql1.GetRecordsOne("exec GetDateCur @p1", dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-01"));
            tbRate.Text = sql1.GetRecordsOne("exec GetRate @p1", dateTimePicker2.Value.Year + "-" + dateTimePicker2.Value.Month + "-01");

            DataTable dt1 = new DataTable();
            dt1 = sql1.GetRecords("exec GetSettings");

            if (dt1.Rows[0].ItemArray[0].ToString() == "0")
                rbAPHCRead.Checked = true;
            else
                rbAPHCEdit.Checked = true;

            if (dt1.Rows[0].ItemArray[1].ToString() == "0")
                rbAPAERead.Checked = true;
            else
                rbAPAEEdit.Checked = true;

            if (dt1.Rows[0].ItemArray[2].ToString() == "0")
                rbAPOMRead.Checked = true;
            else
                rbAPOMEdit.Checked = true;

            yearAccPlan.Text = dt1.Rows[0].ItemArray[3].ToString();

            if (dt1.Rows[0].ItemArray[4].ToString() == "0")
                checkBox1.Checked = false;
            else
                checkBox1.Checked = true;

            if (dt1.Rows[0].ItemArray[5].ToString() == "0")
                checkBox2.Checked = false;
            else
                checkBox2.Checked = true;

            if (dt1.Rows[0].ItemArray[6].ToString() == "0")
                rbAccHCRead.Checked = true;
            else  
                rbAccHCEdit.Checked = true;

            if (dt1.Rows[0].ItemArray[7].ToString() == "0")
                rbAccAERead.Checked = true;
            else  
                rbAccAEEdit.Checked = true;

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            sql sql1 = new sql();

            sql1.GetRecordsOne("exec SetDateCur @p1, @p2", dateTimePicker1.Value.Year + "-" + dateTimePicker1.Value.Month + "-01", dateTimePicker2.Value.Year + "-" + dateTimePicker2.Value.Month + "-01");

            if(tbRate.Text != String.Empty)
                sql1.GetRecordsOne("exec SetRate @p1, @p2", dateTimePicker2.Value.Year + "-" + dateTimePicker2.Value.Month + "-01", Convert.ToDouble(tbRate.Text));

            sql1.GetRecordsOne("exec SetSettings @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8", 
                Convert.ToInt32(rbAPHCEdit.Checked), Convert.ToInt32(rbAPAEEdit.Checked), Convert.ToInt32(rbAPOMEdit.Checked),
                yearAccPlan.Text, Convert.ToInt32(checkBox1.Checked), Convert.ToInt32(checkBox2.Checked), Convert.ToInt32(rbAccHCEdit.Checked), Convert.ToInt32(rbAccAEEdit.Checked));

            MessageBox.Show("Настройки сохранены", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true && checkBox2.Checked == true)
            {
                MessageBox.Show("Доступна может быть только одна функция: либо заполнение, либо перенос.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkBox1.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true && checkBox2.Checked == true)
            {
                MessageBox.Show("Доступна может быть только одна функция: либо заполнение, либо перенос.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkBox2.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
             sql sql1 = new sql();

             if (tbYear.Text != String.Empty)
                 sql1.GetRecordsOne("exec SetNextYear @p1, @p2, @p3, @p4", tbYear.Text, cbLPU.Checked, cbNom.Checked, cbUsers.Checked);
             else
                 MessageBox.Show("Введите год!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}