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
    public partial class UserMessage : Form
    {
        public UserMessage()
        {
            InitializeComponent();
        }

        public UserMessage(string mesId, string userName, string header, string text)
        {
            InitializeComponent();

            cbRead.Checked = false;
            mes_id = mesId;
            this.Text = header;
            lbUserName.Text = "Автор: " + userName;
            tbMesTxt.Text = text;
        }

        string mes_id = "0";

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(mes_id != "0")
            {
                sql sql1 = new sql();

                if (cbRead.Checked)
                    sql1.GetRecords("exec UpdMesHide @p1", mes_id);
                else
                    sql1.GetRecords("exec UpdMesRead @p1", mes_id);
            }
        }
    }
}
