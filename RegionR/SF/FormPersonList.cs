using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF;
using RegionR.SF;

namespace RegionR
{
    public partial class FormPersonList : Form
    {
        private MyStatusStrip _myStatusStrip;
        private SearchInDgv _seacher;

        public FormPersonList()
        {
            InitializeComponent();

            _seacher = new SearchInDgv(dgv);
            _myStatusStrip = new MyStatusStrip(dgv, statusStrip1);
        }
        
        private void FormPersonList_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            PersonList personList = PersonList.GetUniqueInstance();
            UserList userList = UserList.GetUniqueInstance();
            User user = userList.GetItem(globalData.UserID) as User;

            DataTable dt = (user.Role == Roles.Администратор) ? personList.ToDataTable() : personList.ToDataTable();//user);

            dgv.DataSource = dt;

            if (dgv.Columns.Count > 0)
                dgv.Columns[0].Visible = false;
        }

        private void NotImpliment_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            _myStatusStrip.writeStatus();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            _seacher.Find(tbSearch.Text);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void Add()
        {
            Person person = new Person();

            FormFirstStepAddPerson formFirstStepAddPerson = new FormFirstStepAddPerson(person);
            if (formFirstStepAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FormSecondStepAddPerson formSecondStepAddPerson = new FormSecondStepAddPerson(person);
                if (formSecondStepAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                }
            }
        }
    }
}
