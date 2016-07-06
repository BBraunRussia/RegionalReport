using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace ClassLibrary.SF
{
    public class PersonList : InitProvider, IEnumerable<Person>
    {
        private static PersonList _uniqueInstance;
        private Dictionary<string, Person> list;
        private string tableName;

        private PersonList(string tableName)
        {
            this.tableName = tableName;

            list = new Dictionary<string, Person>();

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
            return CreateTable(list.OrderBy(p => p.Value.Name));
        }

        public DataTable ToDataTable(Organization organization)
        {
            return CreateTable(list);
        }

        public List<Person> GetItems(Organization organization)
        {
            return (from item in list
                    where ((item.Value.Organization.ID == organization.ID) || ((item.Value.Organization.ParentOrganization != null) && item.Value.Organization.ParentOrganization.ID == organization.ID))
                    orderby item.Value.Name
                    select item.Value).ToList();
        }

        public DataTable ToDataTable(User user)
        {
            UserRightList userRightList = UserRightList.GetUniqueInstance();
            var userRightList2 = userRightList.ToList(user);

            var listPerson = (from item in list
                              where userRightList2.Contains(((item.Value.Organization.ParentOrganization == null) ? (item.Value.Organization as IHaveRegion) : item.Value.Organization.ParentOrganization as IHaveRegion).RealRegion.RegionRR)
                              orderby item.Value.Name
                              select item);

            return CreateTable(listPerson);
        }

        private DataTable CreateTable(IEnumerable<KeyValuePair<string, Person>> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RR ID", typeof(int));
            dt.Columns.Add("SF ID");
            dt.Columns.Add("Фамилия");
            dt.Columns.Add("Имя");
            dt.Columns.Add("Отчество");
            dt.Columns.Add("Организация");
            dt.Columns.Add("Подразделение");
            dt.Columns.Add("Должность");
            dt.Columns.Add("Регион России");
            dt.Columns.Add("Город");

            foreach (var item in list)
                dt.Rows.Add(item.Value.GetRow());

            return dt;
        }

        public void Add(Person person)
        {
            if (!IsExist(person.NumberSF))
            {
                list.Add(person.NumberSF, person);
            }
        }

        public bool CheckNamesake(Person person)
        {
            return list.Values.ToList().Exists(p => p.LastName == person.LastName && p.FirstName == person.FirstName && p.SecondName == person.SecondName && p.Organization == person.Organization);
        }

        public string Delete(Person person)
        {
            list.Remove(person.NumberSF);

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
            return (IsExist(numberSF)) ? list[numberSF] : null;
        }

        public Person GetItem(int id)
        {
            var listNew = list.Where(p => p.Value.ID == id);

            return listNew.Count() == 0 ? null : listNew.First().Value;
        }

        private bool IsExist(string numberSF)
        {
            return list.ContainsKey(numberSF);
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
