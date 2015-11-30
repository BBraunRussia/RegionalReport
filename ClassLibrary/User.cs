using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer;

namespace ClassLibrary
{
    public class User
    {
        private int id;

        public User(int id)
        {
            this.id = id;
        }

        public void ClearAcc(DateTime date)
        {
            sql sql = new sql();
            sql.GetRecords("exec Acc_ClearFact_ByUser @p1, @p2", id, date);
        }
    }
}
