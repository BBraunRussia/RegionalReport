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
        private int _idLPU;
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
            int.TryParse(row[8].ToString(), out _idLPU);
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

        public string KPP
        {
            get { throw new NotImplementedException(); }
        }

        public string MailAddress
        {
            get { throw new NotImplementedException(); }
        }

        public string Region
        {
            get { throw new NotImplementedException(); }
        }

        public string District
        {
            get { throw new NotImplementedException(); }
        }

        public string City
        {
            get { throw new NotImplementedException(); }
        }

        public string Street
        {
            get { throw new NotImplementedException(); }
        }

        public string AdministrativeLevel
        {
            get { throw new NotImplementedException(); }
        }

        public string GetINN()
        {
            throw new NotImplementedException();
        }

        public void AddChildOrganization()
        {
            throw new NotImplementedException();
        }

        public string NumberSF
        {
            get { throw new NotImplementedException(); }
        }

        public string Type
        {
            get { throw new NotImplementedException(); }
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

        public DataTable GetEmployees()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
