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

        public FormSecondStepAddPerson(Person person)
        {
            InitializeComponent();

            _person = person;
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
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _person.Organization = GetOrganization();
        }

        private Organization GetOrganization()
        {
            int id;
            int.TryParse(dgv.Rows[dgv.CurrentCell.RowIndex].Cells[0].Value.ToString(), out id);

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            return organizationList.GetItem(id);
        }
    }
}
