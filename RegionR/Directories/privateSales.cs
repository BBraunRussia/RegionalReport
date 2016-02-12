using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataLayer;

namespace RegionR.Directories
{
    public partial class privateSales : Form
    {
        public privateSales(String UserName, String typeRep)
        {
            InitializeComponent();

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            userID = Convert.ToInt32(sql1.GetRecordsOne("exec GetUserID @p1", UserName));
            
            this.Text = UserName;
            
            if (typeRep == "AP")
            {
                switch (globalData.Div)
                {
                    case "HC":
                        {
                            dt1 = sql1.GetRecords("exec SelAPHCPersSales  @p1, 0, @p2", globalData.Region, UserName);
                            break;
                        }
                    case "AE":
                        {
                            dt1 = sql1.GetRecords("exec SelAPAEPersSales  @p1, 0, @p2", globalData.Region, UserName);
                            break;
                        }
                    case "OM":
                        {
                            dt1 = sql1.GetRecords("exec SelAPOMPersSales  @p1, 0, @p2", globalData.Region, UserName);
                            break;
                        }
                }

                dataGridView1.DataSource = dt1;

                dataGridView1.Columns["rep_id"].Visible = false;
                dataGridView1.Columns["reg_id"].Visible = false;
                dataGridView1.Columns["ps_count1"].Visible = false;

                dataGridView1.Columns["rep_date"].DefaultCellStyle.Format = "MM - yyyy";
                dataGridView1.Columns["rep_date"].Width = 60;
                dataGridView1.Columns["rep_date"].HeaderText = "Дата";
                dataGridView1.Columns["reg_nameRus"].Width = 150;
                dataGridView1.Columns["reg_nameRus"].HeaderText = "Регион продаж";
                dataGridView1.Columns["rep_RefDoc"].HeaderText = "Счёт-фактура";
                dataGridView1.Columns["cust_code"].HeaderText = "код покупателя";
                dataGridView1.Columns["cust_name"].Width = 200;
                dataGridView1.Columns["cust_name"].HeaderText = "Покупатель";
                dataGridView1.Columns["sba_code"].Width = 40;
                dataGridView1.Columns["sba_code"].HeaderText = "SBA код";
                dataGridView1.Columns["sba_name"].Width = 140;
                dataGridView1.Columns["sba_name"].HeaderText = "SBA";
                dataGridView1.Columns["mmg_code"].Visible = false;
                dataGridView1.Columns["mmg_name"].Visible = false;
                dataGridView1.Columns["msg_code"].Visible = false;
                dataGridView1.Columns["msg_name"].Visible = false;
                dataGridView1.Columns["mat_code"].Width = 120;
                dataGridView1.Columns["mat_code"].HeaderText = "Код продукции";
                dataGridView1.Columns["mat_name"].Width = 150;
                dataGridView1.Columns["mat_name"].HeaderText = "Продукция";
                dataGridView1.Columns["cost"].Visible = false;
                dataGridView1.Columns["rep_tail"].Visible = false;
                dataGridView1.Columns["rep_UnitPrice"].Visible = false;
                dataGridView1.Columns["rep_UnitPriceRub"].Visible = false;
                dataGridView1.Columns["ps_count"].DefaultCellStyle.Format = "N0";
                dataGridView1.Columns["ps_count"].Width = 130;
                dataGridView1.Columns["ps_count"].HeaderText = "Количество";
                dataGridView1.Columns["cost2"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["cost2"].HeaderText = "Сумма в Евро";
                dataGridView1.Columns["cost3"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["cost3"].HeaderText = "Сумма в рублях";
                dataGridView1.Columns["LPU_name"].HeaderText = "ЛПУ";
            }
            else
            {
                DataTable dt2 = new DataTable();

                load = false;

                dt2 = sql1.GetRecords("exec SelRegionByUserID @p1", userID);
                if (dt2 == null)
                {
                    MessageBox.Show("Не найдены регионы пользователя");
                    return;
                }
                cbRegion.DataSource = dt2;
                cbRegion.DisplayMember = "reg_nameRus";
                cbRegion.ValueMember = "reg_id";

                dt2 = sql1.GetRecords("exec SelLPUbyRegID @p1, @p2", userID, cbRegion.SelectedValue);
                if (dt2 == null)
                {
                    MessageBox.Show("Не найдены ЛПУ пользователя");
                    return;
                }
                cbLPU.DataSource = dt2;
                cbLPU.DisplayMember = "lpu_sname";
                cbLPU.ValueMember = "lpu_id";

                load = true;

                if (cbLPU.Items.Count > 0)
                    cbLPU.Enabled = true;
                else
                    cbLPU.Enabled = false;

                btnHideLPU.Text = "Скрыть ЛПУ";
                label13.Visible = true;
                cbLPU.Visible = true;
                type = typeRep;
            }
            lbUser.Text = UserName;
        }

        bool load = false;
        int userID = 0;
        Color bbgreen3 = Color.FromArgb(115, 214, 186);
        Color bbgreen1 = Color.FromArgb(0, 180, 130);
        Color bbgray4 = Color.FromArgb(150, 150, 150);
        Color bbgray5 = Color.FromArgb(230, 230, 230);
        String type = "";

        private void loadAccPlanByLPU()
        {
            if (cbLPU.SelectedValue != null)
            {
                Sql sql1 = new Sql();
                dataGridView1.Columns.Clear();
                DataTable dt1 = new DataTable();
                if (globalData.Div == "AE")
                    dt1 = sql1.GetRecords("exec SelAccByLPU @p1, @p2, @p3", globalData.Div, globalData.CurDate.Year, cbLPU.SelectedValue);
                else
                    dt1 = sql1.GetRecords("exec SelAccHCByLPU @p1, @p2, @p3", globalData.Div, globalData.CurDate.Year, cbLPU.SelectedValue);

                dataGridView1.DataSource = dt1;

                formatAcc();
            }
        }

        private void formatAcc()
        {
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["nom_group"].Visible = false;
            dataGridView1.Columns["nom"].HeaderText = "Номенклатура";
            //убрать, если понадобится ассортиментный план за текущий год -2 и -3
            dataGridView1.Columns["py3"].Visible = false;
            dataGridView1.Columns["py2"].Visible = false;
            dataGridView1.Columns["py1"].Visible = false;
            dataGridView1.Columns["nom"].Frozen = true;
            //-------------------------------------------------------------------
            if (globalData.Div == "HC")
            {
                dataGridView1.Columns["nom_type"].HeaderText = "шт/\nевро";
                dataGridView1.Columns["nom_type"].Width = 40;
                dataGridView1.Columns["nom_type"].Frozen = true;

                dataGridView1.Columns["dilcost"].HeaderText = "Дил. цена\nбез НДС";
                dataGridView1.Columns["dilcost"].Width = 65;

                dataGridView1.Columns["cyplanEuro"].HeaderText = (globalData.CurDate.Year).ToString() + "\nплан, EUR";
                dataGridView1.Columns["cyfactEuro"].HeaderText = (globalData.CurDate.Year).ToString() + "\nфакт, EUR";

                dataGridView1.Columns["cyfactEuro"].DefaultCellStyle.BackColor = bbgray5;

                dataGridView1.Columns["cyplanEuro"].DefaultCellStyle.Format = "N2";
                dataGridView1.Columns["cyfactEuro"].DefaultCellStyle.Format = "N2";

                dataGridView1.Columns["nom_type"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["dilcost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns["cyplanEuro"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["cyfactEuro"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dataGridView1.Columns["py3"].HeaderText = (globalData.CurDate.Year - 3).ToString() + "\nфакт, EUR";
            dataGridView1.Columns["py2"].HeaderText = (globalData.CurDate.Year - 2).ToString() + "\nфакт, EUR";
            dataGridView1.Columns["py1"].HeaderText = (globalData.CurDate.Year - 1).ToString() + "\nфакт, EUR";
            dataGridView1.Columns["cyplan"].HeaderText = (globalData.CurDate.Year).ToString() + "\nплан, шт";


            dataGridView1.Columns["cyfact"].HeaderText = (globalData.CurDate.Year).ToString() + "\nфакт, шт";

            dataGridView1.Columns["cyfact"].DefaultCellStyle.BackColor = bbgray5;

            dataGridView1.Columns["pr"].HeaderText = "% плана";
            dataGridView1.Columns["JanPlan"].HeaderText = "Январь\nплан, ед.";
            dataGridView1.Columns["JanFact"].HeaderText = "Январь\nфакт, ед.";
            dataGridView1.Columns["JanPlan"].DefaultCellStyle.BackColor = bbgray5;
            dataGridView1.Columns["JanFact"].DefaultCellStyle.BackColor = bbgray5;
            dataGridView1.Columns["FebPlan"].HeaderText = "Февраль\nплан, ед.";
            dataGridView1.Columns["FebFact"].HeaderText = "Февраль\nфакт, ед.";
            dataGridView1.Columns["MartPlan"].HeaderText = "Март\nплан, ед.";
            dataGridView1.Columns["MartFact"].HeaderText = "Март\nфакт, ед.";
            dataGridView1.Columns["MartPlan"].DefaultCellStyle.BackColor = bbgray5;
            dataGridView1.Columns["MartFact"].DefaultCellStyle.BackColor = bbgray5;
            dataGridView1.Columns["AprPlan"].HeaderText = "Апрель\nплан, ед.";
            dataGridView1.Columns["AprFact"].HeaderText = "Апрель\nфакт, ед.";
            dataGridView1.Columns["MayPlan"].HeaderText = "Май\nплан, ед.";
            dataGridView1.Columns["MayFact"].HeaderText = "Май\nфакт, ед.";
            dataGridView1.Columns["MayPlan"].DefaultCellStyle.BackColor = bbgray5;
            dataGridView1.Columns["MayFact"].DefaultCellStyle.BackColor = bbgray5;
            dataGridView1.Columns["JunePlan"].HeaderText = "Июнь\nплан, ед.";
            dataGridView1.Columns["JuneFact"].HeaderText = "Июнь\nфакт, ед.";
            dataGridView1.Columns["JulePlan"].HeaderText = "Июль\nплан, ед.";
            dataGridView1.Columns["JuleFact"].HeaderText = "Июль\nфакт, ед.";
            dataGridView1.Columns["JulePlan"].DefaultCellStyle.BackColor = bbgray5;
            dataGridView1.Columns["JuleFact"].DefaultCellStyle.BackColor = bbgray5;
            dataGridView1.Columns["AugPlan"].HeaderText = "Август\nплан, ед.";
            dataGridView1.Columns["AugFact"].HeaderText = "Август\nфакт, ед.";
            dataGridView1.Columns["SepPlan"].HeaderText = "Сентябрь\nплан, ед.";
            dataGridView1.Columns["SepFact"].HeaderText = "Сентябрь\nфакт, ед.";
            dataGridView1.Columns["SepPlan"].DefaultCellStyle.BackColor = bbgray5;
            dataGridView1.Columns["SepFact"].DefaultCellStyle.BackColor = bbgray5;
            dataGridView1.Columns["OktPlan"].HeaderText = "Октябрь\nплан, ед.";
            dataGridView1.Columns["OktFact"].HeaderText = "Октябрь\nфакт, ед.";
            dataGridView1.Columns["NovPlan"].HeaderText = "Ноябрь\nплан, ед.";
            dataGridView1.Columns["NovFact"].HeaderText = "Ноябрь\nфакт, ед.";
            dataGridView1.Columns["NovPlan"].DefaultCellStyle.BackColor = bbgray5;
            dataGridView1.Columns["NovFact"].DefaultCellStyle.BackColor = bbgray5;
            dataGridView1.Columns["DecPlan"].HeaderText = "Декабрь\nплан, ед.";
            dataGridView1.Columns["DecFact"].HeaderText = "Декабрь\nфакт, ед.";

            dataGridView1.Columns["py3"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["py2"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["py1"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["cyplan"].DefaultCellStyle.Format = "N0";

            dataGridView1.Columns["cyfact"].DefaultCellStyle.Format = "N0";

            dataGridView1.Columns["pr"].DefaultCellStyle.Format = "N2";

            dataGridView1.Columns["JanPlan"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["JanFact"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["FebPlan"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["FebFact"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["MartPlan"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["MartFact"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["AprPlan"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["AprFact"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["MayPlan"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["MayFact"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["JunePlan"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["JuneFact"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["JulePlan"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["JuleFact"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["AugPlan"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["AugFact"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["SepPlan"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["SepFact"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["OktPlan"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["OktFact"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["NovPlan"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["NovFact"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["DecPlan"].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns["DecFact"].DefaultCellStyle.Format = "N2";            

            dataGridView1.Columns["cyplan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["cyfact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["pr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["JanPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["JanFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["FebPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["FebFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["MartPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["MartFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["AprPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["AprFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["MayPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["MayFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["JunePlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["JuneFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["JulePlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["JuleFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["AugPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["AugFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["SepPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["SepFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["OktPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["OktFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["NovPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["NovFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["DecPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["DecFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dataGridView1.Columns["nom"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            if (globalData.Div == "HC")
                dataGridView1.Columns["nom_type"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridView1.Columns["nom"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.Columns["nom"].Width = 250;
            

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToInt32(row.Cells["nom_group"].Value) == 0)
                {
                    dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = bbgreen3;
                    dataGridView1.Rows[row.Index].DefaultCellStyle.SelectionBackColor = bbgray4;
                }
                if (Convert.ToInt32(row.Cells["nom_group"].Value) == -1)
                {
                    dataGridView1.Rows[row.Index].DefaultCellStyle.BackColor = bbgreen1;
                    dataGridView1.Rows[row.Index].DefaultCellStyle.SelectionBackColor = bbgray4;
                }
            }
        }

        private void btnHideLPU_Click(object sender, EventArgs e)
        {
            if (btnHideLPU.Text == "Скрыть ЛПУ")
            {

                btnHideLPU.Text = "Показать ЛПУ";
                label13.Visible = false;
                cbLPU.Visible = false;
                Sql sql1 = new Sql();
                dataGridView1.Columns.Clear();
                DataTable dt1 = new DataTable();
                if (globalData.Div == "AE")
                    dt1 = sql1.GetRecords("exec SelAccByUser @p1, @p2, @p3, ''", globalData.Div, globalData.CurDate.Year, userID);
                else
                    dt1 = sql1.GetRecords("exec SelAccHCByUser @p1, @p2, @p3, ''", globalData.Div, globalData.CurDate.Year, userID);

                dataGridView1.DataSource = dt1;

                formatAcc();
            }
            else
            {
                loadAccPlanByLPU();
                btnHideLPU.Text = "Скрыть ЛПУ";
                label13.Visible = true;
                cbLPU.Visible = true;
            }
        }

        private void cbLPU_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                loadAccPlanByLPU();
        }

        private void cbRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                loadAccPlanByLPU();
        }

        private void privateSales_Load(object sender, EventArgs e)
        {
            if(type != "AP")
                loadAccPlanByLPU();
        }
    }
}
