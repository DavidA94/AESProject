using AES.Entities.Migrations;
using AES.Entities.Tables;
using System.Data.Entity;

namespace AES.Entities.Contexts
{
    public class ApplicantDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicantDbContext, ApplicantConfiguration>());
        }

        public DbSet<ApplicantUser> ApplicantUsers { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }

    }
}
