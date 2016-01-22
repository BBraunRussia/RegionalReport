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
    public partial class FormSecondStepAddPerson : Form
    {
        private Person _person;
        private bool _changeOnly;

        public FormSecondStepAddPerson(Person person, bool changeOnly = false)
        {
            InitializeComponent();

            _person = person;
            _changeOnly = changeOnly;

            if (_changeOnly)
            {
                btnAddPerson.Text = "Выбрать подразделение";
                lbSubOrganization.Text = "Выберите подразделение этой организации";
            }
        }

        private void FormSecondStepAddPerson_Load(object sender, EventArgs e)
        {
            lbName.Text = _person.Organization.ShortName;

            LoadData();
        }

        private void LoadData()
        {
            SubOrganizationList subOrganizationList = new SubOrganizationList(_person.Organization);

            DataTable dt = subOrganizationList.ToDataTable();
            dgv.DataSource = dt;
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].Width = 200;
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            _person = new Person();
            if (AddPerson())
                btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private Organization GetOrganization()
        {
            int id;
            int.TryParse(dgv.Rows[dgv.CurrentCell.RowIndex].Cells[0].Value.ToString(), out id);

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            return organizationList.GetItem(id);
        }

        private void dgv_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AddPerson();
        }

        private bool AddPerson()
        {
            _person.Organization = GetOrganization();

            if (_changeOnly)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                return false;
            }

            FormAddPerson formAddPerson = new FormAddPerson(_person);
            return (formAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK);
        }
    }
}
