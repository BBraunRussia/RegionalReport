using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataLayer;

namespace ClassLibrary
{
    public class clQuestionList
    {
        private static clQuestionList uniqueInstance;

        private List<clQuestion> list;

        private clQuestionList()
        {
            list = new List<clQuestion>();

            LoadFromSql();
        }

        public static clQuestionList getInstance()
        {
            if (uniqueInstance == null)
                uniqueInstance = new clQuestionList();

            return uniqueInstance;
        }

        public void ReLoad()
        {
            LoadFromSql();
        }

        private void LoadFromSql()
        {
            sql sql1 = new sql();
            DataTable dt = sql1.GetRecords("exec Question_Select");

            list.Clear();

            foreach (DataRow row in dt.Rows)
            {
                clQuestion question = new clQuestion(row);
                Add(question);
            }
        }

        public void Add(clQuestion question)
        {
            list.Add(question);
        }

        public void Delete(int id)
        {
            clQuestion question = getItem(id);

            list.Remove(question);

            question.Delete();
        }

        public clQuestion getItem(int idQuestion)
        {
            var questions = from question in list
                            where question.IsEqualsID(idQuestion)
                            select question;

            if (questions.Count() > 0)
                return questions.First() as clQuestion;
            else
                return new clQuestion(0);
        }

        public DataTable ToDataTable(int idUser)
        {
            var questions = from question in list
                            where question.IsEqualsUserID(idUser)
                            select question;

            return createTable(questions.ToList());
        }

        private DataTable createTable(List<clQuestion> questions)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("text");
            dt.Columns.Add("time");

            foreach (clQuestion question in questions)
                dt.Rows.Add(question.ToRow());

            return dt;
        }
    }
}
