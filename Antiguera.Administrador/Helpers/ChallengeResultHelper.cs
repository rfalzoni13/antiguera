using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.Helpers
{
    public class ChallengeResultHelper : HttpUnauthorizedResult
    {
        // Usado para proteção XSRF ao adicionar logons externos
        private const string XsrfKey = "XsrfId";

        public ChallengeResultHelper(string provider, string redirectUri)
            : this(provider, redirectUri, null)
        {
        }

        public ChallengeResultHelper(string provider, string redirectUri, string userId)
        {
            LoginProvider = provider;
            RedirectUri = redirectUri;
            UserId = userId;
        }

        public string LoginProvider { get; set; }
        public string RedirectUri { get; set; }
        public string UserId { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            if (UserId != null)
            {
                properties.Dictionary[XsrfKey] = UserId;
            }
            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        }

    }
}