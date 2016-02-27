using AES.Entities.Migrations;
using AES.Entities.Tables;
using System.Data.Entity;

namespace AES.Entities.Contexts
{
    public class OpeningDbContext : DbContext
    {
        /*
        public ApplicantDbContext() : base("ApplicantDbContext") {
            // var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }*/

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicantDbContext, Configuration>());
        }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<JobOpening> JobOpenings { get; set; }

        public DbSet<Store> Stores { get; set; }
    }
}
