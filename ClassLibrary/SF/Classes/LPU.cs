using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class LPU : BaseDictionary, IMainOrganization
    {
        private string _numberSF;
        private string _shortName;
        private int _idTypeLPU;
        private int _idOwnership;
        private int _idAdmLevel;
        private int _idMainSpec;
        private int _idLpuRR;
        private string _inn;
        private string _kpp;
        private string _postAddress;
        private int _idCity;
        private string _street;
        private string _email;
        private string _website;
        private string _phone;
        private int _bedsTotal;
        private int _bedsIC;
        private int _bedsSurgical;
        private int _bedsOperating;
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
            _numberSF = row[1].ToString();
            _shortName = row[3].ToString();
            int.TryParse(row[4].ToString(), out _idTypeLPU);
            int.TryParse(row[5].ToString(), out _idOwnership);
            int.TryParse(row[6].ToString(), out _idAdmLevel);
            int.TryParse(row[7].ToString(), out _idMainSpec);
            int.TryParse(row[8].ToString(), out _idLpuRR);
            _inn = row[9].ToString();
            _kpp = row[10].ToString();
            _postAddress = row[11].ToString();
            int.TryParse(row[12].ToString(), out _idCity);
            _street = row[13].ToString();
            _email = row[14].ToString();
            _website = row[15].ToString();
            _phone = row[16].ToString();
            int.TryParse(row[17].ToString(), out _bedsTotal);
            int.TryParse(row[18].ToString(), out _bedsIC);
            int.TryParse(row[19].ToString(), out _bedsSurgical);
            int.TryParse(row[20].ToString(), out _bedsOperating);
            int.TryParse(row[21].ToString(), out _machineGD);
            int.TryParse(row[22].ToString(), out _machineGDF);
            int.TryParse(row[23].ToString(), out _machineCRRT);
            int.TryParse(row[24].ToString(), out _shift);
            int.TryParse(row[25].ToString(), out _patientGD);
            int.TryParse(row[26].ToString(), out _patientPD);
            int.TryParse(row[27].ToString(), out _patientCRRT);
        }

        public string INN
        {
            get { return _inn; }
        }

        public string KPP
        {
            get { throw new NotImplementedException(); }
        }

        public string MailAddress
        {
            get { throw new NotImplementedException(); }
        }

        public City City
        {
            get
            {
                CityList cityList = CityList.GetUniqueInstance();
                return cityList.GetItem(_idCity) as City;
            }
        }

        public District District { get { return City.District; } }
        public RealRegion RealRegion { get { return District.RealRegion; } }

        public string Street
        {
            get { throw new NotImplementedException(); }
        }

        public string AdministrativeLevel
        {
            get { throw new NotImplementedException(); }
        }
        
        public void AddChildOrganization()
        {
            throw new NotImplementedException();
        }

        public string NumberSF
        {
            get { return _numberSF; }
        }

        public TypeLPU TypeLPU
        {
            get
            {
                TypeLPUList typeLPUList = TypeLPUList.GetUniqueInstance();
                return typeLPUList.GetItem(_idTypeLPU) as TypeLPU;
            }
        }

        public string ShortName
        {
            get { throw new NotImplementedException(); }
        }

        public string Profile
        {
            get { throw new NotImplementedException(); }
        }

        public string Email
        {
            get { throw new NotImplementedException(); }
        }

        public string Website
        {
            get { throw new NotImplementedException(); }
        }

        public string Phone
        {
            get { throw new NotImplementedException(); }
        }

        public LpuRR LpuRR
        {
            get
            {
                LpuRRList lpuRRList = LpuRRList.GetUniqueInstance();
                return lpuRRList.GetItem(_idLpuRR) as LpuRR;
            }
        }

        public RegionRR RegionRR { get { return LpuRR.RegionRR; } }

        public DataTable GetEmployees()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public override DataRow GetRow()
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
            row[8] = RegionRR.Name;

            return row;
        }
    }
}
