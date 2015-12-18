using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class Organization
    {
        private int _id;
        private string _numberSF;
        private TypeOrg _typeOrg;
        private string _name;
        private string _sName;
        private int _idMainSpec;
        private string _email;
        private string _website;
        private string _phone;

        private int _idParentOrganization;

        protected IProvider _provider;

        internal Organization(DataRow row)
        {
            _provider = Provider.GetProvider();

            int.TryParse(row[0].ToString(), out _id);
            _numberSF = row[1].ToString();

            int idTypeOrg;
            int.TryParse(row[2].ToString(), out idTypeOrg);
            _typeOrg = (TypeOrg)idTypeOrg;

            _name = row[3].ToString();
            _sName = row[4].ToString();
            int.TryParse(row[5].ToString(), out _idMainSpec);
            _email = row[6].ToString();
            _website = row[7].ToString();
            _phone = row[8].ToString();

            int.TryParse(row[9].ToString(), out _idParentOrganization);
        }

        internal Organization(TypeOrg typeOrg)
        {
            _provider = Provider.GetProvider();

            _typeOrg = typeOrg;
            _numberSF = string.Empty;
            _sName = string.Empty;
        }

        public int ID { get { return _id; } }
        public string NumberSF { get { return _numberSF; } }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string ShortName
        {
            get { return _sName; }
            set { _sName = value; }
        }
        public TypeOrg TypeOrg { get { return _typeOrg; } }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string WebSite
        {
            get { return _website; }
            set { _website = value; }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public MainSpec MainSpec
        {
            get
            {
                if (_idMainSpec == 0)
                    return null;

                MainSpecList mainSpecList = MainSpecList.GetUniqueInstance();
                return mainSpecList.GetItem(_idMainSpec) as MainSpec;
            }
            set { _idMainSpec = value.ID; }
        }

        public static Organization CreateItem(DataRow row)
        {
            TypeOrg typeOrg = (TypeOrg) Convert.ToInt32(row[2].ToString());

            return (typeOrg == TypeOrg.ЛПУ) ? new LPU(row) : new Organization(row);
        }

        public Organization ParentOrganization
        {
            get
            {
                if (_idParentOrganization == 0)
                    return null;

                OrganizationList organizationList = OrganizationList.GetUniqueInstance();
                return organizationList.GetItem(_idParentOrganization);
            }
            set { _idParentOrganization = value.ID; }
        }

        public static Organization CreateItem(TypeOrg typeOrg)
        {
            return (typeOrg == TypeOrg.ЛПУ) ? new LPU(typeOrg) : new Organization(typeOrg);
        }

        protected void SetID(int id)
        {
            _id = id;
        }

        public virtual void Save()
        {
            int id;
            int.TryParse(_provider.Insert("SF_Organization", ID, NumberSF, TypeOrg, Name, ShortName, MainSpec.ID, Email, WebSite, Phone, ParentOrganization.ID), out id);
            SetID(id);

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            organizationList.Add(this);
        }

        public virtual void Delete()
        { }
    }
}
