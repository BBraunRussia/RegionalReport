using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.addedit
{
    public partial class AddMA_Name : Form
    {
        public AddMA_Name()
        {
            InitializeComponent();

            loadType();
            loadTheme();
            loadConfName();
        }

        public AddMA_Name(int type)
        {
            InitializeComponent();

            loadType();
            loadTheme();

            cbMAType.SelectedValue = type;
            loadConfName();

        }

        void loadTheme()
        {
            sql sql1 = new sql();

            globalData.update = false;
            comboBox1.DataSource = sql1.GetRecords("exec MarkAct_Select_ConfTheme");
            comboBox1.DisplayMember = "confth_name";
            comboBox1.ValueMember = "confth_id";
            globalData.update = true;
        }

        void loadType()
        {
            sql sql1 = new sql();

            globalData.update = false;
            cbMAType.DataSource = sql1.GetRecords("exec MarkAct_Sel_Type @p1", -2);
            cbMAType.DisplayMember = "matype_name";
            cbMAType.ValueMember = "matype_id";
            globalData.update = true;
        }

        bool loadConfName(int search = 0)
        {
            if (dgv1.Rows != null)
            {   
                dgv1.Columns.Clear();
                dgv1.Rows.Clear();
            }

            sql sql1 = new sql();

            globalData.update = false;

            DataTable dt = new DataTable();

            if (search == 0)
                dt = sql1.GetRecords("exec MarkAct_Select_ConfNameAll @p1, @p2", cbMAType.SelectedValue, comboBox1.SelectedValue);
            else
                dt = sql1.GetRecords("exec MarkAct_Select_SearchConf @p1", textBox1.Text.Trim());

            dgv1.Columns.Add("conf_id", "ИД");
            dgv1.Columns.Add("conf_sname", "Сокращенное наименование");
            dgv1.Columns.Add("conf_name", "Полное наименование");
            dgv1.Columns.Add(new DataGridViewLinkColumn());
            dgv1.Columns[3].HeaderText = "Сайт";
            dgv1.Columns.Add("confth_name", "Тематика");
            dgv1.Columns.Add("matype_name", "Тип мероприятия");
            dgv1.Columns.Add("matype_id", "Тип ид");
            dgv1.Columns.Add("confth_id", "ид тема");

            if (dt == null)
                return false;

            foreach (DataRow row in dt.Rows)
            {
                dgv1.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], row.ItemArray[4], row.ItemArray[5], row.ItemArray[6], row.ItemArray[7]);
            }

            dgv1.Columns["conf_id"].Visible = false;
            dgv1.Columns["matype_id"].Visible = false;
            dgv1.Columns["confth_id"].Visible = false;


            globalData.update = true;
            
            return true;
        }

        /* Добавить */
        private void button2_Click(object sender, EventArgs e)
        {
            AddEditMAType am = new AddEditMAType();
            am.ShowDialog();
            this.Close();
        }

        /* Назад */
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
                loadConfName();
        }

        private void cbMAType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
                loadConfName();
        }

        private void dgv1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1 && e.ColumnIndex != 3)
                {
                    sql sql1 = new sql();

                    string count = sql1.GetRecordsOne("exec MarkAct_Select_Plan @p1, @p2, @p3", dgv1[0, e.RowIndex].Value.ToString(), globalData.year, 0);

                    if (count == "0")
                    {
                        globalData.maplan = 0;
                        AddEditMAType am = new AddEditMAType(Convert.ToInt32(dgv1[0, e.RowIndex].Value), dgv1[1, e.RowIndex].Value.ToString(), dgv1[2, e.RowIndex].Value.ToString(), dgv1[3, e.RowIndex].Value.ToString(), dgv1[4, e.RowIndex].Value.ToString(), dgv1[5, e.RowIndex].Value.ToString());
                        am.ShowDialog();
                    }
                    else if (count == "1")
                    {
                        globalData.maplan = Convert.ToInt32(sql1.GetRecordsOne("exec MarkAct_Select_Plan @p1, @p2, @p3", dgv1[0, e.RowIndex].Value.ToString(), globalData.year, 1));
                    }
                    else
                    {
                        AddConf ac = new AddConf(Convert.ToInt32(dgv1[0, e.RowIndex].Value.ToString()), false);
                        ac.ShowDialog();

                    }
                }
            }
            catch
            {
            }
            finally
            {
                globalData.matype = dgv1[6, e.RowIndex].Value.ToString();
                this.Close();
            }
        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgv1.Columns[e.ColumnIndex] is DataGridViewLinkColumn)
            {
                Process.Start(this.dgv1[e.ColumnIndex, e.RowIndex].Value.ToString());
            }
        }

        private void dgv1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    if (e.RowIndex != -1 && e.ColumnIndex != 3)
            //    {
            //        sql sql1 = new sql();

            //        string count = sql1.GetRecordsOne("exec MarkAct_Select_Plan @p1, @p2, @p3", dgv1[0, e.RowIndex].Value.ToString(), globalData.mayear, 0);

            //        if (count == "0")
            //        {
            //            globalData.maplan = 0;
            //            this.Close();
            //        }
            //        else if (count == "1")
            //        {
            //            globalData.maplan = Convert.ToInt32(sql1.GetRecordsOne("exec MarkAct_Select_Plan @p1, @p2, @p3", dgv1[0, e.RowIndex].Value.ToString(), globalData.mayear, 1));
            //            this.Close();
            //        }
            //        else
            //        {
            //            this.Close();
            //            AddConf ac = new AddConf(Convert.ToInt32(dgv1[0, e.RowIndex].Value.ToString()), false);
            //            ac.ShowDialog();

            //        }
            //    }
            //}
            //catch
            //{
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != String.Empty)
            {
                if (loadConfName(1) == false)
                    MessageBox.Show("Ничего не найдено! \nПоробуйте изменить поиск или \nвоспользоваться фильтрами.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Введите строку поиска!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text.Trim() != String.Empty)
            {
                if (loadConfName(1) == false)
                    MessageBox.Show("Ничего не найдено! \nПоробуйте изменить поиск или \nвоспользоваться фильтрами.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Введите строку поиска!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void добавитьМестоПроведенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                globalData.maplan = 0;
                int ri = dgv1.SelectedCells[0].RowIndex;
                AddEditMAType am = new AddEditMAType(Convert.ToInt32(dgv1[0, ri].Value), dgv1[1, ri].Value.ToString(), dgv1[2, ri].Value.ToString(), dgv1[3, ri].Value.ToString(), dgv1[4, ri].Value.ToString(), dgv1[5, ri].Value.ToString());
                am.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Не удалось. Обратитесь к администратору системы.");
            }
        }

        private void просмотрМестПроведенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int ri = dgv1.SelectedCells[0].RowIndex;
                AddConf ac = new AddConf(Convert.ToInt32(dgv1[0, ri].Value.ToString()), false);
                ac.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Не удалось. Обратитесь к администратору системы.");
            }
        }
    }
}
