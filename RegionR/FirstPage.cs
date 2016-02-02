using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Principal;
using DataLayer;

namespace RegionR
{
    public partial class FirstPage : Form
    {
        public FirstPage()
        {
            InitializeComponent();

            if (Connect() != 0)
                return;
        }

        private int Connect()
        {
            globalData.UserID = 0;
            globalData.UserID2 = 0;

            sql sql1 = new sql();
            
            if (sql1 == null)
            {
                DialogResult dr = MessageBox.Show("Не удалось подключится к серверу. Проверьте наличие интеренет-соединения.", "Ошибка", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (dr == System.Windows.Forms.DialogResult.Retry)
                    return 1;
                else
                    return 0;
            }

            String s1 = "";

            if ((globalData.admin) && (globalData.Login != String.Empty))
                s1 = globalData.Login;
            else
            {
                s1 = WindowsIdentity.GetCurrent().Name.Replace("\\", "-");
                String[] s2 = s1.Split('-');
                s1 = s2[1];
            }

            //s1 = "plotstru";

            DataTable dt1 = new DataTable();
            dt1 = sql1.GetRecords("exec UserLogin @p1, @p2", s1, globalData.UserAccess);
            if (dt1.Rows.Count == 0)
            {
                //MessageBox.Show("Пользователь не найден в системе. Вход будет произведён под учётной записью гостя.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                s1 = "anonimus";
            }

            if (s1 == "anonimus")
                dt1 = sql1.GetRecords("exec UserLogin @p1, @p2", s1, globalData.UserAccess);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("Не удалось войти в систему.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                String str = dt1.Rows[0].ItemArray[0].ToString();
                globalData.UserID = Convert.ToInt32(str);
                str = dt1.Rows[0].ItemArray[1].ToString();
                globalData.UserAccess = Convert.ToInt32(str);
                globalData.fp = Convert.ToInt32(dt1.Rows[0].ItemArray[4]);
                globalData.UserName = dt1.Rows[0].ItemArray[3].ToString();
                str = "Здравствуйте, " + globalData.UserName + ".";

                if ((globalData.UserAccess == 1) && (globalData.UserID == 1))
                {
                    str = "Рад видеть тебя, мой Создатель.";
                }

                str += "  Последний раз вы были в системе: " + sql1.GetRecordsOne("exec LastEvent @p1", globalData.UserID);

                lbUserName.Text = str;
                globalData.Login = s1;
            }

            if (globalData.UserAccess == 1)
            {
                globalData.admin = true;
                btnFR.Enabled = true;
            }

            if (globalData.UserID == 0)
                this.Close();

          
            return 0;
        }

        private void btnRR_Click(object sender, EventArgs e)
        {
            globalData.form = 1;
            this.Close();
        }

        private void btnFR_Click(object sender, EventArgs e)
        {
            globalData.form = 2;
            this.Close();
        }

        private void FirstPage_Load(object sender, EventArgs e)
        {
            globalData.form = 1;
            this.Close();

            // Финансовая отчётность

            //if (globalData.UserAccess != 1)
            //{
            //    globalData.form = 1;
            //    this.Close();
            //}
            //else
            //    this.Visible = true;
            
        }

        private void btnFR_MouseMove(object sender, MouseEventArgs e)
        {
            //Random r = new Random();
            //btnFR.Left = r.Next(0, this.ClientSize.Width - btnFR.Width);
            //btnFR.Top = r.Next(0, this.ClientSize.Height - btnFR.Height);
        }

       
    }
}
