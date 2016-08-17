using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF.Interfaces;
using ClassLibrary.SF.Lists;
using RegionReport.Domain;

namespace ClassLibrary.SF.Entities
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
            TypeOrg typeOrg = (TypeOrg)Convert.ToInt32(row["TypeOrg_id"].ToString());
            int idParent;
            int.TryParse(row["parent_id"].ToString(), out idParent);

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
            int.TryParse(row["Organization_id"].ToString(), out id);
            ID = id;

            NumberSF = row["Organization_NumberSF"].ToString();

            int idTypeOrg;
            int.TryParse(row["TypeOrg_id"].ToString(), out idTypeOrg);
            TypeOrg = (TypeOrg)idTypeOrg;

            Name = row["Organization_name"].ToString();
            ShortName = row["Organization_sName"].ToString();
            
            int idMainSpec;
            int.TryParse(row["MainSpec_id"].ToString(), out idMainSpec);
            MainSpec = MainSpecList.GetUniqueInstance().GetItem(idMainSpec) as MainSpec;

            Email = row["Organization_email"].ToString();
            Website = row["Organization_website"].ToString();
            Phone = row["Organization_phone"].ToString();

            int.TryParse(row["parent_id"].ToString(), out _idParentOrganization);

            KPP = row["Organization_KPP"].ToString();
            PostIndex = row["Organization_PostIndex"].ToString();

            int idCity;
            int.TryParse(row["City_id"].ToString(), out idCity);
            City = CityList.GetUniqueInstance().GetItem(idCity) as City;

            Street = row["Organization_street"].ToString();
            INN = row["Organization_INN"].ToString();

            int.TryParse(row["Organization_machineGD"].ToString(), out _machineGD);
            int.TryParse(row["Organization_machineGDF"].ToString(), out _machineGDF);
            int.TryParse(row["Organization_machineCRRT"].ToString(), out _machineCRRT);
            int.TryParse(row["Organization_shift"].ToString(), out _shift);
            int.TryParse(row["Organization_patientGD"].ToString(), out _patientGD);
            int.TryParse(row["Organization_patientPD"].ToString(), out _patientPD);
            int.TryParse(row["Organization_patientCRRT"].ToString(), out _patientCRRT);

            Pharmacy = row["pharmacy"].ToString();
            CrmID = row["organization_crmID"].ToString();
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

            return new object[] { ID, CrmID, ShortName, typeOrgName, INN, (RealRegion == null) ? string.Empty : RealRegion.Name,
                (City == null) ? string.Empty : City.Name, string.Empty, string.Empty, string.Empty };
        }
    }
}
