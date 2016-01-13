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
    public partial class FormAddOtherOrganization : Form
    {
        private OtherOrganization _organization;

        private RealRegionList _realRegionList;
        private CityList _cityList;

        private bool _isLoad;

        public FormAddOtherOrganization(OtherOrganization organization)
        {
            InitializeComponent();

            _organization = organization;

            _realRegionList = RealRegionList.GetUniqueInstance();
            _cityList = CityList.GetUniqueInstance();
        }

        private void FormAddOtherOrganization_Load(object sender, EventArgs e)
        {
            LoadDictionaries();

            LoadData();
        }

        private void LoadData()
        {
            string typeOrg = _organization.TypeOrg.ToString();

            if (_organization.TypeOrg == TypeOrg.Административное_Учреждение)
                typeOrg = "Административное Учреждение";

            this.Text = string.Concat("Карточка Организации \"", typeOrg, "\"");
            lbOrganization.Text = typeOrg;
            lbOrganizationName.Text = _organization.ShortName;

            if (_organization.TypeOrg == TypeOrg.Аптека)
            {
                lbOrganizationName.Location = new Point(93, 9);

                gbCategory.Visible = true;

                if (_organization.Pharmacy == "A")
                    rbA.Checked = true;
                else if (_organization.Pharmacy == "B")
                    rbB.Checked = true;
                else if (_organization.Pharmacy == "C")
                    rbC.Checked = true;
            }
            else if (_organization.TypeOrg == TypeOrg.Дистрибьютор)
                lbOrganizationName.Location = new Point(157, 9);
            else if (_organization.TypeOrg == TypeOrg.Административное_Учреждение)
                lbOrganizationName.Location = new Point(307, 9);

            lbNumberSF.Text = _organization.NumberSF;
            lbTypeOrg.Text = _organization.TypeOrg.ToString();

            tbName.Text = _organization.Name;
            tbShortName.Text = _organization.ShortName;
            tbINN.Text = _organization.INN;
            tbKPP.Text = _organization.KPP;
            tbDistrict.Text = _organization.District;
            tbPostIndex.Text = _organization.PostIndex;
            tbStreet.Text = _organization.Street;
            tbEmail.Text = _organization.Email;
            tbWebSite.Text = _organization.WebSite;
            tbPhone.Text = _organization.Phone;

            if (_organization.RealRegion != null)
                cbRealRegion.SelectedValue = _organization.RealRegion.ID;

            if (_organization.City != null)
            {
                cbCity.SelectedValue = _organization.City.ID;
                tbPhoneCode.Text = _organization.City.PhoneCode;
            }
        }

        private void LoadDictionaries()
        {
            _isLoad = false;
            ClassForForm.LoadDictionary(cbRealRegion, _realRegionList.ToDataTable());
            _isLoad = true;
            LoadCity();
        }

        private void LoadCity()
        {
            int idRealRegion = Convert.ToInt32(cbRealRegion.SelectedValue);
            RealRegion realRegion = _realRegionList.GetItem(idRealRegion) as RealRegion;

            DataTable dt = _cityList.ToDataTable(realRegion);
            ClassForForm.LoadDictionary(cbCity, dt);
        }

        private void cbRealRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoad)
                LoadCity();
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            if (TrySave())
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TrySave();
        }

        private bool TrySave()
        {
            try
            {
                CopyFields();

                _organization.Save();

                return true;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        private void CopyFields()
        {
            ClassForForm.CheckFilled(tbName.Text, "Официальное название");
            ClassForForm.CheckFilled(tbShortName.Text, "Сокращенное название");
            ClassForForm.CheckFilled(tbINN.Text, "ИНН");
            ClassForForm.CheckFilled(tbStreet.Text, "Уличный адрес");

            if ((tbINN.Text.Length != 10) && (tbINN.Text.Length != 12))
                throw new NullReferenceException("Поле ИНН должно содержать 10 или 12 цифр");

            if ((tbINN.Text != _organization.INN) && (tbINN.Text.Length == 12))
            {
                if (MessageBox.Show("Данная организация является ИП?", "ИНН содержит 12 цифр", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    throw new NullReferenceException("Перед сохранением необходимо исправить поле ИНН");
            }

            _organization.Name = tbName.Text;
            _organization.ShortName = tbShortName.Text;
            _organization.INN = tbINN.Text;
            _organization.KPP = tbKPP.Text;
            _organization.PostIndex = tbPostIndex.Text;
            _organization.Email = tbEmail.Text;
            _organization.WebSite = tbWebSite.Text;
            _organization.Phone = tbPhone.Text;

            int idCity = Convert.ToInt32(cbCity.SelectedValue);
            _organization.City = _cityList.GetItem(idCity) as City;

            _organization.District = tbDistrict.Text;
            _organization.Street = tbStreet.Text;

            if (_organization.TypeOrg == TypeOrg.Аптека)
            {
                if (rbA.Checked)
                    _organization.Pharmacy = "A";
                else if (rbB.Checked)
                    _organization.Pharmacy = "B";
                else if (rbC.Checked)
                    _organization.Pharmacy = "C";
            }
        }

        private void btnShowRules_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tbShortName_TextChanged(object sender, EventArgs e)
        {
            lbOrganizationName.Text = tbShortName.Text.ToUpper();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (IsHaveChanges())
            {
                if (MessageBox.Show("На форме имеются не сохранёные изменения, сохранить перед закрытием?", "Сохраненить изменения", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (TrySave())
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        return;
                    }
                    else
                        return;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private bool IsHaveChanges()
        {
            if (_organization.Name != tbName.Text)
                return true;
            if (_organization.ShortName != tbShortName.Text)
                return true;

            if (_organization.INN != tbINN.Text)
                return true;

            if (_organization.KPP != tbKPP.Text)
                return true;
            if (_organization.PostIndex != tbPostIndex.Text)
                return true;
            if (_organization.Email != tbEmail.Text)
                return true;
            if (_organization.WebSite != tbWebSite.Text)
                return true;
            if (_organization.Phone != tbPhone.Text)
                return true;

            int idCity = Convert.ToInt32(cbCity.SelectedValue);
            if (_organization.City != (_cityList.GetItem(idCity) as City))
                return true;

            if (_organization.District != tbDistrict.Text)
                return true;
            if (_organization.Street != tbStreet.Text)
                return true;

            if (_organization.TypeOrg == TypeOrg.Аптека)
            {
                if ((rbA.Checked) && (_organization.Pharmacy != "A"))
                        return true;
                if ((rbB.Checked) && (_organization.Pharmacy != "B"))
                    return true;
                if ((rbC.Checked) && (_organization.Pharmacy != "C"))
                    return true;
            }
            
            return false;
        }

        private void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
                return;

            e.Handled = !char.IsDigit(e.KeyChar);
        }
    }
}
