using ClassLibrary.SF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegionReport.Domain.SF.Abstract
{
    public interface IOrganizationRepository
    {
        IQueryable<Organization> Organizations { get; }
        void Save(IOrganization organization);
        IOrganization Delete(int organizationID);
    }
}
