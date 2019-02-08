using Antiguera.Administrador.Models.Auth;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Antiguera.Administrador.Context
{
    public class Contexto : IdentityDbContext<UserModel>
    {
        public Contexto()
            :base(@"Data Source=RENATO-PC\RENATO;Initial Catalog=Antiguera;Integrated Security=True")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static Contexto Create()
        {
            return new Contexto();
        }
    }
}