using System.Configuration;

namespace Antiguera.Administrador.Helpers
{
    public class UrlConfiguration
    {

        #region Url Principal
        public string Api => ConfigurationManager.AppSettings["AmbienteApi"] == "true" ?
            ConfigurationManager.AppSettings["UrlApiProd"] :
            ConfigurationManager.AppSettings["UrlApiDev"];
        #endregion

        #region UrlFiles
        public string Profile = "~/Content/Images/Profile/";

        public string Emulador = "~/Content/Consoles/Emuladores/";

        public string Rom = "~/Content/Consoles/Roms/";

        public string BoxArt = "/Content/Images/BoxArt/";

        public string Jogo = "~/Content/Games/";

        public string Programa = "~/Content/Programas";
        #endregion
    }
}