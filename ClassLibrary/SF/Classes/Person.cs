using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class Person : BaseDictionary, IHistory
    {
        private string _lastName;
        private string _numberSF;
        private string _firstName;
        private string _secondName;
        private int _idAppeal;
        private Position _position;
        private MainSpecPerson _mainSpecPerson;
        private AcademTitle _academTitle;
        private string _email;
        private string _mobile;
        private string _phone;
        private Organization _organization;
        private string _comment;
                
        public Person()
        {
            if (_numberSF == null)
                _numberSF = string.Empty;
        }

        public Person(DataRow row)
            : base(row)
        {
            _lastName = row[1].ToString();
            _numberSF = row[2].ToString();
            _firstName = row[3].ToString();
            _secondName = row[4].ToString();

            int.TryParse(row[5].ToString(), out _idAppeal);
            
            int idPosition;
            int.TryParse(row[6].ToString(), out idPosition);
            PositionList positionList = PositionList.GetUniqueInstance();
            _position = positionList.GetItem(idPosition) as Position;
            
            int idMainSpecPerson;
            int.TryParse(row[7].ToString(), out idMainSpecPerson);
            MainSpecPersonList mainSpecPersonList = MainSpecPersonList.GetUniqueInstance();
            _mainSpecPerson = mainSpecPersonList.GetItem(idMainSpecPerson) as MainSpecPerson;

            int idAcademTitle;
            int.TryParse(row[8].ToString(), out idAcademTitle);
            AcademTitleList academTitleList = AcademTitleList.GetUniqueInstance();
            _academTitle = academTitleList.GetItem(idAcademTitle) as AcademTitle;

            _email = row[9].ToString();
            _mobile = row[10].ToString();
            _phone = row[11].ToString();
            _comment = row[12].ToString();

            int idOrganization;
            int.TryParse(row[13].ToString(), out idOrganization);
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            _organization = organizationList.GetItem(idOrganization);

            if (_organization == null)
            {
                CanAdd = false;
            }
        }
        
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string SecondName
        {
            get { return _secondName; }
            set { _secondName = value; }
        }
        
        public int Appeal
        {
            get { return _idAppeal; }
            set { _idAppeal = value; }
        }
        
        public Organization Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }
        
        public Position Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public MainSpecPerson MainSpecPerson
        {
            get { return _mainSpecPerson; }
            set { _mainSpecPerson = value; }
        }

        public AcademTitle AcademTitle
        {
            get { return _academTitle; }
            set { _academTitle = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public string NumberSF { get { return _numberSF; } }
        
        public override object[] GetRow()
        {
            Organization organizationWithRegion = ((Organization is LPU) || (Organization is OtherOrganization)) ? Organization : Organization.ParentOrganization;

            string subOrganizationShortName = (Organization is LPU) ? "Администрация" : (Organization is OtherOrganization) ? string.Empty : Organization.ShortName;

            string mobileWithOutFormat = Mobile.Replace("+7", "").Replace("(", "").Replace(")", "").Replace("-", "");

            return new object[] { ID, LastName, FirstName, SecondName, organizationWithRegion.ShortName, subOrganizationShortName, Position.Name, mobileWithOutFormat, (organizationWithRegion as IHaveRegion).RealRegion.Name, (organizationWithRegion as IHaveRegion).City.Name };
        }

        public void Save()
        {
            int id;
            int.TryParse(_provider.Insert("SF_Person", ID, LastName, NumberSF, FirstName, SecondName, Appeal, Position.ID, MainSpecPerson.ID, AcademTitle.ID, Email, Mobile, Phone, Comment, Organization.ID), out id);

            ID = id;

            PersonList personList = GetPersonList();
            personList.Add(this);
        }

        public void Delete()
        {
            PersonList personList = GetPersonList();
            personList.Delete(this);
        }

        public bool CheckNamesake()
        {
            if (ID != 0)
                return false;

            PersonList personList = GetPersonList();
            return personList.CheckNamesake(this);
        }

        public bool IsOrganizationHaveUnique()
        {
            if ((ID != 0) || (!Position.Unique))
                return false;

            PersonList personList = GetPersonList();
            return personList.IsOrganizationHaveUnique(this);
        }

        private PersonList GetPersonList()
        {
            return PersonList.GetUniqueInstance();
        }

        public string ShortName { get { return LastName; } }
        public HistoryType Type { get { return HistoryType.person; } }
    }
}
