using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class PersonList : BaseList
    {
        private static PersonList _uniqueInstance;

        private PersonList(string tableName)
            : base(tableName)
        { }

        public static PersonList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new PersonList("SF_Person");

            return _uniqueInstance;
        }

        public override DataTable ToDataTable()
        {
            var list = from item in List
                       orderby item.Name
                       select (item as Person);

            return CreateTable(list.ToList());
        }

        public DataTable ToDataTable(Organization organization)
        {
            var list = GetItems(organization);

            return CreateTable(list);
        }

        public List<Person> GetItems(Organization organization)
        {
            return (from item in List
                    where (((item as Person).Organization.ID == organization.ID) || (((item as Person).Organization.ParentOrganization != null) && (item as Person).Organization.ParentOrganization.ID == organization.ID))
                    orderby item.Name
                    select (item as Person)).ToList();
        }

        public DataTable ToDataTable(User user)
        {
            UserRightList userRightList = UserRightList.GetUniqueInstance();
            var userRightList2 = userRightList.ToList(user);

            var list = (from item in List
                        where userRightList2.Contains((((item as Person).Organization.ParentOrganization == null) ? ((item as Person).Organization as IHaveRegion) : (item as Person).Organization.ParentOrganization as IHaveRegion).RealRegion.RegionRR)
                        orderby item.Name
                        select (item as Person)).ToList();

            return CreateTable(list);
        }

        private DataTable CreateTable(List<Person> list)
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
                dt.Rows.Add(item.GetRow());

            return dt;
        }

        public void Add(Person person)
        {
            if (!List.Exists(item => item == person))
                List.Add(person);
        }

        public bool CheckNamesake(Person person)
        {
            return List.Exists(item => (item as Person).LastName == person.LastName && (item as Person).FirstName == person.FirstName && (item as Person).SecondName == person.SecondName && (item as Person).Organization == person.Organization);
        }

        public void Delete(Organization organization)
        {
            var personList = List.Where(item => (item as Person).Organization.ID == organization.ID).ToList();
            personList.ForEach(item => (item as Person).Delete());
        }

        public bool IsOrganizationHaveUnique(Person person)
        {
            return List.Exists(item => (item as Person).Organization == person.Organization && (item as Person).Position == person.Position);
        }

        public List<Person> GetList()
        {
            return List.Select(item => item as Person).ToList();
        }

        public new Person GetItem(string numberSF)
        {
            var list = List.Where(item => (item as Person).NumberSF == numberSF).Select(item => item as Person).ToList();

            return (list.Count > 0) ? list.First() : null;
        }
    }
}
