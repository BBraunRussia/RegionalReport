using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using ClassLibrary.SF.Entities;

namespace ClassLibrary.SF.Lists
{
    public class PersonList : InitProvider, IEnumerable<Person>
    {
        private static PersonList _uniqueInstance;
        private Dictionary<int, Person> list;
        private string tableName;

        private PersonList(string tableName)
        {
            this.tableName = tableName;

            list = new Dictionary<int, Person>();

            LoadFromDataBase();
        }

        public void Reload()
        {
            list.Clear();

            LoadFromDataBase();
        }

        private void LoadFromDataBase()
        {
            DataTable dt = _provider.Select(tableName);

            foreach (DataRow row in dt.Rows)
            {
                Person person = Factory.CreateItem(tableName, row) as Person;
                if (person.CanAdd)
                {
                    Add(person);
                }
            }
        }

        public static PersonList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new PersonList("SF_Person");

            return _uniqueInstance;
        }

        public DataTable ToDataTable()
        {
            return CreateTable(list.OrderBy(p => p.Value.Name).Select(p => p.Value));
        }

        public DataTable ToDataTable(Organization organization)
        {
            return CreateTable(GetItems(organization));
        }

        public IEnumerable<Person> GetItems(Organization organization)
        {
            return from item in list
                   where item.Value.Organization != null 
                        && ((item.Value.Organization.ID == organization.ID) || ((item.Value.Organization.ParentOrganization != null) && item.Value.Organization.ParentOrganization.ID == organization.ID))
                   orderby item.Value.Name
                   select item.Value;
        }

        public DataTable ToDataTable(User user)
        {
            UserRightList userRightList = UserRightList.GetUniqueInstance();
            var userRightList2 = userRightList.ToList(user);

            var listPerson = from item in list
                             where userRightList2.Contains(((item.Value.Organization.ParentOrganization == null) ? item.Value.Organization : item.Value.Organization.ParentOrganization).RealRegion.RegionRR)
                             orderby item.Value.Name
                             select item.Value;

            return CreateTable(listPerson);
        }

        private DataTable CreateTable(IEnumerable<Person> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RR ID", typeof(int));
            dt.Columns.Add("CRM ID");
            dt.Columns.Add("Фамилия");
            dt.Columns.Add("Имя");
            dt.Columns.Add("Отчество");
            dt.Columns.Add("Организация");
            dt.Columns.Add("Подразделение");
            dt.Columns.Add("Должность");
            dt.Columns.Add("Регион России");
            dt.Columns.Add("Город");

            foreach (var item in list)
                dt.Rows.Add(item.GetRow());

            return dt;
        }

        public void Add(Person person)
        {
            if (!IsExist(person.ID))
            {
                list.Add(person.ID, person);
            }
        }

        public bool CheckNamesake(Person person)
        {
            return list.Values.ToList().Exists(p => p.LastName == person.LastName && p.FirstName == person.FirstName && p.SecondName == person.SecondName && p.Organization == person.Organization);
        }

        public string Delete(Person person)
        {
            list.Remove(person.ID);

            return _provider.Delete(tableName, person.ID);
        }

        public void Delete(Organization organization)
        {
            var personList = list.Where(item => item.Value.Organization.ID == organization.ID).ToList();
            personList.ForEach(item => item.Value.Delete());
        }

        public bool IsOrganizationHaveUnique(Person person)
        {
            return list.Values.ToList().Exists(item => (item as Person).Organization == person.Organization && (item as Person).Position == person.Position);
        }

        public Person GetItem(string numberSF)
        {
            var listPerson = list.Where(p => p.Value.NumberSF == numberSF).Select(p => p.Value);
            return (listPerson.Count() > 0) ? listPerson.First() : null;
        }

        public Person GetItem(int id)
        {
            var listNew = list.Where(p => p.Value.ID == id);

            return listNew.Count() == 0 ? null : listNew.First().Value;
        }

        private bool IsExist(int id)
        {
            return list.ContainsKey(id);
        }

        public IEnumerator<Person> GetEnumerator()
        {
            foreach (var p in list)
            {
                yield return p.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
