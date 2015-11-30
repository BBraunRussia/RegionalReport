using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataLayer;

namespace ClassLibrary
{
    public class clAnswerList
    {
        private static clAnswerList uniqueInstance;
        private List<clAnswer> list;

        private clAnswerList()
        {
            list = new List<clAnswer>();

            LoadFromSql();
        }

        public static clAnswerList getInstance()
        {
            if (uniqueInstance == null)
                uniqueInstance = new clAnswerList();

            return uniqueInstance;
        }

        public void ReLoad()
        {
            LoadFromSql();
        }

        private void LoadFromSql()
        {
            list.Clear();

            sql sql1 = new sql();
            DataTable dt = sql1.GetRecords("exec Answer_Select");

            foreach (DataRow row in dt.Rows)
            {
                clAnswer answed = new clAnswer(row);
                Add(answed);
            }
        }

        public void Add(clAnswer answed)
        {
            list.Add(answed);
        }

        public clAnswer getItem(int id)
        {
            var answeds = from answed in list
                          where answed.IsEqualsID(id)
                          select answed;

            if (answeds.Count() > 0)
                return answeds.First() as clAnswer;
            else
                return new clAnswer(0);
        }

        public void Delete(int id)
        {
            clAnswer answed = getItem(id);

            list.Remove(answed);

            answed.Delete();
        }

        private List<clAnswer> ToList(int idQuestion)
        {
            var answeds = from answed in list
                          where answed.IsEqualsQuestionID(idQuestion)
                          select answed;

            return answeds.ToList();
        }

        public DataTable ToDataTable(int idQuestion)
        {
            List<clAnswer> answeds = ToList(idQuestion);

            return createTable(answeds);
        }

        private DataTable createTable(List<clAnswer> answeds)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("text");
            dt.Columns.Add("right");

            foreach (clAnswer answed in answeds)
            {
                dt.Rows.Add(answed.ToRow());
            }

            return dt;
        }
    }
}
