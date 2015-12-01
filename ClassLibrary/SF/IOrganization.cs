using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public interface IOrganization
    {
        string NumberSF { get; }
        string Type { get; }
        string Name { get; }
        string ShortName { get; }
        string Profile { get; }
        string Email { get; }
        string Website { get; }
        string Phone { get; }

        DataTable GetEmployees();
        void Save();
    }
}
