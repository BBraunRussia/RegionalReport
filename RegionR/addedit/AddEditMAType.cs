using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.addedit
{
    public partial class AddEditMAType : Form
    {
        private string name_dafault = "Международная (Всероссийская) Пироговская научная медицинская конференция студентов и молодых ученых";
        private string sname_dafault = "Пироговская НМК";
        private string comm_dafault = "XI Международная (XX Всероссийской) Пироговская научная медицинская конференция студентов и молодых ученых";
        private string site_dafault = "http://pirogovka.rsmu.ru/";

        private int _conf = 0;
        private string _name = "";
        private string _sname = "";
        private string _site = "";
        private string _theme = "";
        private string _type = "";
        private int _edit = 0;

        public AddEditMAType()
        {
            InitializeComponent();
            
            button1.Visible = false;
            
            loadType();
            loadTheme();
            loadCountry();
            loadCity();
        }

        public AddEditMAType(int conf, string sname, string name, string site, string theme, string type)
        {
            InitializeComponent();

            button1.Visible = false;
            
            _conf = conf;
            _name = name;
            _sname = sname;
            _site = site;
            _theme = theme;
            _type = type;
            
            loadType();
            loadTheme();
            loadCountry();
            loadCity();

            tbName.Text = _name;
            tbSname.Text = _sname;
            tbSite.Text = _site;

            cbTheme.Text = _theme;
            cbTypeMA.Text = _type;
        }


        public AddEditMAType(int conf, string sname, string name, string site, string theme, string type, int flag_edit)
        {
            InitializeComponent();

            button1.Visible = false;

            _conf = conf;
            _name = name;
            _sname = sname;
            _site = site;
            _theme = theme;
            _type = type;
            _edit = flag_edit;

            if (flag_edit == 1)
                button1.Visible = true;

            loadType();
            loadTheme();
            loadCountry();
            loadCity();

            tbName.Text = _name;
            tbSname.Text = _sname;
            tbSite.Text = _site;

            cbTheme.Text = _theme;
            cbTypeMA.Text = _type;
        }


        void loadType()
        {
            sql sql1 = new sql();

            globalData.update = false;
            cbTypeMA.DataSource = sql1.GetRecords("exec MarkAct_Sel_Type @p1", -1);
            cbTypeMA.DisplayMember = "matype_name";
            cbTypeMA.ValueMember = "matype_id";
            globalData.update = true;
        }

        void loadTheme()
        {
            sql sql1 = new sql();

            globalData.update = false;
            cbTheme.DataSource = sql1.GetRecords("exec MarkAct_Select_ConfTheme");
            cbTheme.DisplayMember = "confth_name";
            cbTheme.ValueMember = "confth_id";
            globalData.update = true;
        }

        void loadCountry()
        {
            sql sql1 = new sql();

            globalData.update = false;
            cbCountry.DataSource = sql1.GetRecords("exec MarkAct_Select_Country @p1", 1);
            cbCountry.DisplayMember = "country_name";
            cbCountry.ValueMember = "country_id";
            globalData.update = true;
        }

        void loadCity()
        {
            globalData.update = false;
            sql sql1 = new sql();
            int country = 0;

            if (cbCountry.SelectedValue != null)
                country = Convert.ToInt32(cbCountry.SelectedValue);

            cbCity.DataSource = sql1.GetRecords("exec MarkAct_Select_City @p1, @p2", country, 1);
            cbCity.DisplayMember = "city_name";
            cbCity.ValueMember = "city_id";
            globalData.update = true;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            if (tbName.Text == name_dafault)
                tbName.Text = String.Empty;
        }

        private void tbSname_TextChanged(object sender, EventArgs e)
        {
            if (tbSname.Text == sname_dafault)
                tbSname.Text = String.Empty;
        }

        private void tbComm_TextChanged(object sender, EventArgs e)
        {
            if (tbComm.Text == comm_dafault)
                tbComm.Text = String.Empty;
        }

        private void tbSite_TextChanged(object sender, EventArgs e)
        {
            if (tbSite.Text == site_dafault)
                tbSite.Text = String.Empty;

        }

        string Check()
        {
            if (tbName.Text.Trim() == String.Empty || tbName.Text == name_dafault)
                return "Введите полное название!";
            if (tbSname.Text.Trim() == String.Empty || tbSname.Text == sname_dafault)
                return "Введите сокращенное название!";
            if (tbSite.Text.Trim() == String.Empty || tbSite.Text == site_dafault)
                return "Введите сайт!";            
            if (cbTheme.SelectedIndex == 0)
                return "Выберите тематику!";           
            if (tbComm.Text.Trim() == String.Empty || tbComm.Text == comm_dafault)
                return "Введите примечание!";
            
            return "Успех";
        }

        void SavePlanConf()
        {
            sql sql1 = new sql();

            string res = sql1.GetRecordsOne("exec MarkAct_Insert_Conf @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10",
                dtpMA.Value.Year.ToString() + "-" + dtpMA.Value.Month.ToString() + "-01",
                cbTypeMA.SelectedValue,
                tbName.Text.Trim(),
                tbSname.Text.Trim(),
                tbComm.Text.Trim(),
                tbSite.Text.Trim(),
                cbTheme.SelectedValue,
                cbCity.SelectedValue,
                _conf,
                globalData.maplan);

            if (res != "0" && res != "")
            {
                globalData.maplan = Convert.ToInt32(res);
                MessageBox.Show("Успешно сохранено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
                MessageBox.Show("Не сохранено!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void SaveTheme()
        {
            sql sql1 = new sql();

            sql1.GetRecords("exec MarkAct_Insert_Theme @p1", tbTheme.Text.Trim());
        }

        void SaveCity()
        {
            sql sql1 = new sql();

            sql1.GetRecords("exec MarkAct_Insert_City @p1, @p2", cbCountry.SelectedValue, tbCity.Text.Trim());
        }

        void SaveCountry()
        {
            sql sql1 = new sql();

            sql1.GetRecords("exec MarkAct_Insert_Country @p1", tbCountry.Text.Trim());
        }


        private void btnTheme_Click(object sender, EventArgs e)
        {
            if (tbTheme.Text.Trim() == String.Empty)
                MessageBox.Show("Вы не ввели новую тематику!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                SaveTheme();
                loadTheme();
                cbTheme.SelectedText = tbTheme.Text;
                tbTheme.Text = String.Empty;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string res = Check();

            if (res != "Успех")
                MessageBox.Show(res, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                SavePlanConf();


        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCountry_Click(object sender, EventArgs e)
        {

            if (tbCountry.Text.Trim() == String.Empty)
                MessageBox.Show("Вы не ввели новую страну!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                SaveCountry();
                loadCountry();
                cbCountry.SelectedText = tbCountry.Text;
                tbCountry.Text = String.Empty;
            }
        }

        private void btnCity_Click(object sender, EventArgs e)
        {

            if (tbCity.Text.Trim() == String.Empty)
                MessageBox.Show("Вы не ввели новый город!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                SaveCity();
                loadCity();
                cbCity.SelectedText = tbCity.Text;
                tbCity.Text = String.Empty;
            }
        }

        private void cbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
                loadCity();
        }

        private void tbName_Click(object sender, EventArgs e)
        {
            if (tbName.Text == name_dafault)
                tbName.Text = String.Empty;
        }

        private void tbSname_Click(object sender, EventArgs e)
        {
            if (tbSname.Text == sname_dafault)
                tbSname.Text = String.Empty;
        }

        private void tbComm_Click(object sender, EventArgs e)
        {
            if (tbComm.Text == comm_dafault)
                tbComm.Text = String.Empty;     
        }

        private void tbSite_Click(object sender, EventArgs e)
        {
            if (tbSite.Text == site_dafault)
                tbSite.Text = String.Empty;     
        }

        private void button1_Click(object sender, EventArgs e)
        {           
             SavePlanConf();
        }
    }
}
