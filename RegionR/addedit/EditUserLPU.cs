using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataLayer;
using ClassLibrary;

namespace RegionR.addedit
{
    public partial class EditUserLPU : Form
    {
        private UserLpuRR _userLPU;

        public EditUserLPU(UserLpuRR userLPU)
        {
            InitializeComponent();

            _userLPU = userLPU;
        }

        private void EditUserLPU_Load(object sender, EventArgs e)
        {
            tbReg.Text = _userLPU.LpuRR.RegionRR.Name;
            tbLPU.Text = _userLPU.LpuRR.Name;
            tbYear1.Text = _userLPU.YearBegin.ToString();
            tbYear2.Text = _userLPU.YearEnd.ToString();

            Sql sql1 = new Sql();

            DataTable dt1 = sql1.GetRecords("exec SelUsersAP @p1, @p2", _userLPU.Sdiv.ToString(), _userLPU.LpuRR.RegionRR.ID);
            cbUsers.DataSource = dt1;
            cbUsers.DisplayMember = "user_name";
            cbUsers.ValueMember = "user_id";

            cbUsers.SelectedValue = _userLPU.User.ID;
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            CopyFields();

            string res = _userLPU.Save(UserLogged.Get());

            if (res == "1")
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Перемещение не возможно, так как " + res, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyFields()
        {
            int yearBegin;
            int.TryParse(tbYear1.Text, out yearBegin);
            _userLPU.YearBegin = yearBegin;

            int yearEnd;
            int.TryParse(tbYear2.Text, out yearEnd);
            _userLPU.YearEnd = yearEnd;

            UserList userList = UserList.GetUniqueInstance();
            int idUser;
            int.TryParse(cbUsers.SelectedValue.ToString(), out idUser);
            User user = userList.GetItem(idUser) as User;

            _userLPU.User = user;
        }
    }
}
