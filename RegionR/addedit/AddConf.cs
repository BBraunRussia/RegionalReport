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
    public partial class AddConf : Form
    {
        private bool flagAE = false;


        public AddConf()
        {
            InitializeComponent();

            btnSearchConf.BackColor = Color.FromArgb(115, 214, 186);
            
            globalData.maplan = 0;
            
            label7.Visible = flagAE;
            comboBox3.Visible = flagAE;

            loadCountry();
            loadCity();
            loadConf();
            loadTheme();
        }

        public AddConf( int conf, bool flag = false )
        {
            InitializeComponent();

            btnSearchConf.BackColor = Color.FromArgb(115, 214, 186);

            label6.Visible = flag;
            label7.Visible = flag;
            label8.Visible = flag;

            tbSearchConf.Visible = flag;
            comboBox3.Visible    = flag;
            comboBox4.Visible    = flag;

            btnSearchConf.Visible = flag;

            comboBox1.Enabled = flag;
            comboBox2.Enabled = flag;
                                    
            loadCountry();
            loadCity();
            loadPlan(conf);
           
        }



        void loadPlan(int conf)
        {
            sql sql1 = new sql();
            _dgvConf.DataSource = sql1.GetRecords("exec MarkAct_Select_Plan @p1, @p2, @p3", conf, globalData.year, 2);
            _dgvConf.Columns[0].Visible = false;
            _dgvConf.Columns[1].DefaultCellStyle.Format = "MM - yyyy";

        }

        public AddConf(bool _flagAE = false)
        {
            InitializeComponent();

            flagAE = _flagAE;

            globalData.maplan = 0;

            btnSearchConf.BackColor = Color.FromArgb(115, 214, 186);
            label7.Visible = flagAE;
            comboBox3.Visible = flagAE;

            loadCountry();
            loadCity();
            loadConf();
            loadTheme();
        }

        void loadCountry()
        {
            sql sql1 = new sql();

            globalData.update = false;
            comboBox1.DataSource = sql1.GetRecords("exec MarkAct_Select_Country");
            comboBox1.DisplayMember = "country_name";
            comboBox1.ValueMember = "country_id";
            globalData.update = true;
        }
        
        void loadTheme()
        {
            sql sql1 = new sql();

            globalData.update = false;
            comboBox3.DataSource = sql1.GetRecords("exec MarkAct_Select_ConfTheme");
            comboBox3.DisplayMember = "confth_name";
            comboBox3.ValueMember = "confth_id";
            globalData.update = true;
        }
        
        void loadCity()
        {
            globalData.update = false;
            sql sql1 = new sql();
            int country = 0;
            
            if (comboBox1.SelectedValue != null)
                country = Convert.ToInt32(comboBox1.SelectedValue);

            comboBox2.DataSource = sql1.GetRecords("exec MarkAct_Select_City @p1", country);
            comboBox2.DisplayMember = "city_name";
            comboBox2.ValueMember = "city_id";
            globalData.update = true;
        }

        void loadConf()
        {
            sql sql1 = new sql();

            globalData.update = false;

            int type = 0;
            int theme = 0;



            if (flagAE == true)
            {
                type = 8;
                if (comboBox3.SelectedValue != null && comboBox3.SelectedIndex != 0)
                    theme = Convert.ToInt32(comboBox3.SelectedValue);
            }

            string text_search = String.Empty;

            if (tbSearchConf.Text != String.Empty)
                text_search = tbSearchConf.Text;

            _dgvConf.DataSource = sql1.GetRecords("exec MarkAct_Select_Conf @p1, @p2, @p3, @p4, @p5, @p6, @p7", 
                dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month.ToString() + "-" + "01",
                dateTimePicker3.Value.Year.ToString() + "-" + dateTimePicker3.Value.Month.ToString() + "-" + "01",
                comboBox1.SelectedValue, comboBox2.SelectedValue, text_search, type, theme);
            

            globalData.update = true;

            _dgvConf.Columns[0].Visible = false;
            _dgvConf.Columns[1].DefaultCellStyle.Format = "MM - yyyy";

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
            {
                loadCity();
                loadConf();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
                loadConf();
        }

        private void _dgvConf_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                globalData.maplan = Convert.ToInt32(_dgvConf[0, e.RowIndex].Value.ToString());
                this.Close();
            }
        }

        private void btnSearchConf_Click(object sender, EventArgs e)
        {
            loadConf();
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loadConf();
            }
        }

        private void dateTimePicker3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loadConf();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
                loadConf();
        }

        private void tbSearchConf_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loadConf();
            }
        }
    }
}
