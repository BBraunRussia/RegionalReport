using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class Person : BaseDictionary
    {
        private string _lastName;
        private string _numberSF;
        private string _firstName;
        private string _secondName;
        private string _appeal;
        private string _position;
        private string _specialization;
        private string _email;
        private string _mobile;
        private string _phone;
        private Organization _organization;
        private string _comment;

        public Person(DataRow row)
            : base(row)
        {
            _lastName = row[1].ToString();
            _numberSF = row[2].ToString();
            _firstName = row[3].ToString();
            _secondName = row[4].ToString();
            _appeal = row[5].ToString();
            _position = row[6].ToString();
            _specialization = row[7].ToString();
            _email = row[8].ToString();
            _mobile = row[9].ToString();
            _phone = row[10].ToString();

            int idOrganization;
            int.TryParse(row[11].ToString(), out idOrganization);
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            _organization = organizationList.GetItem(idOrganization);

            _comment = row[12].ToString();
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

        public Organization Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }

        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public string NumberSF
        {
            get { return _numberSF; }
            set { _numberSF = value; }
        }
        
        public override object[] GetRow()
        {
            LPU lpu = (Organization as LPU);

            return new object[] { ID, LastName, FirstName, SecondName, lpu.ShortName, Position, lpu.City.Name, lpu.RealRegion.RegionRR, NumberSF };
        }
    }
}
