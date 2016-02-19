namespace AES.SecuritySvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ApplicantUsers", new[] { "SSN" });
            AlterColumn("dbo.ApplicantUsers", "SSN", c => c.String(nullable: false, maxLength: 88));
            CreateIndex("dbo.ApplicantUsers", "SSN", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.ApplicantUsers", new[] { "SSN" });
            AlterColumn("dbo.ApplicantUsers", "SSN", c => c.String(nullable: false, maxLength: 11));
            CreateIndex("dbo.ApplicantUsers", "SSN", unique: true);
        }
    }
}
