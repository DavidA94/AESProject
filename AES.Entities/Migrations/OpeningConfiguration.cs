using AES.Entities.Tables;

namespace AES.Entities.Migrations
{
    using Contexts;
    using System.Data.Entity.Migrations;

    internal sealed class OpeningConfiguration : DbMigrationsConfiguration<OpeningDbContext>
    {
        public OpeningConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(OpeningDbContext context)
        {
            context.JobOpenings.RemoveRange(context.JobOpenings);
            context.Jobs.RemoveRange(context.Jobs);
            context.Stores.RemoveRange(context.Stores);

            var portlandStore = new Store
            {
                Address = "8000 SW Barbur Blvd.",
                City = "Portland",
                State = "OR",
                Name = "AES Electronics Portland",
                Phone = "503-555-0000",
                Zip = 97219
            };

            var tualatinStore = new Store
            {
                Address = "2700 SW Tualatin Sherwood Rd.",
                City = "Tualatin",
                State = "OR",
                Name = "AES Electronics Tualatin",
                Phone = "503-555-0000",
                Zip = 97062
            };

            var salesAssociateJob = new Job
            {
                title = "Sales Associate",
                descShort = "Responsible for all sales activities, from lead generation through to close. The successful candidate will be able to elevate company standards, achieve sales goals and meet clients expectations.",
                descLong = "Responsibilities:\n* Ensure high levels of customer satisfaction through excellent sales service\n* Assess customers needs and provide assistance and information on product features\n* 'Go the extra mile' to drive sales\n* Maintain in-stock and presentable condition assigned areas\n* Actively seek out customers in store\n* Remain knowledgeable on products offered and discuss available options\n* Cross sell products\n* Team up with co-workers to ensure proper customer service\n* Build productive trust relationships with customers\n\nRequirements:\n* Proven working experience as sales associate\n* Basic understanding of sales principles and customer service practices\n* Proficiency in English\n* Working knowledge of customer and market dynamics and requirements\n* Track record of over-achieving sales quota\n* Solid communication and interpersonal skills\n* Customer service focus\n* High school degree; BS degree in Marketing or related field would be a plus"
            };

            var janitorJob = new Job
            {
                title = "Maintenece Technician",
                descShort = "We are looking for a thorough Maintenance Technician to undertake the responsibility to preserve the good condition and functionality of premises. You will perform maintenance tasks of great variety such as painting, HVAC installations, landscaping etc.",
                descLong = "A Maintenance Technician is a thorough professional with a practical mind and attention to detail. The ideal candidate will be able to work autonomously and responsibly by observing all health and safety guidelines.\nThe goal is to maintain the buildings and common areas in the best possible condition."
            };

            context.Stores.AddOrUpdate(
                portlandStore,
                tualatinStore
            );

            context.Jobs.AddOrUpdate(
                salesAssociateJob,
                janitorJob
            );

            context.JobOpenings.AddOrUpdate(
                new JobOpening
                {
                    Store = tualatinStore,
                    Job = janitorJob
                },
                new JobOpening
                {
                    Store = portlandStore,
                    Job = janitorJob
                },
                new JobOpening
                {
                    Store = tualatinStore,
                    Job = salesAssociateJob
                },
                new JobOpening
                {
                    Store = portlandStore,
                    Job = salesAssociateJob
                }
            );

            context.SaveChanges();
        }
    }
}
