using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF;

namespace RegionR.SF
{
    public partial class FormAddCity : Form
    {
        private City _city;

        public FormAddCity(City city)
        {
            InitializeComponent();

            _city = city;
        }

        private void FormAddCity_Load(object sender, EventArgs e)
        {
            lbRealRegion.Text = string.Concat("Регион России: ", _city.RealRegion.Name);
            tbName.Text = _city.Name;
            tbPhoneCode.Text = _city.PhoneCode;

            UserList userList = UserList.GetUniqueInstance();
            User user = userList.GetItem(globalData.UserID) as User;

            if ((user.Role == Roles.Администратор) || (user.Role == Roles.Руководство1) || (user.Role == Roles.Руководство2))
            {
                tbName.ReadOnly = false;
                tbPhoneCode.ReadOnly = false;
                btnSave.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save();

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Save()
        {
            ClassForForm.CheckFilled(tbName.Text, "Название");

            _city.Name = tbName.Text;

            CityList cityList = CityList.GetUniqueInstance();
            if (cityList.IsInList(_city))
            {
                throw new NullReferenceException("Населенный пункт с таким название уже присутствует в списке");
            }
            
            _city.PhoneCode = tbPhoneCode.Text;
            _city.Save();
        }
    }
}
