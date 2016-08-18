﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using ClassLibrary;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Entities;
using RegionReport.Domain;
using RegionR.Controllers;

namespace RegionR.SF
{
    public class OrganizationListController : BaseOperations, IController
    {
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

            DataTable dt = (_user.RoleSF == RolesSF.Пользователь) ? _lpuList.ToDataTable(_user) : _lpuList.ToDataTable();

            _dgv.DataSource = dt;

            _dgv.Columns[0].Width = 80;
            _dgv.Columns[1].Width = 122;
            _dgv.Columns[2].Width = 300;
            _dgv.Columns[3].Width = 80;
            _dgv.Columns[4].Width = 80;
            _dgv.Columns[5].Width = 120;

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
                else
                {
                    FormAddOtherOrganization formAddOtherOrganization = new FormAddOtherOrganization(organization);
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
            else
            {
                FormAddOtherOrganization formAddOtherOrganization = new FormAddOtherOrganization(organization as Organization);
                return (formAddOtherOrganization.ShowDialog() == System.Windows.Forms.DialogResult.OK);
            }
        }

        public bool DeleteOrganization()
        {
            Organization organization = GetOrganization();
            
            if ((organization != null) && (ClassForForm.DeleteOrganization(organization)))
            {
                History.Save(organization, UserLogged.Get(), HistoryAction.Удалил);
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
            if (_dgv.CurrentCell == null)
                return null;

            int id = Convert.ToInt32(_dgv.Rows[_dgv.CurrentCell.RowIndex].Cells[0].Value);

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            return organizationList.GetItem(id);
        }


        public void SetStyle()
        {
            throw new NotImplementedException();
        }
    }
}