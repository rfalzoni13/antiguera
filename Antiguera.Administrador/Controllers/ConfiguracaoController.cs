using Antiguera.Administrador.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class ConfiguracaoController : Controller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        // GET: Configuracao
        public ActionResult Index()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            try
            {
                var model = new ConfigModel();

                var corHeader = ConfigurationManager.AppSettings["CorHeader"];

                switch (corHeader)
                {
                    case "skin-blue":
                        model.IdCorHeader = 1;
                        model.IdCorBarra = 2;
                        break;

                    case "skin-blue-light":
                        model.IdCorHeader = 1;
                        model.IdCorBarra = 1;
                        break;

                    case "skin-yellow":
                        model.IdCorHeader = 2;
                        model.IdCorBarra = 2;
                        break;

                    case "skin-yellow-light":
                        model.IdCorHeader = 2;
                        model.IdCorBarra = 1;
                        break;

                    case "skin-red":
                        model.IdCorHeader = 3;
                        model.IdCorBarra = 2;
                        break;

                    case "skin-red-light":
                        model.IdCorHeader = 3;
                        model.IdCorBarra = 1;
                        break;

                    case "skin-purple":
                        model.IdCorHeader = 4;
                        model.IdCorBarra = 2;
                        break;

                    case "skin-purple-light":
                        model.IdCorHeader = 4;
                        model.IdCorBarra = 1;
                        break;

                    case "skin-green":
                        model.IdCorHeader = 5;
                        model.IdCorBarra = 2;
                        break;

                    case "skin-green-light":
                        model.IdCorHeader = 5;
                        model.IdCorBarra = 1;
                        break;

                    case "skin-black":
                        model.IdCorHeader = 6;
                        model.IdCorBarra = 2;
                        break;

                    case "skin-black-light":
                        model.IdCorHeader = 6;
                        model.IdCorBarra = 1;
                        break;

                    default:
                        model.IdCorHeader = 1;
                        model.IdCorBarra = 2;
                        break;
                }
                
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: ", ex);
                throw;
            }
        }

        // POST: Configuracao/Salvar
        [HttpPost]
        public JsonResult Salvar(ConfigModel model)
        {
            List<string> errorsList = new List<string>();

            try
            {
                var corHeader = string.Empty;

                if (model.IdCorBarra > 0 && model.IdCorHeader > 0)
                {
                    switch (model.IdCorHeader)
                    {
                        case 1:
                            corHeader = "skin-blue";
                            break;

                        case 2:
                            corHeader = "skin-yellow";
                            break;

                        case 3:
                            corHeader = "skin-red";
                            break;

                        case 4:
                            corHeader = "skin-purple";
                            break;

                        case 5:
                            corHeader = "skin-green";
                            break;

                        case 6:
                            corHeader = "skin-black";
                            break;

                        default:
                            corHeader = "skin-blue";
                            break;
                    }

                    var config = WebConfigurationManager.OpenWebConfiguration("~");

                    if (model.IdCorBarra == 1)
                    {
                        corHeader += "-light";
                    }

                    config.AppSettings.Settings["CorHeader"].Value = corHeader;

                    if (corHeader == "skin-black" || corHeader == "skin-black-light")
                    {
                        config.AppSettings.Settings["TipoLogo"].Value = "LogoAntiguera2.png";
                        config.AppSettings.Settings["TipoLogoMini"].Value = "antiguera-mini-logo-2.fw.png";
                    }
                    else
                    {
                        config.AppSettings.Settings["TipoLogo"].Value = "LogoAntiguera.png";
                        config.AppSettings.Settings["TipoLogoMini"].Value = "antiguera-mini-logo.fw.png";
                    }

                    config.Save();
                }
                else
                {
                    errorsList.Add("Parâmetros incorretos!");
                    return Json(new { success = true, errors = errorsList });
                }
                    
                return Json(new { success = true, mensagem = "Configurações salvas com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                errorsList.Add(ex.Message);
#if !DEBUG
                errorsList.Add("Ocorreu um erro, verifique o arquivo de log e tente novamente!");
#endif

                return Json(new { success = false, errors = errorsList });
            }
        }
    }
}