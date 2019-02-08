using Microsoft.AspNet.Identity.EntityFramework;

namespace Antiguera.Administrador.Models.Auth
{
    public class UserModel : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}