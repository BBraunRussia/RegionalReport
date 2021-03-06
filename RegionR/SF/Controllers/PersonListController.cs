﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF;
using System.Windows.Forms;

namespace RegionR.SF
{
    public class PersonListController
    {
        private PersonList _personList;
        private Organization _organization;
        private User _user;
        private DataGridView _dgv;
        private SearchInDgv _seacher;
        private FilteredDGV _filtredDGV;
        private SortDGV _sortDGV;

        public PersonListController(DataGridView dgv, Organization organization)
        {
            _organization = organization;
            _dgv = dgv;

            _seacher = new SearchInDgv(_dgv);
            _personList = PersonList.GetUniqueInstance();
            _user = UserLogged.Get();

            _filtredDGV = new FilteredDGV(dgv);
            _sortDGV = new SortDGV(dgv);
        }

        public DataGridView ToDataGridView()
        {
            ReLoad();

            DataTable dt = (_organization != null) ? _personList.ToDataTable(_organization) : (_user.RoleSF == RolesSF.Администратор) ? _personList.ToDataTable() : _personList.ToDataTable(_user);

            _dgv.DataSource = dt;

            _dgv.Columns[0].Visible = false;
            _dgv.Columns[4].Width = 300;
            _dgv.Columns[5].Width = 150;
            
            return _dgv;
        }

        public void ReLoad()
        {
            _personList.Reload();
        }

        public bool Add()
        {
            if (_organization == null)
            {
                return AddPerson();
            }
            else
            {
                Person person = new Person();
                person.Organization = _organization;
                return AddPerson(person);
            }
        }

        private bool AddPerson()
        {
            Person person = new Person();

            FormFirstStepAddPerson formFirstStepAddPerson = new FormFirstStepAddPerson(person);
            if (formFirstStepAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return AddPerson(person);
            }

            return false;
        }

        private bool AddPerson(Person person)
        {
            FormSecondStepAddPerson formSecondStepAddPerson = new FormSecondStepAddPerson(person);
            return (formSecondStepAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK);
        }

        public void Sort()
        {
            _sortDGV.Sort();
        }

        public void Search(string text)
        {
            _seacher.Find(text);
        }
        
        public bool DeletePerson()
        {
            if (MessageBox.Show("Вы действительно хотите удалить персону?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                Person person = GetPerson();
                person.Delete();
                History.Save(person, UserLogged.Get(), ClassLibrary.SF.Action.Удалил);
                return true;
            }

            return false;
        }

        public bool EditPerson()
        {
            Person person = GetPerson();

            FormAddPerson formAddPerson = new FormAddPerson(person);
            return (formAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK);
        }

        private Person GetPerson()
        {
            int id;
            int.TryParse(_dgv.Rows[_dgv.CurrentCell.RowIndex].Cells[0].Value.ToString(), out id);

            return _personList.GetItem(id) as Person;
        }

        public void CreateFilter()
        {
            _filtredDGV.Create();
        }

        public void DeleteFilter()
        {
            _filtredDGV.Delete();
        }

        public void ApplyFilter()
        {
            _filtredDGV.Apply();
        }
    }
}
