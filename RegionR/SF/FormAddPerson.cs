using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF;
using ClassLibrary;

namespace RegionR.SF
{
    public partial class FormAddPerson : Form
    {
        private Person _person;

        private AcademTitleList _academTitleList;
        private MainSpecPersonList _mainSpecPersonList;
        private PositionList _positionList;
        private HistoryList _historyList;

        private bool _changeSubOrg;

        public FormAddPerson(Person person)
        {
            InitializeComponent();

            _person = person;

            _academTitleList = AcademTitleList.GetUniqueInstance();
            _mainSpecPersonList = MainSpecPersonList.GetUniqueInstance();
            _positionList = PositionList.GetUniqueInstance();
            _historyList = HistoryList.GetUniqueInstance();

            lbSubOrganization.Visible = !(_person.Organization is OtherOrganization);
            tbSubOrganization.Visible = !(_person.Organization is OtherOrganization);

            _changeSubOrg = false;
        }
        
        private void FormAddPerson_Load(object sender, EventArgs e)
        {
            LoadDictionary();

            LoadData();

            SetEnabledComponent();
        }

        private void LoadData()
        {
            lbPersonID.Text = _person.ID.ToString();
            SetRealRegionAndCity();

            tbLastName.Text = _person.LastName;
            tbFirstName.Text = _person.FirstName;
            tbSecondName.Text = _person.SecondName;

            dtpDateBirth.Value = (_person.DateBirth.HasValue) ? _person.DateBirth.Value : DateTimePicker.MinimumDateTime;
            
            cbAppeal.SelectedIndex = _person.Appeal;

            if (_person.Position != null)
                cbPosition.SelectedValue = _person.Position.ID;

            if (_person.MainSpecPerson != null)
                cbMainSpecPerson.SelectedValue = _person.MainSpecPerson.ID;

            if (_person.AcademTitle != null)
                cbAcademTitle.SelectedValue = _person.AcademTitle.ID;

            tbEmail.Text = _person.Email;
            mtbMobile.Text = _person.Mobile;
            mtbPhone.Text = _person.Phone;

            if (!string.IsNullOrEmpty(_person.NumberSF))
            {
                lbNumberSF.Text = _person.NumberSF;
            }

            tbOrganization.Text = _person.GetOrganizationName();
            tbSubOrganization.Text = _person.GetSubOrganizationName();
            tbComment.Text = _person.Comment;

            if ((_person.ID == 0) && (_person.Organization.TypeOrg == TypeOrg.Аптека))
            {
                cbMainSpecPerson.SelectedValue = 86;
                cbPosition.SelectedValue = 7;
            }

            SetPhoneCode();
            SetPhoneMask();

            ShowHistory();
        }

        private void ShowHistory()
        {
            lbAutor.Text = _historyList.GetItemString(_person, HistoryAction.Создал);
            lbEditor.Text = _historyList.GetItemString(_person, HistoryAction.Редактировал);
        }

        private void SetEnabledComponent()
        {
            ControlEditMode controlEditMode = new ControlEditMode(this.Controls, btnClose);
            controlEditMode.SetEnableValue(tbOrganization, false);
            controlEditMode.SetEnableValue(tbSubOrganization, false);
        }

        private void SetPhoneCode()
        {
            IHaveRegion organization = ((_person.Organization is IHaveRegion) ? _person.Organization : _person.Organization.ParentOrganization) as IHaveRegion;
            tbPhoneCode.Text = organization.City.PhoneCode;
        }

        private void SetPhoneMask()
        {
            int lenght = (tbPhoneCode.Text == string.Empty) ? 10 : 10 - tbPhoneCode.Text.Length;

            string mask = string.Empty;

            for (int i = 0; i < lenght; i++)
                mask += "0";

            mtbPhone.Mask = mask;
        }

        private void SetRealRegionAndCity()
        {
            IHaveRegion region = (_person.Organization is IHaveRegion) ? _person.Organization as IHaveRegion : _person.Organization.ParentOrganization as IHaveRegion;

            lbCity.Text = region.City.Name;
            lbRealRegion.Text = region.RealRegion.Name;
        }

        private void LoadDictionary()
        {
            ClassForForm.LoadDictionary(cbAcademTitle, _academTitleList.ToDataTable());
            ClassForForm.LoadDictionary(cbMainSpecPerson, _mainSpecPersonList.ToDataTable());
            ClassForForm.LoadDictionary(cbPosition, _positionList.ToDataTable());
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            TrySave();
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            if (TrySave())
                DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private bool TrySave()
        {
            try
            {
                if (!IsHaveChanges())
                    return true;

                if (!CopyFields())
                    return false;
                                                
                _person.Save();

                History.Save(_person, UserLogged.Get());

                ShowHistory();

                return true;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Обязательное поле", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return false;
            }
        }
        
        private bool CopyFields()
        {
            ClassForForm.CheckFilled(tbLastName.Text, "Фамилия");
            ClassForForm.CheckFilled(tbFirstName.Text, "Имя");
            ClassForForm.CheckFilled(tbSecondName.Text, "Отчество");

            string lastName, firstName, secondName;

            lastName = _person.LastName;
            firstName = _person.FirstName;
            secondName = _person.SecondName;

            _person.LastName = tbLastName.Text;
            _person.FirstName = tbFirstName.Text;
            _person.SecondName = tbSecondName.Text;

            if (_person.CheckNamesake())
            {
                if (MessageBox.Show("В данной организации уже есть сотрудник с такими ФИО. Продолжить сохранение?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                {
                    _person.LastName = lastName;
                    _person.FirstName = firstName;
                    _person.SecondName = secondName;
                    return false;
                }
            }
            
            _person.Appeal = cbAppeal.SelectedIndex;

            Position position = _person.Position;

            _person.Position = _positionList.GetItem(Convert.ToInt32(cbPosition.SelectedValue)) as Position;

            if (_person.IsOrganizationHaveUnique())
            {
                MessageBox.Show("В данной организации уже есть сотрудник с такой должностью.\nИсправьте пожалуйста.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _person.Position = position;
                return false;
            }

            _person.MainSpecPerson = _mainSpecPersonList.GetItem(Convert.ToInt32(cbMainSpecPerson.SelectedValue)) as MainSpecPerson;
            _person.AcademTitle = _academTitleList.GetItem(Convert.ToInt32(cbAcademTitle.SelectedValue)) as AcademTitle;

            _person.Email = tbEmail.Text;
            _person.Mobile = mtbMobile.Text;
            _person.Phone = mtbPhone.Text;

            _person.Comment = tbComment.Text;

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (SaveIfNeed())
            {
                if (TrySave())
                    DialogResult = System.Windows.Forms.DialogResult.OK;

                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private bool IsHaveChanges()
        {
            if (_changeSubOrg)
                return true;

            if (_person.LastName != tbLastName.Text)
                return true;

            if (_person.FirstName != tbFirstName.Text)
                return true;

            if (_person.SecondName != tbSecondName.Text)
                return true;

            if (_person.Appeal != cbAppeal.SelectedIndex)
                return true;
                        
            if (_person.Position != null)
            {
                Position position = _positionList.GetItem(Convert.ToInt32(cbPosition.SelectedValue)) as Position;
                if (_person.Position != position)
                    return true;
            }

            if (_person.MainSpecPerson != null)
            {
                MainSpecPerson mainSpecPerson = _mainSpecPersonList.GetItem(Convert.ToInt32(cbMainSpecPerson.SelectedValue)) as MainSpecPerson;
                if (_person.MainSpecPerson != mainSpecPerson)
                    return true;
            }

            if (_person.AcademTitle != null)
            {
                AcademTitle academTitle = _academTitleList.GetItem(Convert.ToInt32(cbAcademTitle.SelectedValue)) as AcademTitle;
                if (_person.AcademTitle != academTitle)
                    return true;
            }
            
            if (_person.Email != tbEmail.Text)
                return true;

            if ((mtbMobile.Text != "+7(   )   -  -") && (_person.Mobile != mtbMobile.Text))
                return true;

            if (_person.Phone != mtbPhone.Text)
                return true;

            if (_person.Comment != tbComment.Text)
                return true;

            return false;
        }

        private void btnChangeOrganizationAndSubOrganization_Click(object sender, EventArgs e)
        {
            if (!TrySave())
                return;
            
            FormFirstStepAddPerson formFirstStepAddPerson = new FormFirstStepAddPerson(_person);
            if (formFirstStepAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FormSecondStepAddPerson formSecondStepAddPerson = new FormSecondStepAddPerson(_person, true);
                if (formSecondStepAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _changeSubOrg = true;
                    LoadData();
                }
            }
        }

        private bool SaveIfNeed()
        {
            if (IsHaveChanges())
            {
                if (MessageBox.Show("На форме имеются не сохранёные изменения, сохранить перед продолжение?", "Сохраненить изменения", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    return true;
                }
            }

            return false;
        }

        private void btnChangeSubOrganization_Click(object sender, EventArgs e)
        {
            if (!TrySave())
                return;

            FormSecondStepAddPerson formSecondStepAddPerson = new FormSecondStepAddPerson(_person, true);
            if (formSecondStepAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _changeSubOrg = true;
                LoadData();
            }
        }
    }
}
