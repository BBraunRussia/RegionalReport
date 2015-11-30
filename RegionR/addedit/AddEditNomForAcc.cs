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
    public partial class AddEditNomForAcc : Form
    {
        public AddEditNomForAcc()
        {
            InitializeComponent();
            fillNom();

            label6.Text = "Дилерская цена за " + globalData.input + " год:";
            cbType.SelectedIndex = 0;

            idNom = "0";
        }

        string idNom;

        public AddEditNomForAcc(string nom_id, string Name, string nom_group, string type, string dilCost, string nom_seq, string nom_year1, string nom_year2)
        {
            InitializeComponent();
            fillNom();

            label6.Text = "Дилерская цена за " + globalData.input + " год:";
            cbType.SelectedIndex = 0;

            idNom = nom_id;

            tbName.Text = Name;

            if (nom_group == "")
                cbNomGroup.SelectedValue = 0;
            else
                cbNomGroup.SelectedValue = Convert.ToInt32(nom_group);
            
            if (type == "шт")
                cbType.SelectedIndex = 0;
            else
                cbType.SelectedIndex = 1;

            tbDilCost.Text = dilCost;
            tbSeq.Text = nom_seq;
            dtYear1.Value = Convert.ToDateTime(nom_year1 + "-01-01");
            dtYear2.Value = Convert.ToDateTime(nom_year2 + "-01-01");

            tbYearDilCost.Text = globalData.input;
        }

        public void fillNom()
        {
            sql sql1 = new sql();
            DataTable dt = sql1.GetRecords("exec SelNomGroup @p1", globalData.Div);

            if (dt != null)
            {
                cbNomGroup.DataSource = dt;
                cbNomGroup.DisplayMember = "nom_name";
                cbNomGroup.ValueMember = "nom_id";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbName.Text != "")
            {
                sql sql1 = new sql();

                if (idNom == "0")
                    sql1.GetRecords("exec InsNom @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8", tbName.Text, cbNomGroup.SelectedValue, cbType.SelectedItem, tbDilCost.Text.Replace(',', '.'), tbSeq.Text, dtYear1.Value.Year, dtYear2.Value.Year, globalData.Div);
                else
                    sql1.GetRecords("exec UpdNom @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8", idNom, tbName.Text, cbNomGroup.SelectedValue, cbType.SelectedItem, tbDilCost.Text.Replace(',', '.'), tbSeq.Text, dtYear1.Value.Year, dtYear2.Value.Year);

            }
        }

        private void btnUpdateDilCost_Click(object sender, EventArgs e)
        {
            if ((tbYearDilCost.Text != String.Empty) && (tbDilCost.Text != String.Empty) && (idNom != "0"))
            {
                sql sql1 = new sql();
                sql1.GetRecords("exec UpdDilCost @p1, @p2, @p3", idNom, tbDilCost.Text.Replace(',', '.'), tbYearDilCost.Text);
            }
            else
                MessageBox.Show("Проверьте заполненность полей", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
