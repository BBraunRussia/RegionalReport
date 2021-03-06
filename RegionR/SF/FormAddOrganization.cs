﻿using System;
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
    public partial class FormAddOrganization : Form
    {
        private MainSpecList _mainSpecList;
        private HistoryList _historyList;

        private Organization _organization;
        private LPU _parentLPU;

        public FormAddOrganization(Organization organization)
        {
            InitializeComponent();

            _organization = organization;
            _parentLPU = (_organization.ParentOrganization as LPU);

            _mainSpecList = MainSpecList.GetUniqueInstance();
            _historyList = HistoryList.GetUniqueInstance();
        }

        private void FormAddOrganization_Load(object sender, EventArgs e)
        {
            LoadDictionaries();

            LoadData();
        }

        private void LoadData()
        {
            this.Text = string.Concat("Карточка ", _organization.TypeOrg, " ЛПУ");
            lbTypeOrgName.Text = string.Concat(_organization.TypeOrg.ToString(), ":");
            
            lbLPU.Text = _parentLPU.ShortName.ToUpper();

            tbName.Text = _organization.Name;
            tbShortName.Text = _organization.ShortName;

            if (_organization.MainSpec != null)
                cbMainSpec.SelectedValue = _organization.MainSpec.ID;

            if (_organization.TypeOrg == TypeOrg.Аптека)
            {
                cbMainSpec.SelectedValue = 40;
                cbMainSpec.Enabled = false;
                lbMainSpec.Visible = false;
                lbBranch.Location = new Point(85, 32);
            }
            else if (_organization.TypeOrg == TypeOrg.Отделение)
            {
                btnShowRules.Visible = true;

                lbBranch.Location = new Point(117, 32);
            }
            else if (_organization.TypeOrg == TypeOrg.Отдел)
            {
                lbBranch.Location = new Point(81, 32);
                cbMainSpec.Visible = false;
                lbMainSpec.Visible = false;
            }
            
            tbEmail.Text = _organization.Email;
            tbWebSite.Text = _organization.WebSite;
            tbPhone.Text = _organization.Phone;

            lbNumberSF.Text = _organization.NumberSF;
            lbTypeOrg.Text = _organization.TypeOrg.ToString();

            ShowHistory();
        }

        private void ShowHistory()
        {
            lbAutor.Text = _historyList.GetItemString(_organization, ClassLibrary.SF.Action.Создал);
            lbEditor.Text = _historyList.GetItemString(_organization, ClassLibrary.SF.Action.Редактировал);
        }

        private void LoadDictionaries()
        {
            ClassForForm.LoadDictionary(cbMainSpec, _mainSpecList.ToDataTable());
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            TrySave();

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

                History.Save(_organization, UserLogged.Get());

                ShowHistory();

                return true;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Обязательное поле", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        private void CopyFields()
        {
            if (cbMainSpec.Visible)
            {
                int idMainSpec = Convert.ToInt32(cbMainSpec.SelectedValue);
                _organization.MainSpec = _mainSpecList.GetItem(idMainSpec) as MainSpec;
            }

            ClassForForm.CheckFilled(tbName.Text, "Официальное название");
            ClassForForm.CheckFilled(tbShortName.Text, "Сокращенное название");

            _organization.Name = tbName.Text;
            _organization.ShortName = tbShortName.Text;
            _organization.Email = tbEmail.Text;
            _organization.WebSite = tbWebSite.Text;
            _organization.Phone = tbPhone.Text;
        }

        private void tbShortName_TextChanged(object sender, EventArgs e)
        {
            lbBranch.Text = tbShortName.Text;
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
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

        private void btnShowEmployees_Click(object sender, EventArgs e)
        {
            FormPersonList formPersonList = new FormPersonList(_organization);
            formPersonList.ShowDialog();
        }

        private void tbPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!char.IsDigit(e.KeyChar));
        }

        private void btnShowRules_Click(object sender, EventArgs e)
        {
            if (!MyFile.Open(Files.rules_department))
                MessageBox.Show("Файл не найден", "Файл отсутствует", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
