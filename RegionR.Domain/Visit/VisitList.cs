using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DataLayer;

namespace ClassLibrary
{
    public class VisitList : IEnumerable
    {
        private int idUser;
        private DateTime date;

        public readonly List<Visit> visitList;

        public VisitList(int idUser, DateTime date)
        {
            this.idUser = idUser;
            this.date = date;

            visitList = new List<Visit>();

            fillList();
        }

        private void fillList()
        {
            Sql sql1 = new Sql();
            DataTable dt = sql1.GetRecords("exec SelVisitPlanMonthByUser @p1, @p2", new DateTime(date.Year, date.Month, 1), idUser);

            foreach (DataRow row in dt.Rows)
            {
                visitList.Add(new Visit(Convert.ToInt32(row[0]), idUser));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public VisitEnum GetEnumerator()
        {
            return new VisitEnum(visitList);
        }
    }
}
