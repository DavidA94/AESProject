using System.Data.Entity;
using AES.Entities.Migrations;
using AES.Entities.Tables;

namespace AES.Entities.Contexts
{
    public class AESDbContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AESDbContext, AESDbConfiguration>());
        }

        public DbSet<ApplicantUser> ApplicantUsers { get; set; }
        public DbSet<ApplicantUser> EmployeeUsers { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<JobQuestion> Questions { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobOpening> JobOpenings { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<JobHistory> JobHistories { get; set; }
        public DbSet<EducationHistory> EducationHistories { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<ApplicationShortAnswer> ShortAnswers { get; set; }
        public DbSet<ApplicationMultiAnswer> MultiAnswers { get; set; }

    }
}