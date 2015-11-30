using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataLayer;

namespace ClassLibrary
{
    public class clAnswer
    {
        private int id;
        private int idQuestion;
        private string text;
        private int right;
        private sql sql1;

        public bool Right
        {
            get
            {
                return Convert.ToBoolean(right);
            }
            set
            {
                right = Convert.ToInt32(value);
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

        public int ID
        {
            get
            {
                return id;
            }
        }

        public int questionID
        {
            get
            {
                return idQuestion;
            }
        }


        internal clAnswer(int idQuestion)
        {
            this.idQuestion = idQuestion;
            Init();
        }

        internal clAnswer(DataRow row)
        {
            fillFields(row);
            Init();
        }

        private void fillFields(DataRow row)
        {
            int.TryParse(row.ItemArray[0].ToString(), out id);
            int.TryParse(row.ItemArray[1].ToString(), out idQuestion);
            text = row.ItemArray[2].ToString();
            int.TryParse(row.ItemArray[3].ToString(), out right);
        }

        private void Init()
        {
            sql1 = new sql();
        }

        public void Save()
        {
            int.TryParse(sql1.GetRecordsOne("exec Answer_Insert @p1, @p2, @p3, @p4", id, idQuestion, text, right), out id);
        }

        internal void Delete()
        {
            sql1.GetRecords("exec Answer_Delete @p1", id);
        }

        internal bool IsEqualsID(int id)
        {
            return this.id == id;
        }

        internal bool IsEqualsQuestionID(int idQuestion)
        {
            return this.idQuestion == idQuestion;
        }

        internal object[] ToRow()
        {
            return new object[3] { id, text, right };
        }
    }
}
