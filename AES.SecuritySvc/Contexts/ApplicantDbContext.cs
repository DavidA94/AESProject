using System.Data.Entity;
using AES.Entities.Tables;

namespace AES.Entities.Contexts
{
    public class ApplicantDbContext : DbContext
    {
        public ApplicantDbContext() : base("ApplicantDbContext") {
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public DbSet<ApplicantUser> ApplicantUsers { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
    }
}
