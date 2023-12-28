using Antiguera.Administrador.Clients;
using Antiguera.Utils.Helpers;
using Microsoft.Owin;
using System;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Antiguera.Administrador.Filters
{
    public class CustomActionAttribute : ActionFilterAttribute
    {
        private readonly AccountClient _accountClient;

        public CustomActionAttribute(AccountClient accountClient)
        {
            _accountClient = accountClient;
        }

        public override async void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                return;

            DateTime dateLimit = Convert.ToDateTime(RequestHelper.GetTokenExpire());

            if(dateLimit >= DateTime.Now)
            {
                await _accountClient.RefreshToken();
            }
            

            base.OnActionExecuting(actionContext);
        }

        protected virtual IOwinContext GetOwinContext()
        {
            return HttpContext.Current.GetOwinContext();
        }
    }
}