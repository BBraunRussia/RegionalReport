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

        public FormAddPerson(Person person)
        {
            InitializeComponent();

            _person = person;
        }
        
        private void FormAddPerson_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            tbLastName.Text = _person.LastName;
            tbFirstName.Text = _person.FirstName;
            tbSecondName.Text = _person.SecondName;
            
            tbAppeal.Text = _person.Appeal;
            tbPosition.Text = _person.Position;
            tbSpecialization.Text = _person.Specialization;

            tbEmail.Text = _person.Email;
            tbMobile.Text = _person.Mobile;
            tbPhone.Text = _person.Phone;

            lbNumberSF.Text = _person.NumberSF;

            tbOrganization.Text = (_person.Organization.ParentOrganization == null) ? _person.Organization.ShortName : _person.Organization.ParentOrganization.ShortName;
            tbSubOrganization.Text = (_person.Organization.ParentOrganization == null) ? "Администрация" : _person.Organization.ShortName;
            tbComment.Text = _person.Comment;
        }

        private void notImplement_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
