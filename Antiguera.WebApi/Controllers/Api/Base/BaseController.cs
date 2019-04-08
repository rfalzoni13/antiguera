using Antiguera.Infra.Cross.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Antiguera.WebApi.Controllers.Api.Base
{
    public class BaseController : ApiController
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return ControllerContext.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return ControllerContext.Request.GetOwinContext().Get<ApplicationRoleManager>();
            }

            set
            {
                _roleManager = value;
            }
        }

        [NonAction]
        protected static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
