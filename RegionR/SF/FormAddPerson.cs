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
    public partial class FormAddPerson : Form
    {
        private Person _person;

        private AcademTitleList _academTitleList;
        private MainSpecPersonList _mainSpecPersonList;
        private PositionList _positionList;

        private bool _changeSubOrg;

        public FormAddPerson(Person person)
        {
            InitializeComponent();

            _person = person;

            _academTitleList = AcademTitleList.GetUniqueInstance();
            _mainSpecPersonList = MainSpecPersonList.GetUniqueInstance();
            _positionList = PositionList.GetUniqueInstance();

            lbSubOrganization.Visible = !(_person.Organization is OtherOrganization);
            tbSubOrganization.Visible = !(_person.Organization is OtherOrganization);

            _changeSubOrg = false;
        }
        
        private void FormAddPerson_Load(object sender, EventArgs e)
        {
            LoadDictionary();

            LoadData();
        }

        private void LoadData()
        {
            tbLastName.Text = _person.LastName;
            tbFirstName.Text = _person.FirstName;
            tbSecondName.Text = _person.SecondName;
            
            cbAppeal.SelectedIndex = _person.Appeal;

            if (_person.Position != null)
                cbPosition.SelectedValue = _person.Position.ID;

            if (_person.MainSpecPerson != null)
                cbMainSpecPerson.SelectedValue = _person.MainSpecPerson.ID;

            if (_person.AcademTitle != null)
                cbAcademTitle.SelectedValue = _person.AcademTitle.ID;

            tbEmail.Text = _person.Email;
            mtbMobile.Text = _person.Mobile;
            tbPhone.Text = _person.Phone;

            lbNumberSF.Text = _person.NumberSF;

            tbOrganization.Text = (_person.Organization.ParentOrganization == null) ? _person.Organization.ShortName : _person.Organization.ParentOrganization.ShortName;
            tbSubOrganization.Text = (_person.Organization.ParentOrganization == null) ? "Администрация" : _person.Organization.ShortName;
            tbComment.Text = _person.Comment;

            if ((_person.ID == 0) && (_person.Organization.TypeOrg == TypeOrg.Аптека))
            {
                cbMainSpecPerson.SelectedValue = 86;
                cbPosition.SelectedValue = 7;
            }
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
                CopyFields();

                _person.Save();

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
            ClassForForm.CheckFilled(tbLastName.Text, "Фамилия");
            ClassForForm.CheckFilled(tbFirstName.Text, "Имя");
            ClassForForm.CheckFilled(tbSecondName.Text, "Отчество");
            
            _person.LastName = tbLastName.Text;
            _person.FirstName = tbFirstName.Text;
            _person.SecondName = tbSecondName.Text;

            _person.Appeal = cbAppeal.SelectedIndex;
            _person.Position = _positionList.GetItem(Convert.ToInt32(cbPosition.SelectedValue)) as Position;
            _person.MainSpecPerson = _mainSpecPersonList.GetItem(Convert.ToInt32(cbMainSpecPerson.SelectedValue)) as MainSpecPerson;
            _person.AcademTitle = _academTitleList.GetItem(Convert.ToInt32(cbAcademTitle.SelectedValue)) as AcademTitle;

            _person.Email = tbEmail.Text;
            _person.Mobile = mtbMobile.Text;
            _person.Phone = tbPhone.Text;

            _person.Comment = tbComment.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (SaveIfNeed())
            {
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

            if (_person.Mobile != mtbMobile.Text)
                return true;

            if (_person.Phone != tbPhone.Text)
                return true;

            if (_person.Comment != tbComment.Text)
                return true;

            return false;
        }

        private void btnChangeOrganizationAndSubOrganization_Click(object sender, EventArgs e)
        {
            SaveIfNeed();

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
                    TrySave();
                    return true;
                }
            }

            return false;
        }
    }
}
