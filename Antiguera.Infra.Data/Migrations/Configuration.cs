namespace Antiguera.Infra.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Antiguera.Infra.Data.Contexto.AntigueraContexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Antiguera.Infra.Data.Contexto.AntigueraContexto context)
        {
        }
    }
}