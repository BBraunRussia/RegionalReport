using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF.Import
{
    internal class PersonModel
    {
        internal PersonModel(DataRow row)
        {
            Parse(row);
        }

        public string NumberSF { get; private set; }
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public string SecondName { get; private set; }
        public int Appeal { get; private set; }
        public string AcademTitle { get; private set; }
        public string ID { get; private set; }
        public string MainSpecPerson { get; private set; }
        public string Position { get; private set; }
        public string Email { get; private set; }
        public string Mobile { get; private set; }
        public string Phone { get; private set; }
        public string Comment { get; private set; }
        public bool Deleted { get; private set; }

        private void Parse(DataRow row)
        {
            NumberSF = row["ID"].ToString();
            //"Z_RECORD_TYPE_DEVELOPER_NAME__C" значение RU_Person не понятно для чего его выгружать
            LastName = row["LASTNAME"].ToString();
            FirstName = row["FIRSTNAME"].ToString();
            SecondName = row["MIDDLENAME"].ToString();

            int appeal = Convert.ToInt32(row["SALUTATION"].ToString()); //1-male 3-female
            Appeal = (appeal == 1) ? 0 : 1;

            AcademTitle = row["Z_CD_ACADEMIC_TITLE__PC"].ToString();
            ID = row["Z_RU_RR_PERSON__PC"].ToString();
            MainSpecPerson = row["Z_MAJOR_SPECIALTIES_RU__PC"].ToString();
            Position = row["Z_ES_PRIMARY_FUNCTION__PC"].ToString();
            Email = row["Z_PWR_EMAIL__PC"].ToString();
            Mobile = row["PERSONMOBILEPHONE"].ToString();
            Phone = row["Z_PWR_PHONE_NO__PC"].ToString();
            Comment = row["Description"].ToString();
            Deleted = Convert.ToBoolean(row["Z_R3_INACTIVE__C"].ToString());
        }
    }
}
