using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class LPU : MainOrganization
    {
        private LpuRR _lpuRR;
        private string _inn;

        private int _bedsTotal;
        private int _bedsIC;
        private int _surgical;
        private int _operating;
        private int _machineGD;
        private int _machineGDF;
        private int _machineCRRT;
        private int _shift;
        private int _patientGD;
        private int _patientPD;
        private int _patientCRRT;

        public LPU(DataRow row)
            : base(row)
        {
            _inn = row[16].ToString();

            int idLpuRR;
            int.TryParse(row[17].ToString(), out idLpuRR);
            LpuRRList lpuRRList = LpuRRList.GetUniqueInstance();
            _lpuRR = lpuRRList.GetItem(idLpuRR) as LpuRR;

            int.TryParse(row[18].ToString(), out _bedsTotal);
            int.TryParse(row[19].ToString(), out _bedsIC);
            int.TryParse(row[20].ToString(), out _surgical);
            int.TryParse(row[21].ToString(), out _operating);
            int.TryParse(row[22].ToString(), out _machineGD);
            int.TryParse(row[23].ToString(), out _machineGDF);
            int.TryParse(row[24].ToString(), out _machineCRRT);
            int.TryParse(row[25].ToString(), out _shift);
            int.TryParse(row[26].ToString(), out _patientGD);
            int.TryParse(row[27].ToString(), out _patientPD);
            int.TryParse(row[28].ToString(), out _patientCRRT);
        }

        public LPU(TypeOrg typeOrg)
            : base(typeOrg)
        { }

        public string INN
        {
            get { return _inn; }
            set { _inn = value; }
        }

        public LpuRR LpuRR
        {
            get { return _lpuRR; }
            set { _lpuRR = value; }
        }

        public string BedsTotal
        {
            get { return (_bedsTotal == 0) ? string.Empty : _bedsTotal.ToString(); }
            set { int.TryParse(value, out _bedsTotal); }
        }
        public string BedsIC
        {
            get { return (_bedsIC == 0) ? string.Empty : _bedsIC.ToString(); }
            set { int.TryParse(value, out _bedsIC); }
        }
        public string Surgical
        {
            get { return (_surgical == 0) ? string.Empty : _surgical.ToString(); }
            set { int.TryParse(value, out _surgical); }
        }
        public string Operating
        {
            get { return (_operating == 0) ? string.Empty : _operating.ToString(); }
            set { int.TryParse(value, out _operating); }
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

        public DataRow GetRow()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Номер SF");
            dt.Columns.Add("Название организации");
            dt.Columns.Add("Тип");
            dt.Columns.Add("ИНН");
            dt.Columns.Add("Регион федерации");
            dt.Columns.Add("Город");
            dt.Columns.Add("Сопоставление ЛПУ-RR");
            dt.Columns.Add("Регион RR");

            DataRow row = dt.NewRow();
            row[0] = ID;
            row[1] = NumberSF;
            row[2] = Name;
            row[3] = TypeLPU.Name;
            row[4] = INN;
            row[5] = RealRegion.Name;
            row[6] = City.Name;
            row[7] = LpuRR.Name;
            row[8] = LpuRR.RegionRR.Name;

            return row;
        }

        public override void Save()
        {
            IProvider _provider = Provider.GetProvider();

            _provider.Insert("SF_LPU", ID, NumberSF, TypeOrg, Name, ShortName, MainSpec.ID, Email, WebSite, Phone, TypeLPU.ID, Ownership.ID, AdmLevel.ID,
                INN, KPP, PostIndex, City.ID, Street, LpuRR.ID,
                _bedsTotal, _bedsIC, _surgical, _operating, _machineGD, _machineGDF, _machineCRRT, _shift, _patientGD, _patientPD, _patientCRRT);

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            organizationList.Add(this);
        }
    }
}
