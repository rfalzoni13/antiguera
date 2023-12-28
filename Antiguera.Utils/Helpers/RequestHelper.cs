using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Antiguera.Utils.Helpers
{
    public class RequestHelper
    {
        public static string GetAccessToken()
        {
            return HttpContext.Current.GetOwinContext().Authentication.User.Claims
                .FirstOrDefault(x => x.Type.Contains("AccessToken"))?.Value;
        }

        public static string GetRefreshToken()
        {
            return HttpContext.Current.GetOwinContext().Authentication.User.Claims
                .FirstOrDefault(x => x.Type.Contains("RefreshToken"))?.Value;
        }

        public static string GetTokenExpire()
        {
            return HttpContext.Current.GetOwinContext().Authentication.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Expired)?.Value;
        }
    }
}
