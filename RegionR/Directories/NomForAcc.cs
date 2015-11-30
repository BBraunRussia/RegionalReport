using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RegionR.addedit;
using DataLayer;

namespace RegionR.Directories
{
    public partial class NomForAcc : Form
    {
        public NomForAcc()
        {
            InitializeComponent();

            cbYear.Items.Add(globalData.CurDate.Year - 1);
            cbYear.Items.Add(globalData.CurDate.Year);
            cbYear.Items.Add(globalData.CurDate.Year + 1);

            globalData.load = false;
            cbYear.SelectedIndex = 1;
            cbSDiv.SelectedIndex = 0;
            globalData.Div = cbSDiv.SelectedItem.ToString();
            globalData.input = cbYear.SelectedItem.ToString();
            globalData.load = true;
        }

        Color bbgreen3 = Color.FromArgb(115, 214, 186);
        Color bbgray4 = Color.FromArgb(150, 150, 150);

        public void loadData()
        {
            sql sql1 = new sql();
            DataTable dt1 = sql1.GetRecords("exec SelNomBySDiv @p1, @p2", cbSDiv.SelectedItem, cbYear.SelectedItem);

            if (dt1 != null)
            {
                if (_dgv1.Columns.Count == 0)
                {
                    _dgv1.Columns.Add("nom_id", "nom_id");
                    _dgv1.Columns.Add("nom_name", "Наименование");
                    _dgv1.Columns.Add("nom_group", "Итоговая номенклатура");
                    _dgv1.Columns.Add("nom_group_id", "");
                    _dgv1.Columns["nom_group_id"].Visible = false;
                    _dgv1.Columns.Add("nom_type", "Единицы измерения");
                    _dgv1.Columns.Add("dilCost_sum", "Дилерская цена");
                    _dgv1.Columns.Add("nom_seq", "Порядок отображения");
                    _dgv1.Columns.Add("nom_year1", "Год начала отчётности");
                    _dgv1.Columns.Add("nom_year2", "Год окончания отчётности");
                }
                else
                    _dgv1.Rows.Clear();

                int i = 0;

                foreach (DataRow row in dt1.Rows)
                {
                    _dgv1.Rows.Add(row.ItemArray);

                    if (row.ItemArray[2].ToString() == "")
                        _dgv1.Rows[i].DefaultCellStyle.BackColor = bbgreen3;
                    if (Convert.ToInt32(row.ItemArray[8]) < Convert.ToInt32(cbYear.SelectedItem))
                        _dgv1.Rows[i].DefaultCellStyle.BackColor = bbgray4;
                    i++;
                }

                formatData();
                globalData.load = true;
            }
            else
            {
                MessageBox.Show("Не удалось получить данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void formatData()
        {
            _dgv1.Columns["nom_name"].Width = 300;
            _dgv1.Columns["nom_group"].Width = 200;

            if (cbSDiv.SelectedItem.ToString() == "AE")
            {
                _dgv1.Columns["nom_type"].Visible = false;
                _dgv1.Columns["dilCost_sum"].Visible = false;
            }
            else
            {
                _dgv1.Columns["nom_type"].Visible = true;
                _dgv1.Columns["dilCost_sum"].Visible = true;
            }
        }

        private void cbSDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                globalData.Div = cbSDiv.SelectedItem.ToString();
                loadData();
            }
        }

        private void _dgv1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                AddEditNomForAcc aeNFA = new AddEditNomForAcc(_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["nom_id"].Value.ToString(), _dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["nom_name"].Value.ToString(), _dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["nom_group_id"].Value.ToString(), _dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["nom_type"].Value.ToString(), _dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["dilCost_sum"].Value.ToString(), _dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["nom_seq"].Value.ToString(), _dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["nom_year1"].Value.ToString(), _dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["nom_year2"].Value.ToString());
                aeNFA.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Не удалось войти в режим редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditNomForAcc aeNFA = new AddEditNomForAcc();
            aeNFA.ShowDialog();
        }

        private void NomForAcc_Activated(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                loadData();
            }
        }

        private void cbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                globalData.input = cbYear.SelectedItem.ToString();
                loadData();
            }
        }
    }
}
