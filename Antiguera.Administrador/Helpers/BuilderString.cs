using System.Linq;

namespace Antiguera.Administrador.Helpers
{
    public static class BuilderString
    {
        public static string SetDashboardName(string name)
            => $"{name.Split(' ').FirstOrDefault()} {name.Split(' ').LastOrDefault()}";
    }
}