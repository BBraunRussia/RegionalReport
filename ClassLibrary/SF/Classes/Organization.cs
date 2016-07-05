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

            if (typeOrg == TypeOrg.ЛПУ)
                return new LPU(row);
            else if (idParent != 0)
                return new Organization(row);
            else
                return new OtherOrganization(row);
        }

        public static Organization CreateItem(TypeOrg typeOrg, Organization parentOrganization = null)
        {
            Organization organization;

            if (typeOrg == TypeOrg.ЛПУ)
            {
                organization = new LPU(typeOrg);
            }
            else if (parentOrganization == null)
            {
                organization = new OtherOrganization(typeOrg);
            }
            else
            {
                organization = new Organization(typeOrg);
            }

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

            int.TryParse(row[24].ToString(), out _machineGD);
            int.TryParse(row[25].ToString(), out _machineGDF);
            int.TryParse(row[26].ToString(), out _machineCRRT);
            int.TryParse(row[27].ToString(), out _shift);
            int.TryParse(row[28].ToString(), out _patientGD);
            int.TryParse(row[29].ToString(), out _patientPD);
            int.TryParse(row[30].ToString(), out _patientCRRT);

            bool deleted;
            bool.TryParse(row[35].ToString(), out deleted);
            Deleted = deleted;
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
        public string Name { get; set; }
        public string ShortName { get; set; }
        public TypeOrg TypeOrg { get; private set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public bool Deleted { get; set; }
        public HistoryType Type { get { return HistoryType.organization; } }
        public MainSpec MainSpec { get; set; }

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
            int.TryParse(_provider.Insert("SF_Organization", ID, NumberSF, TypeOrg, Name, ShortName, (MainSpec == null) ? 0 : MainSpec.ID, Email, Website, Phone,
                (ParentOrganization == null) ? 0 : ParentOrganization.ID,
                _machineGD, _machineGDF, _machineCRRT, _shift, _patientGD, _patientPD, _patientCRRT, Deleted.ToString()), out id);
            ID = id;

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            organizationList.Add(this);
        }

        public virtual void Delete()
        {
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            organizationList.Delete(this);
        }

        public bool IsBelongsINNToRealRegion()
        {
            if ((ID != 0) || (!(this is IHaveRegion)) || (ParentOrganization != null))
                return true;

            IHaveRegion organization = this as IHaveRegion;
            
            string idRealRegion = organization.RealRegion.ID.ToString();
            if (organization.RealRegion.ID < 10)
                idRealRegion = "0" + organization.RealRegion.ID.ToString();

            return idRealRegion == organization.INN.Substring(0, 2);
        }
    }
}
