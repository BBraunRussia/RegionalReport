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
    public partial class AddEditMat : Form
    {
        public AddEditMat()
        {
            InitializeComponent();

            loaddata();
        }

        public AddEditMat(string matID)
        {
            InitializeComponent();

            mat_id = matID;

            loaddata();
        }

        string mat_id = "0";

        private void loaddata()
        {
            sql sql1 = new sql();

            globalData.load = false;

            cbSDiv.DataSource = sql1.GetRecords("exec SelSDiv");
            cbSDiv.DisplayMember = "sdiv_code";
            cbSDiv.ValueMember = "sdiv_id";

            globalData.load = true;

            loadPDiv();
            loadHier();
            loadNom();
            loadButton();

            if (mat_id != "0")
            {
                DataTable dt1 = sql1.GetRecords("exec selMaterialByID @p1", mat_id);

                tbMatCode.Text = dt1.Rows[0].ItemArray[1].ToString();
                tbMatName.Text = dt1.Rows[0].ItemArray[2].ToString();
                cbSDiv.SelectedValue = dt1.Rows[0].ItemArray[3].ToString();
                cbPDiv.SelectedValue = dt1.Rows[0].ItemArray[4].ToString();
                cbSBA.SelectedValue = dt1.Rows[0].ItemArray[5].ToString();
                cbMMG.SelectedValue = dt1.Rows[0].ItemArray[6].ToString();
                cbMSG.SelectedValue = dt1.Rows[0].ItemArray[7].ToString();
                cbNom.SelectedValue = dt1.Rows[0].ItemArray[8].ToString();
                cbButton.SelectedValue = dt1.Rows[0].ItemArray[9].ToString();
            }
        }

        private void loadNom()
        {
            sql sql1 = new sql();
            cbNom.DataSource = sql1.GetRecords("exec SelNom @p1", cbSDiv.SelectedValue);
            cbNom.DisplayMember = "nom_name";
            cbNom.ValueMember = "nom_id";
        }

        private void loadHier()
        {
            sql sql1 = new sql();

            cbSBA.DataSource = sql1.GetRecords("exec SelSBAForMat @p1, @p2", cbSDiv.SelectedValue, cbPDiv.SelectedValue);
            cbSBA.DisplayMember = "sba";
            cbSBA.ValueMember = "sba_id";

            cbMMG.DataSource = sql1.GetRecords("exec SelMMGForMat @p1, @p2", cbSDiv.SelectedValue, cbPDiv.SelectedValue);
            cbMMG.DisplayMember = "mmg";
            cbMMG.ValueMember = "mmg_id";

            cbMSG.DataSource = sql1.GetRecords("exec SelMSGForMat @p1, @p2", cbSDiv.SelectedValue, cbPDiv.SelectedValue);
            cbMSG.DisplayMember = "msg";
            cbMSG.ValueMember = "msg_id";
        }

        private void loadPDiv()
        {
            sql sql1 = new sql();

            globalData.load = false;

            cbPDiv.DataSource = sql1.GetRecords("exec SelPDiv @p1", cbSDiv.SelectedValue);
            cbPDiv.DisplayMember = "pdiv_code";
            cbPDiv.ValueMember = "pdiv_id";

            globalData.load = true;
        }

        private void loadButton()
        {
            sql sql1 = new sql();

            cbButton.DataSource = sql1.GetRecords("exec SelButton @p1", cbSDiv.SelectedValue);
            cbButton.DisplayMember = "btn_name";
            cbButton.ValueMember = "btn_id";
        }

        private void cbSDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                loadPDiv();
                loadHier();
                loadNom();
                loadButton();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            sql sql1 = new sql();

            if (mat_id == "0")
            {
                sql1.GetRecords("exec InsMat @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9", tbMatCode.Text, tbMatName.Text, cbSDiv.SelectedValue, cbPDiv.SelectedValue, cbSBA.SelectedValue, cbMMG.SelectedValue, cbMSG.SelectedValue, cbButton.SelectedValue, cbNom.SelectedValue);
                MessageBox.Show("Материал добавлен", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                sql1.GetRecords("exec UpdMat @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10", mat_id, tbMatCode.Text, tbMatName.Text, cbSDiv.SelectedValue, cbPDiv.SelectedValue, cbSBA.SelectedValue, cbMMG.SelectedValue, cbMSG.SelectedValue, cbButton.SelectedValue, cbNom.SelectedValue);
                MessageBox.Show("Информация по материалу обновлена", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            globalData.update = true;
        }

        private void cbPDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                loadHier();
            }
        }
    }
}