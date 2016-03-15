using AES.Entities.Migrations;
using AES.Entities.Tables;
using System.Data.Entity;

namespace AES.Entities.Contexts
{
    public class OpeningDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<OpeningDbContext, OpeningConfiguration>());
        }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<JobOpening> JobOpenings { get; set; }

        public DbSet<Store> Stores { get; set; }
    }
}
