using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataLayer;

namespace ClassLibrary
{
    public class clQuestion
    {
        private int id;
        private int idUser;
        private int time;
        private string text;
        private Sql sql1;

        public string Time
        {
            get
            {
                return time.ToString();
            }
            set
            {
                int.TryParse(value, out time);
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        public clQuestion(int idUser)
        {
            id = 0;
            this.idUser = idUser;
            Init();
        }

        public clQuestion(DataRow row)
        {
            fillFields(row);
            Init();
        }

        private void Init()
        {
            sql1 = new Sql();
        }

        private void fillFields(DataRow row)
        {
            int.TryParse(row.ItemArray[0].ToString(), out id);
            int.TryParse(row.ItemArray[1].ToString(), out idUser);
            text = row.ItemArray[2].ToString();
            int.TryParse(row.ItemArray[3].ToString(), out time);
        }

        public void Save()
        {
            if (time == 0)
                time = 60;

            int.TryParse(sql1.GetRecordsOne("exec Question_Insert @p1, @p2, @p3, @p4", id, idUser, text, time), out id);
        }

        internal bool IsEqualsID(int id)
        {
            return this.id == id;
        }

        internal void Delete()
        {
            sql1.GetRecords("exec Question_Delete @p1", id);
        }

        internal bool IsEqualsUserID(int idUser)
        {
            return this.idUser == idUser;
        }

        internal object[] ToRow()
        {
            return new object[3] { id, text, time };
        }

        public bool IsNew()
        {
            return id == 0;
        }

        public clAnswer CreateAnswed()
        {
            return new clAnswer(id);
        }
    }
}
