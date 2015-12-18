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
    public partial class FormAddOrganization : Form
    {
        private MainSpecList _mainSpecList;

        private Organization _organization;
        private LPU _parentLPU;

        public FormAddOrganization(Organization organization)
        {
            InitializeComponent();

            _organization = organization;
            _parentLPU = (_organization.ParentOrganization as LPU);

            _mainSpecList = MainSpecList.GetUniqueInstance();
        }

        private void FormAddOrganization_Load(object sender, EventArgs e)
        {
            LoadDictionaries();

            lbBranch.Text = _organization.TypeOrg.ToString();
            lbName.Text = _parentLPU.ShortName.ToUpper();

            tbName.Text = _organization.Name;
            tbShortName.Text = _organization.ShortName;

            if (_organization.MainSpec != null)
                cbMainSpec.SelectedValue = _organization.MainSpec.ID;

            tbEmail.Text = _organization.Email;
            tbWebSite.Text = _organization.WebSite;
            tbPhone.Text = _organization.Phone;

            lbNumberSF.Text = _organization.NumberSF;
            lbTypeOrg.Text = _organization.TypeOrg.ToString();
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

        private void TrySave()
        {
            try
            {
                CopyFields();

                _organization.Save();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "Обязательное поле", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyFields()
        {
            int idMainSpec = Convert.ToInt32(cbMainSpec.SelectedValue);
            _organization.MainSpec = _mainSpecList.GetItem(idMainSpec) as MainSpec;

            ClassForForm.CheckFilled(tbName.Text, "Официальное название");
            ClassForForm.CheckFilled(tbShortName.Text, "Сокращенное название");

            _organization.Name = tbName.Text;
            _organization.ShortName = tbShortName.Text;
            _organization.Email = tbEmail.Text;
            _organization.WebSite = tbWebSite.Text;
            _organization.Phone = tbPhone.Text;
        }
    }
}
