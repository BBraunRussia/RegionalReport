using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class Report
    {
        private int lpu_id;
        public Report(int lpu_id, string lpu_name, string lpu_sname)
        {
            this.lpu_id = lpu_id;
        }
        public int idLPU
        {
            get { return lpu_id; }
        }
    }
}
