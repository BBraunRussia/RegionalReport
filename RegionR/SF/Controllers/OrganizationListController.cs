using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF;
using System.Data;

namespace RegionR.SF
{
    public class OrganizationListController : BaseOperations, IController
    {
        private DataGridView _dgv;
        private LpuList _lpuList;
        private UserList _userList;
        private User _user;
        
        private TypeOrg _typeOrg;

        public OrganizationListController(DataGridView dgv)
            : base(dgv)
        {
            _dgv = dgv;
                        
            _userList = UserList.GetUniqueInstance();

            _lpuList = new LpuList();

            _user = _userList.GetItem(globalData.UserID) as User;

            _typeOrg = TypeOrg.ЛПУ;
        }

        public DataGridView ToDataGridView()
        {
            ReLoad();

            DataTable dt = (_user.RoleSF == RolesSF.Администратор) ? _lpuList.ToDataTable() : _lpuList.ToDataTable(_user);

            _dgv.DataSource = dt;

            _dgv.Columns[0].Width = 80;
            _dgv.Columns[1].Width = 300;
            _dgv.Columns[2].Width = 80;
            _dgv.Columns[3].Width = 80;
            _dgv.Columns[4].Width = 120;

            return _dgv;
        }

        public void ReLoad()
        {
            _lpuList.ReLoad();
        }
        
        public bool AddOrganization()
        {
            FormFirstStepAddOrganization formFirstStepAddOrganization = new FormFirstStepAddOrganization(this);
            if (formFirstStepAddOrganization.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Organization organization = Organization.CreateItem(_typeOrg);
                if (organization is LPU)
                {
                    FormSecondStepAddOrganization formSecondStepAddOrganization = new FormSecondStepAddOrganization(organization as LPU);
                    if (formSecondStepAddOrganization.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        FormAddLPU formAddLPU = new FormAddLPU(organization as LPU);
                        return (formAddLPU.ShowDialog() == System.Windows.Forms.DialogResult.OK);                            
                    }
                }
                else if (organization is OtherOrganization)
                {
                    FormAddOtherOrganization formAddOtherOrganization = new FormAddOtherOrganization(organization as OtherOrganization);
                    return (formAddOtherOrganization.ShowDialog() == System.Windows.Forms.DialogResult.OK);
                }
            }

            return false;
        }

        public void SetTypeOrg(TypeOrg typeOrg)
        {
            _typeOrg = typeOrg;
        }

        public bool EditOrganization()
        {
            Organization organization = GetOrganization();

            if (organization is LPU)
            {
                FormAddLPU FormAddLPU = new FormAddLPU(organization as LPU);
                return (FormAddLPU.ShowDialog() == System.Windows.Forms.DialogResult.OK);
            }
            else if (organization is OtherOrganization)
            {
                FormAddOtherOrganization formAddOtherOrganization = new FormAddOtherOrganization(organization as OtherOrganization);
                return (formAddOtherOrganization.ShowDialog() == System.Windows.Forms.DialogResult.OK);
            }

            return false;
        }

        public bool DeleteOrganization()
        {
            Organization organization = GetOrganization();

            if (ClassForForm.DeleteOrganization(organization))
            {
                History.Save(organization, UserLogged.Get(), ClassLibrary.SF.Action.Удалил);
                return true;
            }

            return false;
        }

        public void AddPerson()
        {
            Organization organization = GetOrganization();

            Person person = new Person();
            person.Organization = organization;

            bool open = true;

            if (organization is LPU)
            {
                FormSecondStepAddPerson formSecondStepAddPerson = new FormSecondStepAddPerson(person);
                open = (formSecondStepAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK);
            }

            if (open)
            {
                FormAddPerson formAddPerson = new FormAddPerson(person);
                formAddPerson.ShowDialog();
            }
        }
        
        private Organization GetOrganization()
        {
            int id;
            int.TryParse(_dgv.Rows[_dgv.CurrentCell.RowIndex].Cells[0].Value.ToString(), out id);

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            return organizationList.GetItem(id);
        }
    }
}