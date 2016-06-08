using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF.Import
{
    public class ReadFilePerson
    {
        private AcademTitleList academTitleList;
        private MainSpecPersonList mainSpecPersonList;
        private PositionList positionList;

        public ReadFilePerson()
        {
            academTitleList = AcademTitleList.GetUniqueInstance();
            mainSpecPersonList = MainSpecPersonList.GetUniqueInstance();
            positionList = PositionList.GetUniqueInstance();
        }

        public void Start()
        {
            ReadFile readFile = new ReadFile(ImportFileType.Person);

            if (readFile.dt == null)
                return;

            foreach (DataRow row in readFile)
            {
                Update(new PersonModel(row));
            }
        }

        private void Update(PersonModel model)
        {
            Person person = GetPerson(model);
            if (person == null)
                return;

            person.NumberSF = model.NumberSF;
            person.LastName = model.LastName;
            person.FirstName = model.FirstName;
            person.SecondName = model.SecondName;
            person.Appeal = model.Appeal;
            person.AcademTitle = academTitleList.GetItem(model.AcademTitle) as AcademTitle;
            person.MainSpecPerson = mainSpecPersonList.GetItem(model.MainSpecPerson) as MainSpecPerson;
            person.Position = positionList.GetItem(model.Position) as Position;
            person.Email = model.Email;
            person.Mobile = model.Mobile;
            person.Phone = model.Phone;
            person.Comment = model.Comment;
            person.Deleted = model.Deleted;

            person.Save();
        }

        private Person GetPerson(PersonModel model)
        {
            PersonList personList = PersonList.GetUniqueInstance();

            int id = Convert.ToInt32(model.ID);

            //create person if not find it
            return (personList.GetItem(id) as Person) ?? new Person();
        }
    }
}
