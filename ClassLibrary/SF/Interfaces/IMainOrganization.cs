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
        string Region { get; }
        string District { get; }
        string City { get; }
        string Street { get; }
        string AdministrativeLevel { get; }

        string GetINN();
        void AddChildOrganization();
    }
}
