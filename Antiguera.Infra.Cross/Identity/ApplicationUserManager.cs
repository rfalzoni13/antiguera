﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Antiguera.Infra.Cross.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            :base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<ApplicationDbContext>();
            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(appDbContext));
            
            return appUserManager;
        }
    }
}
