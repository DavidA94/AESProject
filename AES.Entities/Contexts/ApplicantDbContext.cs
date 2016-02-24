using AES.Entities.Migrations;
using AES.Entities.Tables;
using System.Data.Entity;

namespace AES.Entities.Contexts
{
    public class ApplicantDbContext : DbContext
    {
        public ApplicantDbContext() : base("ApplicantDbContext") {
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicantDbContext, Configuration>());
        }

        public DbSet<ApplicantUser> ApplicantUsers { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
    }
}
