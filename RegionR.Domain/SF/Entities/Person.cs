using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Interfaces;
using RegionReport.Domain;

namespace ClassLibrary.SF.Entities
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
            LastName = row["person_lastName"].ToString();
            NumberSF = row["person_numberSF"].ToString();
            FirstName = row["person_firstName"].ToString();
            SecondName = row["person_secondName"].ToString();

            Appeal = Convert.ToInt32(row["appeal_id"].ToString());
            
            int idPosition;
            int.TryParse(row["position_id"].ToString(), out idPosition);
            PositionList positionList = PositionList.GetUniqueInstance();
            Position = positionList.GetItem(idPosition) as Position;
            
            int idMainSpecPerson;
            int.TryParse(row["mainSpecPerson_id"].ToString(), out idMainSpecPerson);
            MainSpecPersonList mainSpecPersonList = MainSpecPersonList.GetUniqueInstance();
            MainSpecPerson = mainSpecPersonList.GetItem(idMainSpecPerson) as MainSpecPerson;

            int idAcademTitle;
            int.TryParse(row["academTitle_id"].ToString(), out idAcademTitle);
            AcademTitleList academTitleList = AcademTitleList.GetUniqueInstance();
            AcademTitle = academTitleList.GetItem(idAcademTitle) as AcademTitle;

            Email = row["person_email"].ToString();
            Mobile = row["person_mobile"].ToString();
            Phone = row["person_phone"].ToString();
            Comment = row["person_comment"].ToString();

            int idOrganization;
            int.TryParse(row["organization_id"].ToString(), out idOrganization);
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            Organization = organizationList.GetItem(idOrganization);

            CrmID = row["person_crmID"].ToString();
            Deleted = false;
        }

        public string NumberSF { get; set; }
        public string CrmID { get; set; }
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
            Organization organization = (Organization == null) ? new LPU(TypeOrg.ЛПУ) :
                Organization.ParentOrganization == null ? Organization : Organization.ParentOrganization;

            return new object[] { 
                ID,
                CrmID,
                LastName,
                FirstName,
                SecondName,
                GetOrganizationName(),
                GetSubOrganizationName(),
                (Position == null) ? string.Empty : Position.Name, 
                (organization.RealRegion == null) ? string.Empty : organization.RealRegion.Name,
                organization.City
            };
        }

        //TODO: Сделать сохранение дня рождения
        public void Save()
        {
            int id;
            int.TryParse(_provider.Insert("SF_Person", ID, LastName, NumberSF, FirstName, SecondName, Appeal, (Position == null) ? 0 : Position.ID,
                (MainSpecPerson == null) ? 0 : MainSpecPerson.ID, (AcademTitle == null) ? 0 : AcademTitle.ID,
                Email, Mobile, Phone, Comment, (Organization == null) ? 0 : Organization.ID, Deleted.ToString(), CrmID), out id);

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

            return (Organization.ParentOrganization != null) ? Organization.ShortName : (Organization is LPU) ? "Администрация" : Organization.ShortName;
        }
    }
}
