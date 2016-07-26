using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class Organization : IHistory, IAvitum
    {
        private int _idParentOrganization;

        public string Pharmacy { get; set; }

        protected int _machineGD;
        protected int _machineGDF;
        protected int _machineCRRT;
        protected int _shift;
        protected int _patientGD;
        protected int _patientPD;
        protected int _patientCRRT;

        protected IProvider _provider;

        public static Organization CreateItem(DataRow row)
        {
            TypeOrg typeOrg = (TypeOrg)Convert.ToInt32(row[2].ToString());
            int idParent;
            int.TryParse(row[9].ToString(), out idParent);

            return (typeOrg == TypeOrg.ЛПУ) ? new LPU(row) : new Organization(row);
        }

        public static Organization CreateItem(TypeOrg typeOrg, Organization parentOrganization = null)
        {
            Organization organization = (typeOrg == TypeOrg.ЛПУ) ? new LPU(typeOrg) : new Organization(typeOrg);
            
            organization.ParentOrganization = parentOrganization;

            return organization;
        }

        internal Organization(DataRow row)
        {
            _provider = Provider.GetProvider();

            int id;
            int.TryParse(row[0].ToString(), out id);
            ID = id;

            NumberSF = row[1].ToString();

            int idTypeOrg;
            int.TryParse(row[2].ToString(), out idTypeOrg);
            TypeOrg = (TypeOrg)idTypeOrg;

            Name = row[3].ToString();
            ShortName = row[4].ToString();
            
            int idMainSpec;
            int.TryParse(row[5].ToString(), out idMainSpec);
            MainSpec = MainSpecList.GetUniqueInstance().GetItem(idMainSpec) as MainSpec;

            Email = row[6].ToString();
            Website = row[7].ToString();
            Phone = row[8].ToString();

            int.TryParse(row[9].ToString(), out _idParentOrganization);

            KPP = row[13].ToString();
            PostIndex = row[14].ToString();

            int idCity;
            int.TryParse(row[15].ToString(), out idCity);
            City = CityList.GetUniqueInstance().GetItem(idCity) as City;

            Street = row[17].ToString();
            INN = row[18].ToString();

            int.TryParse(row[24].ToString(), out _machineGD);
            int.TryParse(row[25].ToString(), out _machineGDF);
            int.TryParse(row[26].ToString(), out _machineCRRT);
            int.TryParse(row[27].ToString(), out _shift);
            int.TryParse(row[28].ToString(), out _patientGD);
            int.TryParse(row[29].ToString(), out _patientPD);
            int.TryParse(row[30].ToString(), out _patientCRRT);
            
            Pharmacy = row[31].ToString();
            CrmID = row[35].ToString();
            Deleted = false;
        }

        public Organization(TypeOrg typeOrg)
        {
            _provider = Provider.GetProvider();

            TypeOrg = typeOrg;
            NumberSF = string.Empty;
            ShortName = string.Empty;
        }

        public int ID { protected set; get; }
        public string NumberSF { get; set; }
        public string CrmID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public TypeOrg TypeOrg { get; private set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public bool Deleted { get; set; }
        public HistoryType Type { get { return HistoryType.organization; } }
        public MainSpec MainSpec { get; set; }

        public string KPP { get; set; }
        public string PostIndex { get; set; }
        public string Street { get; set; }
        public City City { get; set; }
        public string INN { get; set; }
        public RealRegion RealRegion
        {
            get { return (City == null) ? null : City.RealRegion; }
            set
            {
                CityList cityList = CityList.GetUniqueInstance();
                City city = cityList.GetItem(value);
                City = city;
            }
        }

        public string MachineGD
        {
            get { return (_machineGD == 0) ? string.Empty : _machineGD.ToString(); }
            set { int.TryParse(value, out _machineGD); }
        }
        public string MachineGDF
        {
            get { return (_machineGDF == 0) ? string.Empty : _machineGDF.ToString(); }
            set { int.TryParse(value, out _machineGDF); }
        }
        public string MachineCRRT
        {
            get { return (_machineCRRT == 0) ? string.Empty : _machineCRRT.ToString(); }
            set { int.TryParse(value, out _machineCRRT); }
        }
        public string Shift
        {
            get { return (_shift == 0) ? string.Empty : _shift.ToString(); }
            set { int.TryParse(value, out _shift); }
        }
        public string PatientGD
        {
            get { return (_patientGD == 0) ? string.Empty : _patientGD.ToString(); }
            set { int.TryParse(value, out _patientGD); }
        }
        public string PatientPD
        {
            get { return (_patientPD == 0) ? string.Empty : _patientPD.ToString(); }
            set { int.TryParse(value, out _patientPD); }
        }
        public string PatientCRRT
        {
            get { return (_patientCRRT == 0) ? string.Empty : _patientCRRT.ToString(); }
            set { int.TryParse(value, out _patientCRRT); }
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
            set
            {
                if (value != null)
                    _idParentOrganization = value.ID;
            }
        }
        
        public virtual void Save()
        {
            int id;
            int.TryParse(_provider.Insert("SF_Organization",
                ID,
                NumberSF,
                TypeOrg,
                Name,
                ShortName,
                (MainSpec == null) ? 0 : MainSpec.ID,
                Email,
                Website,
                Phone,
                (ParentOrganization == null) ? 0 : ParentOrganization.ID,
                _machineGD,
                _machineGDF,
                _machineCRRT,
                _shift,
                _patientGD,
                _patientPD,
                _patientCRRT,
                INN,
                KPP,
                PostIndex,
                (City == null) ? 0 : ID,
                Street,
                Pharmacy,
                Deleted.ToString(),
                CrmID
                ), out id);

            ID = id;

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            organizationList.Add(this);
        }

        public virtual void Delete()
        {
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            organizationList.Delete(this);
        }
        
        public virtual object[] GetRow()
        {
            string typeOrgName = TypeOrg.ToString();

            return new object[] { ID, NumberSF, ShortName, typeOrgName, INN, (RealRegion == null) ? string.Empty : RealRegion.Name,
                (City == null) ? string.Empty : City.Name, string.Empty, string.Empty, string.Empty };
        }
    }
}
