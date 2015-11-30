using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DataLayer;

namespace RegionR.addedit
{
    public partial class AddRowReport : Form
    {
        int userchange = 0;
        int repID = 0;
        bool load = false;
        int idDB = 0;

        public AddRowReport()
        {
            InitializeComponent();

            dateTimePicker1.Value = Convert.ToDateTime(globalData.CurDate);

            loaddata();

            changeEnable();
        }

        public AddRowReport(String rep_id, String db_id, bool edit)
        {
            InitializeComponent();

            repID = Convert.ToInt32(rep_id);
            idDB = Convert.ToInt32(db_id);

            sql sql1 = new sql();
            DataTable dt1 = sql1.GetRecords("exec selRepKosByID @p1, @p2", rep_id, db_id);

            dateTimePicker1.Value = Convert.ToDateTime(dt1.Rows[0].ItemArray[0].ToString());
            
            loaddata();

            cbComp.SelectedValue = Convert.ToInt32(dt1.Rows[0].ItemArray[1].ToString());
            cbCust.SelectedValue = Convert.ToInt32(dt1.Rows[0].ItemArray[2].ToString());
            tbDist.Text = dt1.Rows[0].ItemArray[3].ToString();
            cbSubReg.SelectedValue = Convert.ToInt32(dt1.Rows[0].ItemArray[4].ToString());

            if (globalData.Div != "OM")
                cbLPU.SelectedValue = Convert.ToInt32(dt1.Rows[0].ItemArray[5].ToString());
                        
            if (Convert.ToInt32(dt1.Rows[0].ItemArray[6].ToString()) != 0)
            {
                cbGroup.SelectedValue = Convert.ToInt32(dt1.Rows[0].ItemArray[6].ToString());
                cbNom.SelectedValue = Convert.ToInt32(dt1.Rows[0].ItemArray[7].ToString());
            }
            else
                cbGroup.SelectedValue = Convert.ToInt32(dt1.Rows[0].ItemArray[7].ToString());   

            changeEnable();


            load = false;

            tbCount.Text = dt1.Rows[0].ItemArray[9].ToString();
            tbPriceEuro.Text = dt1.Rows[0].ItemArray[10].ToString();
            tbPriceRub.Text = dt1.Rows[0].ItemArray[11].ToString();
                       
            load = true;

            if (((!edit) || (dateTimePicker1.Value != Convert.ToDateTime(globalData.CurDate))) && (globalData.UserAccess != 1))
            {
                tbCount.Enabled = false;
                tbPriceEuro.Enabled = false;
                tbPriceRub.Enabled = false;
                cbGroup.Enabled = false;
                cbNom.Enabled = false;
            }
            else
            {
                tbCount.Enabled = true;
                tbPriceEuro.Enabled = true;
                tbPriceRub.Enabled = true;
                cbGroup.Enabled = true;
                cbNom.Enabled = true;
            }
        }

        private void loaddata()
        {
            sql sql1 = new sql();
            DataTable dt = sql1.GetRecords("exec SelUserByID @p1", globalData.UserID2);

            lbUserName.Text = dt.Rows[0].ItemArray[1].ToString();

            tbRate.Text = sql1.GetRecordsOne("exec GetRate @p1", dateTimePicker1.Value);            
                        
            cbCust.Items.Clear();
            cbRegCust.Items.Clear();
            cbSubReg.Items.Clear();
            cbLPU.Items.Clear();

            cbComp.DataSource = sql1.GetRecords("exec selComp");
            cbComp.DisplayMember = "comp_name";
            cbComp.ValueMember = "comp_id";

            dt = sql1.GetRecords("exec selKosCustByName @p1, @p2", globalData.Region, globalData.Div);

            cbCust.DataSource = dt;
            cbCust.DisplayMember = "cust_name";
            cbCust.ValueMember = "cust_id";
            if (cbCust.Items.Count > 0)
                cbCust.SelectedIndex = 0;
            
            cbRegCust.DataSource = dt;
            cbRegCust.DisplayMember = "reg_nameRus";
            cbRegCust.ValueMember = "reg_id";

            cbSubReg.DataSource = sql1.GetRecords("exec selSubRegion @p1", globalData.Region);
            cbSubReg.DisplayMember = "subreg_nameRus";
            cbSubReg.ValueMember = "subreg_id";

            cbLPU.DataSource = sql1.GetRecords("exec selLPUbySubReg @p1, @p2, @p3, @p4", globalData.UserID2, cbSubReg.SelectedValue, globalData.Div, globalData.CurDate.Year);
            cbLPU.DisplayMember = "lpu_sname";
            cbLPU.ValueMember = "ulpu_id";

            load = false;

            cbGroup.DataSource = sql1.GetRecords("exec SelGroupProd @p1", globalData.Div);
            cbGroup.DisplayMember = "nom_name";
            cbGroup.ValueMember = "nom_id";

            dt = sql1.GetRecords("exec SelNomProd @p1", cbGroup.SelectedValue);

            if (dt != null)
            {
                cbNom.DataSource = dt;
                cbNom.DisplayMember = "nom_name";
                cbNom.ValueMember = "nom_id";
            }

            if (dt.Rows.Count == 0)
            {
                cbNom.Visible = false;
                label22.Visible = false;
            }
            else
            {
                cbNom.Visible = true;
                label22.Visible = true;
            }

            load = true;

            if (globalData.Div == "AE")
            {
                label14.Visible = false;
                tbCount.Visible = false;
                tbCount.TabStop = false;
            }
            if (globalData.Div == "OM")
            {
                label14.Visible = true;
                tbCount.Visible = true;
                tbCount.TabStop = true;
                label22.Visible = false;
                label10.Visible = false;
                cbLPU.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void changeEnable()
        {
            if (load)
            {
                if (globalData.Div == "HC")
                {
                    sql sql1 = new sql();
                    String type;

                    if (cbNom.SelectedValue == null)
                        type = sql1.GetRecordsOne("exec SelNomType @p1", cbGroup.SelectedValue.ToString());
                    else
                        type = sql1.GetRecordsOne("exec SelNomType @p1", cbNom.SelectedValue.ToString());

                    if (type == null)
                    {
                        MessageBox.Show("Неудалось загрузить данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                    if (type == "euro")
                    {
                        tbPriceEuro.ReadOnly = false;
                        tbPriceRub.ReadOnly = false;

                        tbCount.Text = String.Empty;
                        tbCount.ReadOnly = true;
                    }
                    else
                    {
                        tbPriceEuro.Text = String.Empty;
                        tbPriceRub.Text = String.Empty;

                        tbPriceEuro.ReadOnly = true;
                        tbPriceRub.ReadOnly = true;
                        tbCount.ReadOnly = false;
                    }
                }

                if ((globalData.Div == "OM") && (cbNom.SelectedValue != null))
                {
                    sql sql1 = new sql();

                    String type = sql1.GetRecordsOne("exec SelNomType @p1", cbNom.SelectedValue.ToString());
                    
                    if (type == null)
                    {
                        MessageBox.Show("Неудалось загрузить данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                    if (type == "euro")
                    {
                        tbPriceEuro.ReadOnly = false;
                        tbPriceRub.ReadOnly = false;

                        tbCount.Text = String.Empty;
                        tbCount.ReadOnly = true;
                    }
                    else
                    {
                        tbPriceEuro.Text = String.Empty;
                        tbPriceRub.Text = String.Empty;

                        tbPriceEuro.ReadOnly = true;
                        tbPriceRub.ReadOnly = true;
                        tbCount.ReadOnly = false;
                    }
                }

                if ((globalData.Div == "OM") && (cbNom.SelectedValue == null))
                {
                    tbPriceEuro.ReadOnly = false;
                    tbPriceRub.ReadOnly = false;
                    tbCount.ReadOnly = false;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            sql sql1 = new sql();
            int count = 1;
            double sumEuro = 0, sumRub = 0;

            if (globalData.Div == "AE")
            {
                if (tbPriceEuro.Text != String.Empty)
                {
                    if (tbCount.Text == "0")
                        count = 1;
                    else if (tbCount.Text != String.Empty)
                        count = Convert.ToInt32(tbCount.Text);
                    if (tbPriceEuro.Text != String.Empty)
                        sumEuro = Convert.ToDouble(tbPriceEuro.Text);
                    if (tbPriceRub.Text != String.Empty)
                        sumRub = Convert.ToDouble(tbPriceRub.Text);

                    if (repID == 0)
                    {
                        if (cbNom.SelectedValue == null)
                            sql1.GetRecords("exec InsReportKos 0, 0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, 0, @p8, @p9, @p10, @p11, @p12, @p13, @p14", globalData.db_id, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), count, sumEuro, sumRub, globalData.UserID2, cbLPU.SelectedValue.ToString(), globalData.Div);
                        else
                            sql1.GetRecords("exec InsReportKos 0, 0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15", globalData.db_id, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), cbNom.SelectedValue.ToString(), count, sumEuro, sumRub, globalData.UserID2, cbLPU.SelectedValue.ToString(), globalData.Div);

                        MessageBox.Show("Косвенная продажа добавлена", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (cbNom.SelectedValue == null)
                            sql1.GetRecords("exec UpdReportKos @p1, @p2, @p3, @p4, @p5, @p6, @p7, 0, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15", repID, idDB, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), count, sumEuro, sumRub, globalData.UserID2, cbLPU.SelectedValue.ToString());
                        else
                            sql1.GetRecords("exec UpdReportKos @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15", repID, idDB, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), cbNom.SelectedValue.ToString(), count, sumEuro, sumRub, globalData.UserID2, cbLPU.SelectedValue.ToString());

                        MessageBox.Show("Косвенная продажа обновлена", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        globalData.load = true;
                        Close();
                        return;
                    }

                    tbDist.Text = "";
                    if (tbCount.Text != String.Empty)
                        tbCount.Text = "";
                    if (tbPriceEuro.Text != String.Empty)
                        tbPriceEuro.Text = "";
                    if (tbPriceRub.Text != String.Empty)
                        tbPriceRub.Text = "";

                    globalData.load = true;

                    return;
                }
                if ((tbPriceRub.Text != String.Empty) && (tbRate.Text != String.Empty))
                {
                    if (tbCount.Text == "0")
                        count = 1;
                    else if (tbCount.Text != String.Empty)
                        count = Convert.ToInt32(tbCount.Text);
                    if (tbPriceEuro.Text != String.Empty)
                        sumEuro = Convert.ToDouble(tbPriceEuro.Text);
                    if (tbPriceRub.Text != String.Empty)
                        sumRub = Convert.ToDouble(tbPriceRub.Text);

                    if (repID == 0)
                        sql1.GetRecords("exec InsReportKos 0, 0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15", globalData.db_id, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), cbNom.SelectedValue.ToString(), count, sumEuro, sumRub, globalData.UserID2, cbLPU.SelectedValue.ToString(), globalData.Div);
                    else
                    {
                        sql1.GetRecords("exec UpdReportKos @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16", repID, idDB, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), cbNom.SelectedValue.ToString(), count, sumEuro, sumRub, globalData.UserID2, cbLPU.SelectedValue.ToString());

                        MessageBox.Show("Косвенная продажа обновлена", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        globalData.load = true;
                        Close();
                        return;
                    }

                    tbDist.Text = "";
                    if (tbCount.Text != String.Empty)
                        tbCount.Text = "";
                    if (tbPriceEuro.Text != String.Empty)
                        tbPriceEuro.Text = "";
                    if (tbPriceRub.Text != String.Empty)
                        tbPriceRub.Text = "";

                    MessageBox.Show("Косвенная продажа добавлена", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    globalData.load = true;

                    return;
                }
                else if (tbPriceRub.Text == String.Empty)
                {
                    MessageBox.Show("Для сохранения необходимо ввести цену.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if ((tbCount.Text != String.Empty) || (tbPriceEuro.Text != String.Empty))
            {
                if (tbCount.Text == "0")
                    count = 1;
                else if (tbCount.Text != String.Empty)
                    count = Convert.ToInt32(tbCount.Text);

                if (tbPriceEuro.Text != String.Empty)
                    sumEuro = Convert.ToDouble(tbPriceEuro.Text);
                if (tbPriceRub.Text != String.Empty)
                    sumRub = Convert.ToDouble(tbPriceRub.Text);

                if (repID == 0)
                {
                    if (globalData.Div == "OM")
                    {
                        if (cbNom.SelectedValue == null)
                            sql1.GetRecords("exec InsReportKos 0, 0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, 0, @p8, @p9, @p10, @p11, @p12, 0, @p13", globalData.db_id, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), count, sumEuro, sumRub, globalData.UserID2, globalData.Div);
                        else
                            sql1.GetRecords("exec InsReportKos 0, 0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, 0, @p14", globalData.db_id, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), cbNom.SelectedValue.ToString(), count, sumEuro, sumRub, globalData.UserID2, globalData.Div);
                    }
                    else
                    {
                        if (cbNom.SelectedValue == null)
                            sql1.GetRecords("exec InsReportKos 0, 0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, 0, @p8, @p9, @p10, @p11, @p12, @p13, @p14", globalData.db_id, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), count, sumEuro, sumRub, globalData.UserID2, cbLPU.SelectedValue.ToString(), globalData.Div);
                        else
                            sql1.GetRecords("exec InsReportKos 0, 0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15", globalData.db_id, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), cbNom.SelectedValue.ToString(), count, sumEuro, sumRub, globalData.UserID2, cbLPU.SelectedValue.ToString(), globalData.Div);
                    }
                }
                else
                {
                    if (globalData.Div == "OM")
                    {
                        if (cbNom.SelectedValue == null)
                            sql1.GetRecords("exec UpdReportKos @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, 0, @p10, @p11, @p12, @p13, 0", repID, idDB, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), count, sumEuro, sumRub, globalData.UserID2);
                        else
                            sql1.GetRecords("exec UpdReportKos @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, 0", repID, idDB, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), cbNom.SelectedValue.ToString(), count, sumEuro, sumRub, globalData.UserID2);
                    }
                    else
                        if (cbNom.SelectedValue != null)
                            sql1.GetRecords("exec UpdReportKos @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15", repID, idDB, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), cbNom.SelectedValue.ToString(), count, sumEuro, sumRub, globalData.UserID2, cbLPU.SelectedValue.ToString());
                        else
                            sql1.GetRecords("exec UpdReportKos @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15", repID, idDB, dateTimePicker1.Value, cbComp.SelectedValue.ToString(), cbCust.SelectedValue.ToString(), tbDist.Text, cbRegCust.SelectedValue.ToString(), cbSubReg.SelectedValue.ToString(), cbGroup.SelectedValue.ToString(), 0, count, sumEuro, sumRub, globalData.UserID2, cbLPU.SelectedValue.ToString());

                    MessageBox.Show("Косвенная продажа обновлена", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    globalData.load = true;
                    Close();
                    return;
                }

                tbDist.Text = "";
                if (tbCount.Text != String.Empty)
                    tbCount.Text = "";
                if (tbPriceEuro.Text != String.Empty)
                    tbPriceEuro.Text = "";
                if (tbPriceRub.Text != String.Empty)
                    tbPriceRub.Text = "";

                MessageBox.Show("Косвенная продажа добавлена", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);

                globalData.load = true;
            }
            else if (tbPriceEuro.Text == String.Empty)
            {
                MessageBox.Show("Для сохранения необходимо ввести цену.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Для сохранения необходимо ввести количество.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            sql sql1 = new sql();
            DataTable dt = new DataTable();

            dt = sql1.GetRecords("exec selLPUbySubReg @p1, @p2", globalData.UserID2, cbSubReg.SelectedValue);

            if (dt != null)
            {
                cbLPU.DataSource = dt;
                cbLPU.DisplayMember = "lpu_name";
                cbLPU.ValueMember = "ulpu_id";
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            validation(e);
        }
        
        private void cbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                sql sql1 = new sql();

                DataTable dt = new DataTable();
                dt = sql1.GetRecords("exec SelNomProd @p1", cbGroup.SelectedValue);
                cbNom.DataSource = dt;
                cbNom.DisplayMember = "nom_name";
                cbNom.ValueMember = "nom_id";

                if (dt.Rows.Count == 0)
                {
                    cbNom.Visible = false;
                    label22.Visible = false;
                }
                else
                {
                    cbNom.Visible = true;
                    label22.Visible = true;
                }

                if (dt.Rows.Count == 0)
                    changeEnable();
            }
        }

        private void cbNom_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeEnable();
        }

        private void tbPriceEuro_KeyPress(object sender, KeyPressEventArgs e)
        {
            validation(e);
        }

        private void tbPriceRub_KeyPress(object sender, KeyPressEventArgs e)
        {
            validation(e);
        }

        private void validation(KeyPressEventArgs e)
        {
            if ((!(Char.IsDigit(e.KeyChar))) && (e.KeyChar != ','))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void tbPriceEuro_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (((userchange == 0) || (userchange == 3)) && load)
                {
                    if (tbPriceEuro.Text != "")
                    {
                        userchange = 3;
                        if (globalData.Div == "AE")
                        {
                            tbCount.ReadOnly = true;
                            tbPriceRub.ReadOnly = true;
                        }
                        double temp = Convert.ToDouble(tbPriceEuro.Text) * Convert.ToDouble(tbRate.Text);
                        tbPriceRub.Text = Math.Round(temp, 2).ToString();
                    }
                    else
                    {
                        userchange = 0;
                        tbPriceRub.Text = "";
                        if (globalData.Div == "AE")
                        {
                            tbCount.ReadOnly = false;
                            tbPriceRub.ReadOnly = false;
                        }
                    }
                }

                if (globalData.Div == "OM")
                    tbCount.ReadOnly = false;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void tbPriceRub_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (((userchange == 0) || (userchange == 4)) && load)
                {
                    if (tbPriceRub.Text != "")
                    {
                        userchange = 4;
                        if (globalData.Div == "AE")
                        {
                            tbCount.ReadOnly = true;
                            tbPriceEuro.ReadOnly = true;
                        }
                        double temp = Convert.ToDouble(tbPriceRub.Text) / Convert.ToDouble(tbRate.Text);
                        tbPriceEuro.Text = Math.Round(temp, 2).ToString();
                    }
                    else
                    {
                        userchange = 0;
                        tbPriceEuro.Text = "";
                        if (globalData.Div == "AE")
                        {
                            tbCount.ReadOnly = false;
                            tbPriceEuro.ReadOnly = false;
                        }
                    }
                }
                if (globalData.Div == "OM")
                    tbCount.ReadOnly = false;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void tbCount_TextChanged(object sender, EventArgs e)
        {            
            try
            {
                if (((userchange == 0) || (userchange == 2)) && load)
                {
                    if (tbCount.Text != "")
                    {
                        sql sql1 = new sql();
                        String str = "";

                        if (cbNom.SelectedValue != null)
                            str = sql1.GetRecordsOne("exec GetPrice @p1, @p2", cbNom.SelectedValue, globalData.CurDate.Year);
                        else
                            str = sql1.GetRecordsOne("exec GetPrice @p1, @p2", cbGroup.SelectedValue, globalData.CurDate.Year);
                        if (str != "")
                        {
                            userchange = 2;
                            double temp = Convert.ToDouble(str) * Convert.ToInt32(tbCount.Text);
                            tbPriceEuro.Text = Math.Round(temp, 2).ToString();

                            if (tbRate.Text == "")
                                MessageBox.Show("Для данного месяца не введён курс евро по-этому не возможно расчитать стоимость в рублях, пожалуйста, обратитесь в отдел Контроллинга", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                                tbPriceRub.Text = Math.Round((temp * Convert.ToDouble(tbRate.Text)), 2).ToString();
                        }
                        if (globalData.Div == "AE")
                        {
                            tbPriceEuro.ReadOnly = true;
                            tbPriceRub.ReadOnly = true;
                        }
                    }
                    else
                    {
                        userchange = 0;
                        tbPriceEuro.Text = "";
                        if (globalData.Div == "AE")
                        {
                            tbPriceEuro.ReadOnly = false;
                            tbPriceRub.ReadOnly = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}