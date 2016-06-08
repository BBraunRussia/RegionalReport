using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF.Import
{
    internal class OrganizationModel
    {
        internal OrganizationModel(DataRow row)
        {
            Parse(row);
        }

        public string NumberSF { get; private set; }
        public string RrID { get; private set; }
        public string RecordType { get; private set; }
        public string ParentNumberSF { get; private set; }
        public string Name { get; private set; }
        public string ShortName { get; private set; }
        public string INN { get; private set; }
        public string KPP { get; private set; }
        public string RealRegion { get; private set; }
        public string City { get; private set; }
        public string PostIndex { get; private set; }
        public string Street { get; private set; }
        public string AdmLevel { get; private set; }
        public string TypeFin { get; private set; }
        public string MainSpec { get; private set; }
        public string SubRegion { get; private set; }
        public string BedsTotal { get; private set; }
        public string BedsIC { get; private set; }
        public string BedsSurgical { get; private set; }
        public string Operating { get; private set; }
        public string MachineGD { get; private set; }
        public string MachineGDF { get; private set; }
        public string MachineCRRT { get; private set; }
        public string Shifts { get; private set; }
        public string PatientGD { get; private set; }
        public string PatientPD { get; private set; }
        public string PatientCRRT { get; private set; }
        public string Email { get; private set; }
        public string Website { get; private set; }
        public string Phone { get; private set; }
        public string Pharmacy { get; private set; }
        public string ClientType { get; private set; }
        public string TypeLPU { get; private set; }
        public string Ownership { get; private set; }
        public bool Deleted { get; private set; }

        private void Parse(DataRow row)
        {
            NumberSF = row["ID"].ToString();
            RrID = row["Z_RU_RR_INSTITUTION__C"].ToString();
            RecordType = row["Z_RECORD_TYPE_DEVELOPER_NAME__C"].ToString();
            ParentNumberSF = row["PARENTID"].ToString();
            Name = row["Z_RU_CUSTOMER_LONG_NAME__C"].ToString();
            ShortName = row["NAME"].ToString();
            INN = row["Z_RU_TIN__C"].ToString();
            KPP = row["Z_RU_CRR__C"].ToString();
            RealRegion = row["BILLINGSTATE"].ToString();
            City = row["BILLINGCITY"].ToString();
            PostIndex = row["BILLINGPOSTALCODE"].ToString();
            Street = row["BILLINGSTREET"].ToString();
            AdmLevel = row["Z_RU_TYPE_OF_GEOGRAPHICAL_LEVEL__C"].ToString();
            TypeFin = row["Z_SEE_FINANCIAL_STATUS__C"].ToString();
            MainSpec = row["Z_RU_INSTITUTION_MAIN_SPECIALITIES__C"].ToString();
            SubRegion = row["Z_R3_TERRITORIES_RU__C"].ToString();
            BedsTotal = row["Z_CD_NUMBER_OF_BEDS__C"].ToString();
            BedsIC = row["Z_ICU_BEDS__C"].ToString();
            BedsSurgical = row["Z_NUMBER_OF_ACUTE_BEDS_IN_HOSPITAL__C"].ToString();
            Operating = row["Z_RU_NUMBER_OF_SURGICAL_ROOMS__C"].ToString();
            MachineGD = row["Z_RU_NUMBER_OF_HD_DIALYSIS_MACHINES__C"].ToString();
            MachineGDF = row["Z_RU_NUMBER_OF_HDF_DIALYSIS_MACHINES__C"].ToString();
            MachineCRRT = row["Z_RU_NUMBER_OF_CRRT_MACHINES__C"].ToString();
            Shifts = row["Z_RU_NUMBER_OF_SHIFTS__C"].ToString();
            PatientGD = row["Z_RU_NUMBER_OF_PATIENT_HD__C"].ToString();
            PatientPD = row["Z_RU_NUMBER_OF_PATIENT_PD__C"].ToString();
            PatientCRRT = row["Z_RU_NUMBER_OF_ACCUTE_PATIENTS_PER_YEAR__C"].ToString();
            Email = row["Z_R3_EMAIL_ADDRESS__C"].ToString();
            Website = row["WEBSITE"].ToString();
            Phone = row["PHONE"].ToString();
            Pharmacy = row["Z_CUSTOMER_CLASSIFICATION__C"].ToString();
            ClientType = row["Z_RU_INSTITUTION_SUB_TYPE__C"].ToString(); //TypeOrg
            TypeLPU = row["Z_HOSPITAL_TYPE__C"].ToString();
            Ownership = row["OWNERSHIP"].ToString();
            Deleted = Convert.ToBoolean(row["Z_R3_INACTIVE__C"].ToString());
        }
    }
}
