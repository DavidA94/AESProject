using System;
using System.Collections.Generic;

using AES.Entities.Migrations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AES.Entities.Tables;

namespace AES.Entities.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, [??]>());
        }

        public DbSet<ApplicantUser> Users { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<JobQuestion> Questions { get; set; }
    }
}
