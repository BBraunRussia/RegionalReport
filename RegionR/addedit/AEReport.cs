using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataLayer;

namespace RegionR.addedit
{
    public partial class AEReport : Form
    {
        public AEReport()
        {
            InitializeComponent();
        }
        
        string idRep = "0", idDB = "0";

        public AEReport(string idRep1, string idDB1)
        {
            try
            {
                InitializeComponent();

                loadData();

                idRep = idRep1;
                idDB = idDB1;

                Sql sql1 = new Sql();
                DataTable dt1 = sql1.GetRecords("exec GetReportByID @p1, @p2", idRep, idDB);

                if (dt1 == null)
                {
                    MessageBox.Show("Не удалось загрузить данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }

                cbReg.SelectedValue = Convert.ToInt32(dt1.Rows[0].ItemArray[0]);
                cbSubReg.SelectedValue = Convert.ToInt32(dt1.Rows[0].ItemArray[1]); ;
                dateTimePicker1.Value = Convert.ToDateTime(dt1.Rows[0].ItemArray[2]);
                cbSDiv.SelectedValue = Convert.ToInt32(dt1.Rows[0].ItemArray[3]);
                
                if (dt1.Rows[0].ItemArray[4].ToString() == "0")
                {
                    lbCheck.Text = "Продажа не распределена";
                    btnOK.Enabled = true;
                }
                else
                {
                    lbCheck.Text = "Продажа распределена. Количество распределений - " + dt1.Rows[0].ItemArray[4].ToString();
                    btnOK.Enabled = false;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Не загрузить данные. Системная ошибка: " + err.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            if (globalData.UserAccess == 1)
            {
                dateTimePicker1.Enabled = true;
                cbReg.Enabled = true;
            }

            if ((globalData.UserAccess == 5) || (globalData.UserAccess == 6))
            {
                dateTimePicker1.Enabled = false;
                cbReg.Enabled = false;
            }
        }

        private void loadData()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec Region_Select");
            cbReg.DataSource = dt1;
            cbReg.DisplayMember = "reg_nameRus";
            cbReg.ValueMember = "reg_id";

            dt1 = sql1.GetRecords("exec SelSubRegion");
            cbSubReg.DataSource = dt1;
            cbSubReg.DisplayMember = "subreg_nameRus";
            cbSubReg.ValueMember = "subreg_id";

            dt1 = sql1.GetRecords("exec SelSDiv 1");
            cbSDiv.DataSource = dt1;
            cbSDiv.DisplayMember = "sdiv_code";
            cbSDiv.ValueMember = "sdiv_id";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            globalData.update = false;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                globalData.update = false;
                Sql sql1 = new Sql();
                DataTable dt1 = sql1.GetRecords("exec UpdReport @p1, @p2, @p3, @p4, @p5, @p6, @p7", idRep, idDB, cbReg.SelectedValue, cbSubReg.SelectedValue, globalData.UserID2, dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month.ToString() + "-01", cbSDiv.SelectedValue);

                if (dt1 == null)
                    MessageBox.Show("Не удалось обновить продажу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            catch(Exception err)
            {
                MessageBox.Show("Не удалось обновить продажу. Системная ошибка: " + err.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
