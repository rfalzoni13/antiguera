using Microsoft.AspNet.Identity.EntityFramework;

namespace Antiguera.Administrador.Config
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            :base(@"Data Source=RENATO-PC\RENATO;Initial Catalog=Antiguera;Integrated Security=True", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
