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
    public partial class fUser : Form
    {
        public fUser(String fio, String login, String role)
        {
            InitializeComponent();
            sql sql1 = new sql();

            DataTable dt1 = new DataTable();
            dt1 = sql1.GetRecords("exec GetUserReg @p1", fio);

            if (dt1.Rows.Count > 0)
            {
                _dgv1.DataSource = dt1;

                lbNameTable.Visible = true;
                _dgv1.Visible = true;
                lbRD.Visible = true;
                lbRDName.Visible = true;

                lbRD.Text = sql1.GetRecordsOne("exec GetRDbyUserName @p1", fio);

                _dgv1.Columns["reg_nameRus"].ReadOnly = true;
                _dgv1.Columns["reg_nameRus"].Width = 170;

                if ((role == "Региональный представитель") || (role == "Региональный менеджер"))
                    lbNameTable.Text = "Регионы продаж";
                else if (role == "Региональный директор")
                    lbNameTable.Text = "Cписок подведомственных регионов";
            }
            else
            {
                lbNameTable.Visible = false;
                lbRD.Visible = false;
                lbRDName.Visible = false;
                _dgv1.Visible = false;
            }

            lbFIO.Text = fio;
            lbLogin.Text = login;
            lbRole.Text = role;

            dt1 = sql1.GetRecords("exec GetDivByUserName @p1", fio);
            String temp = "";
            foreach (DataRow row in dt1.Rows)
            {
                if (temp != String.Empty)
                    temp += ", ";
                temp += row[0].ToString();
            }
            lbDiv.Text = temp;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}