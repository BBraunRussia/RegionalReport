using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class Person : BaseDictionary, IHistory
    {
        private string _mobile;
                
        public Person()
        {
            if (NumberSF == null)
            {
                NumberSF = string.Empty;
            }
        }

        public Person(DataRow row)
            : base(row)
        {
            LastName = row[1].ToString();
            NumberSF = row[2].ToString();
            FirstName = row[3].ToString();
            SecondName = row[4].ToString();

            Appeal = Convert.ToInt32(row[5].ToString());
            
            int idPosition;
            int.TryParse(row[6].ToString(), out idPosition);
            PositionList positionList = PositionList.GetUniqueInstance();
            Position = positionList.GetItem(idPosition) as Position;
            
            int idMainSpecPerson;
            int.TryParse(row[7].ToString(), out idMainSpecPerson);
            MainSpecPersonList mainSpecPersonList = MainSpecPersonList.GetUniqueInstance();
            MainSpecPerson = mainSpecPersonList.GetItem(idMainSpecPerson) as MainSpecPerson;

            int idAcademTitle;
            int.TryParse(row[8].ToString(), out idAcademTitle);
            AcademTitleList academTitleList = AcademTitleList.GetUniqueInstance();
            AcademTitle = academTitleList.GetItem(idAcademTitle) as AcademTitle;

            Email = row[9].ToString();
            Mobile = row[10].ToString();
            Phone = row[11].ToString();
            Comment = row[12].ToString();

            int idOrganization;
            int.TryParse(row[13].ToString(), out idOrganization);
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            Organization = organizationList.GetItem(idOrganization);
            /*
            if (Organization == null)
            {
                CanAdd = false;
            }
            */
            Deleted = Convert.ToBoolean(row[14].ToString());
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public Organization Organization { get; set; }
        public Position Position { get; set; }
        public MainSpecPerson MainSpecPerson { get; set; }
        public AcademTitle AcademTitle { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
        public DateTime? DateBirth { get; set; }
        public string NumberSF { get; set; }
        public int Appeal { get; set; }
        public string ShortName { get { return LastName; } }
        public HistoryType Type { get { return HistoryType.person; } }
        public bool Deleted { get; set; }

        public string Mobile
        {
            get { return (_mobile == "+7(   )   -  -") ? string.Empty : _mobile; }
            set { _mobile = value; }
        }

        public override object[] GetRow()
        {
            IHaveRegion region = (Organization == null) ? new LPU(TypeOrg.ЛПУ) : 
                ((Organization is IHaveRegion) ? Organization : Organization.ParentOrganization) as IHaveRegion;

            return new object[] { ID, NumberSF, LastName, FirstName, SecondName, GetOrganizationName(),
                GetSubOrganizationName(), (Position == null) ? string.Empty : Position.Name, 
                (region.RealRegion == null) ? string.Empty : region.RealRegion.Name, (region.City == null) ? string.Empty : region.City.Name };
        }

        //TODO: Сделать сохранение дня рождения
        public void Save()
        {
            int id;
            int.TryParse(_provider.Insert("SF_Person", ID, LastName, NumberSF, FirstName, SecondName, Appeal, (Position == null) ? 0 : Position.ID,
                (MainSpecPerson == null) ? 0 : MainSpecPerson.ID, (AcademTitle == null) ? 0 : AcademTitle.ID,
                Email, Mobile, Phone, Comment, (Organization == null) ? 0 : Organization.ID, Deleted.ToString()), out id);

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
        
        public string GetOrganizationName()
        {
            if (Organization == null)
            {
                return string.Empty;
            }

            return (Organization.ParentOrganization == null) ? Organization.ShortName : Organization.ParentOrganization.ShortName;
        }

        public string GetSubOrganizationName()
        {
            if (Organization == null)
            {
                return string.Empty;
            }

            return (Organization is OtherOrganization) ? string.Empty : (Organization.ParentOrganization == null) ? "Администрация" : Organization.ShortName;
        }
    }
}
