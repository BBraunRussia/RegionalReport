using RegionReport.Domain.SF.Abstract;
using System.Data.Entity;

namespace ClassLibrary.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<IOrganization> Organizations { get; set; }
    }
}