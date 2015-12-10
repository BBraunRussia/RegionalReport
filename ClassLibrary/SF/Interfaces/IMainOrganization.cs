using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public interface IMainOrganization : IOrganization
    {
        string KPP { get; }
        string MailAddress { get; }
        RealRegion RealRegion { get; }
        District District { get; }
        City City { get; }
        string Street { get; }
        string AdministrativeLevel { get; }
        string INN { get; }

        void AddChildOrganization();
    }
}
