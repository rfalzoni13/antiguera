using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antiguera.Utils.Helpers
{
    public static class GuidHelper
    {
        public static Guid StringToGuid(string value)
        {
            Guid result = Guid.Empty;
            Guid.TryParse(value, out result);
            return result;
        }

        public static string GuidToString(Guid guid)
        {
            return guid.ToString();
        }
    }
}
