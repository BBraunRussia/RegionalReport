﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Entities;
using ClassLibrary.SF.Common;
using RegionReport.Domain;

namespace RegionR.SF
{
    public partial class FormAddOtherOrganization : Form
    {
        private Organization _organization;
        private HistoryList _historyList;

        private RealRegionList _realRegionList;
        
        public FormAddOtherOrganization(Organization organization)
        {
            InitializeComponent();

            _organization = organization;

            _realRegionList = RealRegionList.GetUniqueInstance();
            _historyList = HistoryList.GetUniqueInstance();
        }

        private void FormAddOtherOrganization_Load(object sender, EventArgs e)
        {
            LoadDictionaries();

            LoadData();

            SetEnabledComponent();

            lbID.Text = _organization.ID.ToString();
        }

        private void LoadData()
        {
            string typeOrg = _organization.TypeOrg.ToString();

            if (_organization.TypeOrg == TypeOrg.Административное_Учреждение)
            {
                typeOrg = "Административное Учреждение";
            }
            else if (_organization.TypeOrg == TypeOrg.Ветеренарная_клиника)
            {
                typeOrg = "Ветеренарная клиника";
            }

            this.Text = string.Concat("Карточка Организации \"", typeOrg, "\"");
            lbOrganization.Text = typeOrg;
            lbOrganizationName.Text = _organization.ShortName;

            if (_organization.TypeOrg == TypeOrg.Аптека)
            {
                lbOrganizationName.Location = new Point(93, 9);

                btnShowRules.Visible = true;
                gbCategory.Visible = true;

                if (_organization.Pharmacy == "A")
                    rbA.Checked = true;
                else if (_organization.Pharmacy == "B")
                    rbB.Checked = true;
                else if (_organization.Pharmacy == "C")
                    rbC.Checked = true;
            }
            else if (_organization.TypeOrg == TypeOrg.Дистрибьютор)
            {
                lbOrganizationName.Location = new Point(157, 9);
            }
            else if ((_organization.TypeOrg == TypeOrg.Административное_Учреждение) || (_organization.TypeOrg == TypeOrg.Ветеренарная_клиника))
            {
                lbOrganizationName.Location = new Point(307, 9);
            }

            lbCrmID.Text = _organization.CrmID;
            lbTypeOrg.Text = _organization.TypeOrg.ToString();

            tbName.Text = _organization.Name;
            tbShortName.Text = _organization.ShortName;
            tbINN.Text = _organization.INN;
            tbKPP.Text = _organization.KPP;
            tbPostIndex.Text = _organization.PostIndex;
            tbStreet.Text = _organization.Street;
            tbEmail.Text = _organization.Email;
            tbWebSite.Text = _organization.Website;
            tbPhone.Text = _organization.Phone;
            tbCity.Text = _organization.City;

            if (_organization.RealRegion != null)
            {
                cbRealRegion.SelectedValue = _organization.RealRegion.ID;
            }
            
            ShowHistory();
        }

        private void ShowHistory()
        {
            lbAutor.Text = _historyList.GetItemString(_organization, HistoryAction.Создал);
            lbEditor.Text = _historyList.GetItemString(_organization, HistoryAction.Редактировал);
        }

        private void SetEnabledComponent()
        {
            ControlEditMode controlEditMode = new ControlEditMode(this.Controls, btnCancel);
            controlEditMode.SetEnable(gbCategory.Controls);
            controlEditMode.SetEnableValue(btnShowRules, true);
            controlEditMode.SetEnableValue(btnShowPerson, true);
        }

        private void LoadDictionaries()
        {
            if (UserLogged.Get().RoleSF == RolesSF.Пользователь)
                ClassForForm.LoadDictionary(cbRealRegion, _realRegionList.ToDataTable(UserLogged.Get()));
            else
                ClassForForm.LoadDictionary(cbRealRegion, _realRegionList.ToDataTable());
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
                if (!IsHaveChanges())
                    return true;

                if (!CopyFields())
                    return false;
                
                _organization.Save();

                History.Save(_organization, UserLogged.Get());

                ShowHistory();

                return true;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        private bool CopyFields()
        {
            ClassForForm.CheckFilled(tbName.Text, "Официальное название");
            ClassForForm.CheckFilled(tbShortName.Text, "Сокращенное название");
            ClassForForm.CheckFilled(tbStreet.Text, "Уличный адрес");

            if (tbINN.Text != string.Empty)
            {
                ClassForForm.CheckINN(_organization, tbINN.Text);
            }

            _organization.Name = tbName.Text;
            _organization.ShortName = tbShortName.Text;
            _organization.INN = tbINN.Text;
            
            _organization.KPP = tbKPP.Text;
            _organization.PostIndex = tbPostIndex.Text;
            _organization.Email = tbEmail.Text;
            _organization.Website = tbWebSite.Text;
            _organization.Phone = tbPhone.Text;
            _organization.City = tbCity.Text;
                        
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
            else
            {
                _organization.Pharmacy = string.Empty;
            }

            return true;
        }

        private void btnShowRules_Click(object sender, EventArgs e)
        {
            if (!MyFile.Open(Files.rules_pharmacy))
                MessageBox.Show("Файл не найден", "Файл отсутствует", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (_organization.Website != tbWebSite.Text)
                return true;
            if (_organization.Phone != tbPhone.Text)
                return true;
            if (_organization.City != tbCity.Text)
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

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            if (_organization.ID == 0)
            {
                if (!TrySave())
                    return;
            }

            Person person = new Person();
            person.Organization = _organization;
            
            FormAddPerson formAddPerson = new FormAddPerson(person);
            formAddPerson.ShowDialog();
        }

        private void btnShowPerson_Click(object sender, EventArgs e)
        {
            FormPersonList formPersonList = new FormPersonList(_organization);
            formPersonList.ShowDialog();
        }
    }
}
