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
    public partial class RepDistRight : Form
    {
        public RepDistRight()
        {
            InitializeComponent();

            globalData.update = false;
            cbYear.Items.Add(globalData.CurDate.Year - 2);
            cbYear.Items.Add(globalData.CurDate.Year - 1);
            cbYear.Items.Add(globalData.CurDate.Year);
            cbYear.SelectedIndex = cbYear.Items.Count - 1;
            globalData.update = true;

            loaddata();
        }

        private void loaddata()
        {
            sql sql1 = new sql();
            DataTable dt1 = sql1.GetRecords("exec SelRepDistRight @p1", cbYear.SelectedItem);

            format();

            bool a1 = false, a2 = false, a3 = false, a4 = false, a5 = false, a6 = false, a7 = false, a8 = false, a9 = false, a10 = false, a11 = false, a12 = false;

            foreach (DataRow row in dt1.Rows)
            {
                a1 = Convert.ToBoolean(row["rdr_allow1"]);
                a2 = Convert.ToBoolean(row["rdr_allow2"]);
                a3 = Convert.ToBoolean(row["rdr_allow3"]);
                a4 = Convert.ToBoolean(row["rdr_allow4"]);
                a5 = Convert.ToBoolean(row["rdr_allow5"]);
                a6 = Convert.ToBoolean(row["rdr_allow6"]);
                a7 = Convert.ToBoolean(row["rdr_allow7"]);
                a8 = Convert.ToBoolean(row["rdr_allow8"]);
                a9 = Convert.ToBoolean(row["rdr_allow9"]);
                a10 = Convert.ToBoolean(row["rdr_allow10"]);
                a11 = Convert.ToBoolean(row["rdr_allow11"]);
                a12 = Convert.ToBoolean(row["rdr_allow12"]);
                
                _dgv1.Rows.Add(row["cust_id"], row["cust_code"], row["cust_name"], a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
            }
        }

        private void format()
        {
            _dgv1.Columns.Clear();
            _dgv1.Columns.Add("cust_id", "cust_id");
            _dgv1.Columns["cust_id"].Visible = false;
            _dgv1.Columns.Add("cust_code", "cust_code");
            _dgv1.Columns.Add("cust_name", "cust_name");

            DataGridViewCheckBoxColumn jan = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn feb = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn mart = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn apr = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn may = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn june = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn jule = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn aug = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn sep = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn okt = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn nov = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn dec = new DataGridViewCheckBoxColumn();

            jan.DataPropertyName = "rdr_allow1";
            feb.DataPropertyName = "rdr_allow2";
            mart.DataPropertyName = "rdr_allow3";
            apr.DataPropertyName = "rdr_allow4";
            may.DataPropertyName = "rdr_allow5";
            june.DataPropertyName = "rdr_allow6";
            jule.DataPropertyName = "rdr_allow7";
            aug.DataPropertyName = "rdr_allow8";
            sep.DataPropertyName = "rdr_allow9";
            okt.DataPropertyName = "rdr_allow10";
            nov.DataPropertyName = "rdr_allow11";
            dec.DataPropertyName = "rdr_allow12";

            jan.Name = "rdr_allow1";
            feb.Name = "rdr_allow2";
            mart.Name = "rdr_allow3";
            apr.Name = "rdr_allow4";
            may.Name = "rdr_allow5";
            june.Name = "rdr_allow6";
            jule.Name = "rdr_allow7";
            aug.Name = "rdr_allow8";
            sep.Name = "rdr_allow9";
            okt.Name = "rdr_allow10";
            nov.Name = "rdr_allow11";
            dec.Name = "rdr_allow12";
            
            _dgv1.Columns.Add(jan);
            _dgv1.Columns.Add(feb);
            _dgv1.Columns.Add(mart);
            _dgv1.Columns.Add(apr);
            _dgv1.Columns.Add(may);
            _dgv1.Columns.Add(june);
            _dgv1.Columns.Add(jule);
            _dgv1.Columns.Add(aug);
            _dgv1.Columns.Add(sep);
            _dgv1.Columns.Add(okt);
            _dgv1.Columns.Add(nov);
            _dgv1.Columns.Add(dec);
            
            _dgv1.Columns["rdr_allow1"].SortMode = DataGridViewColumnSortMode.Programmatic;
            _dgv1.Columns["rdr_allow2"].SortMode = DataGridViewColumnSortMode.Programmatic;
            _dgv1.Columns["rdr_allow3"].SortMode = DataGridViewColumnSortMode.Programmatic;
            _dgv1.Columns["rdr_allow4"].SortMode = DataGridViewColumnSortMode.Programmatic;
            _dgv1.Columns["rdr_allow5"].SortMode = DataGridViewColumnSortMode.Programmatic;
            _dgv1.Columns["rdr_allow6"].SortMode = DataGridViewColumnSortMode.Programmatic;
            _dgv1.Columns["rdr_allow7"].SortMode = DataGridViewColumnSortMode.Programmatic;
            _dgv1.Columns["rdr_allow8"].SortMode = DataGridViewColumnSortMode.Programmatic;
            _dgv1.Columns["rdr_allow9"].SortMode = DataGridViewColumnSortMode.Programmatic;
            _dgv1.Columns["rdr_allow10"].SortMode = DataGridViewColumnSortMode.Programmatic;
            _dgv1.Columns["rdr_allow11"].SortMode = DataGridViewColumnSortMode.Programmatic;
            _dgv1.Columns["rdr_allow12"].SortMode = DataGridViewColumnSortMode.Programmatic;
            
            _dgv1.Columns["cust_code"].HeaderText = "код Дистрибьютора";
            _dgv1.Columns["cust_name"].HeaderText = "название Дистрибьютора";
            _dgv1.Columns["rdr_allow1"].HeaderText = "Январь";
            _dgv1.Columns["rdr_allow2"].HeaderText = "Февраль";
            _dgv1.Columns["rdr_allow3"].HeaderText = "Март";
            _dgv1.Columns["rdr_allow4"].HeaderText = "Апрель";
            _dgv1.Columns["rdr_allow5"].HeaderText = "Май";
            _dgv1.Columns["rdr_allow6"].HeaderText = "Июнь";
            _dgv1.Columns["rdr_allow7"].HeaderText = "Июль";
            _dgv1.Columns["rdr_allow8"].HeaderText = "Август";
            _dgv1.Columns["rdr_allow9"].HeaderText = "Сентябрь";
            _dgv1.Columns["rdr_allow10"].HeaderText = "Октябрь";
            _dgv1.Columns["rdr_allow11"].HeaderText = "Ноябрь";
            _dgv1.Columns["rdr_allow12"].HeaderText = "Декабрь";
        }

        private void cbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update)
                loaddata();
        }

        private void _dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 2)
            {
                _dgv1[e.ColumnIndex, e.RowIndex].Value = !Convert.ToBoolean(_dgv1[e.ColumnIndex, e.RowIndex].Value);

                sql sql1 = new sql();
                sql1.GetRecords("exec ChangeRepDistRight @p1, @p2", _dgv1[0, e.RowIndex].Value, cbYear.SelectedItem.ToString() + "-" + (e.ColumnIndex-2).ToString() + "-01");
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InputDialog ind = new InputDialog("Дистрибьютор", "Введите код дистр.(Напр.:20440997)\nНе забудьте добавить РЕГИОН!", true);
            ind.ShowDialog();

            if (ind.DialogResult == DialogResult.Cancel)
                return;
            String code = globalData.input;


            ind = new InputDialog("Дата начала отчётов", "Введите год", true);
            ind.ShowDialog();

            if (ind.DialogResult == DialogResult.Cancel)
                return;
            String yearR = globalData.input;

            ind = new InputDialog("Дата начала отчётов", "Введите месяц в формате 1,2,3..", true);
            ind.ShowDialog();

            if (ind.DialogResult == DialogResult.Cancel)
                return;
            String monthR = globalData.input;

            sql sql1 = new sql();
            DataTable dt1 = sql1.GetRecords("exec InsRepDistRight @p1, @p2", code, yearR + "-" + monthR + "-01");
        }
    }
}
