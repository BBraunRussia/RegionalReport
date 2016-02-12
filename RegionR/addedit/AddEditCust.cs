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
    public partial class AddEditCust : Form
    {
        string id;

        public AddEditCust()
        {
            InitializeComponent();

            tbCustCode.Text = "";
            tbCustName.Text = "";
            tbShipTo.Text = "";
            tbPayer.Text = "";
            tbPlant.Text = "";
            tbDistChan.Text = "";

            id = "0";

            loadRegion();

            this.Text = "Добавление покупателя";
        }

        public AddEditCust(string id1)
        {
            InitializeComponent();

            id = id1;

            loadData();

            this.Text = "Редактирование покупателя";
        }

        private void loadData()
        {
            loadRegion();

            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec GetCustByID @p1", id);

            if (dt1 != null)
            {
                tbCustCode.Text = dt1.Rows[0].ItemArray[1].ToString();
                tbCustName.Text = dt1.Rows[0].ItemArray[2].ToString();
                tbShipTo.Text = dt1.Rows[0].ItemArray[3].ToString();
                tbPayer.Text = dt1.Rows[0].ItemArray[4].ToString();
                tbPlant.Text = dt1.Rows[0].ItemArray[5].ToString();
                tbDistChan.Text = dt1.Rows[0].ItemArray[6].ToString();

                if (dt1.Rows[0].ItemArray[7].ToString() != "0")
                    cbReg.SelectedValue = Convert.ToInt32(dt1.Rows[0].ItemArray[7]);
            }
        }

        private void loadRegion()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec Region_Select '', '', 1");

            cbReg.DataSource = dt1;
            cbReg.DisplayMember = "reg_nameRus";
            cbReg.ValueMember = "reg_id";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();
            
            if (tbCustCode.Text == "")
            {
                MessageBox.Show("Введите код покупателя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tbCustName.Text == "")
            {
                MessageBox.Show("Введите название покупателя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sql1.GetRecords("exec InsCustomer @p1, @p2, @p3, @p4, @p5, @p6, @p7", tbCustCode.Text, tbCustName.Text, tbShipTo.Text, 
                tbPayer.Text, tbPlant.Text, tbDistChan.Text, cbReg.SelectedValue);

            Close();
            globalData.update = true;
        }
    }
}
