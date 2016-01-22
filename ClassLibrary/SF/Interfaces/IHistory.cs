using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public interface IHistory
    {
        int ID { get; }
        string ShortName { get; }
        HistoryType Type { get; }
    }
}
