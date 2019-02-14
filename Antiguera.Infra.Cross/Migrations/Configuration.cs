namespace Antiguera.Infra.Cross.Migrations
{
    using Antiguera.Infra.Cross.Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Antiguera.Infra.Cross.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Antiguera.Infra.Cross.Infrastructure.ApplicationDbContext context)
        {
        }
    }
}
