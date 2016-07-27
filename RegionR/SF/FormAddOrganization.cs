using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Models;
using ClassLibrary.SF.Common;

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
            SetEnabledComponent();

            LoadDictionaries();

            LoadData();
        }

        private void LoadData()
        {
            this.Text = string.Concat("Карточка ", _organization.TypeOrg, " ЛПУ");
            lbTypeOrgName.Text = string.Concat(_organization.TypeOrg.ToString(), ":");

            lbID.Text = _organization.ID.ToString();

            lbLPU.Text = _parentLPU.ShortName.ToUpper();

            tbName.Text = _organization.Name;
            tbShortName.Text = _organization.ShortName;

            if (_organization.MainSpec != null)
                cbMainSpec.SelectedValue = _organization.MainSpec.ID;

            if (_organization.TypeOrg == TypeOrg.Аптека)
            {
                cbMainSpec.SelectedValue = 40;
                cbMainSpec.Enabled = false;
                lbBranch.Location = new Point(85, 32);
            }
            else if (_organization.TypeOrg == TypeOrg.Отделение)
            {
                btnShowRules.Visible = true;

                lbBranch.Location = new Point(117, 32);
             
                gbAvitum.Visible = true;
                tbMachineGD.Text = _organization.MachineGD;
                tbMachineGDF.Text = _organization.MachineGDF;
                tbMachineCRRT.Text = _organization.MachineCRRT;
                tbShift.Text = _organization.Shift;
                tbPatientGD.Text = _organization.PatientGD;
                tbPatientPD.Text = _organization.PatientPD;
                tbPatientCRRT.Text = _organization.PatientCRRT;
            }

            else if (_organization.TypeOrg == TypeOrg.Отдел)
            {
                lbBranch.Location = new Point(81, 32);
                cbMainSpec.Visible = false;
                lbMainSpec.Visible = false;
            }
            else
            {
                tbMachineGD.Text = string.Empty;
                tbMachineGDF.Text = string.Empty;
                tbMachineCRRT.Text = string.Empty;
                tbShift.Text = string.Empty;
                tbPatientGD.Text = string.Empty;
                tbPatientPD.Text = string.Empty;
                tbPatientCRRT.Text = string.Empty;
            }

            tbEmail.Text = _organization.Email;
            tbWebSite.Text = _organization.Website;
            tbPhone.Text = _organization.Phone;

            tbPhoneCode.Text = (_organization.ParentOrganization as LPU).City.PhoneCode;

            SetPhoneCodeMask();

            lbCrmID.Text = _organization.CrmID;
            lbTypeOrg.Text = _organization.TypeOrg.ToString();

            ShowHistory();
        }

        private void ShowHistory()
        {
            lbAutor.Text = _historyList.GetItemString(_organization, HistoryAction.Создал);
            lbEditor.Text = _historyList.GetItemString(_organization, HistoryAction.Редактировал);
        }

        private void SetPhoneCodeMask()
        {
            tbPhone.MaxLength = Phone.GetPhoneLenght(tbPhoneCode.Text);
        }

        private void SetEnabledComponent()
        {
            ControlEditMode controlEditMode = new ControlEditMode(this.Controls, btnCancel);
            controlEditMode.SetEnableValue(btnShowRules, true);
            controlEditMode.SetEnableValue(btnShowEmployees, true);
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
            }
            catch (NotImplementedException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка создания", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
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

            CheckNameAdmin(tbName.Text);
            CheckNameAdmin(tbShortName.Text);

            _organization.Name = tbName.Text;
            _organization.ShortName = tbShortName.Text;
            _organization.Email = tbEmail.Text;
            _organization.Website = tbWebSite.Text;
            _organization.Phone = tbPhone.Text;

            if (_organization.TypeOrg == TypeOrg.Отделение)
            {
                _organization.MachineGD = tbMachineGD.Text;
                _organization.MachineGDF = tbMachineGDF.Text;
                _organization.MachineCRRT = tbMachineCRRT.Text;
                _organization.Shift = tbShift.Text;
                _organization.PatientGD = tbPatientGD.Text;
                _organization.PatientPD = tbPatientPD.Text;
                _organization.PatientCRRT = tbPatientCRRT.Text;
            }
            else
            {
                _organization.MachineGD = string.Empty;
                _organization.MachineGDF = string.Empty;
                _organization.MachineCRRT = string.Empty;
                _organization.Shift = string.Empty;
                _organization.PatientGD = string.Empty;
                _organization.PatientPD = string.Empty;
                _organization.PatientCRRT = string.Empty;
            }
        }

        private void CheckNameAdmin(string text)
        {
            if (text.ToLower().Replace(" ", "") == "администрация")
                throw new NotImplementedException("Запрещено создавать подразделение с названием \"Администрация\", если нужно добавить персону в \"Администрацию\" добавляйте напрямую в ЛПУ/филиал.");
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
            if (e.KeyChar == (char)Keys.Back)
                return;

            e.Handled = !char.IsDigit(e.KeyChar);
        }

        private void btnShowRules_Click(object sender, EventArgs e)
        {
            if (!MyFile.Open(Files.rules_department))
                MessageBox.Show("Файл не найден", "Файл отсутствует", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
