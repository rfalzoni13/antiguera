using Antiguera.Administrador.Client;
using Antiguera.Administrador.Helpers;
using Antiguera.Administrador.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    public class AccountController : Controller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly AccountClient _accountClient;

        public AccountController(AccountClient accountClient)
        {
            _accountClient = accountClient;
        }

        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            try
            {
                var model = new LoginModel();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                throw;
            }
        }

        // POST: Account
        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            try
            {
                _accountClient.Login(model, Request);

                return RedirectToAction("Index", "Home");
            }
            catch(ApplicationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
            catch(Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                throw;
            }
        }

        //POST: Logout
        [HttpPost]
        public async Task<ActionResult> LogOut()
        {
            try
            {
                await _accountClient.Logout(Request);

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                throw;
            }
        }

        //
        // POST: /Account/LoginExterno
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginExterno(string provider, string returnUrl)
        {
            var url = $"{UrlConfiguration.AccountExternalLogin}?provider={provider}";

            await _accountClient.LoginExterno(url);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/EnviarCodigo
        [AllowAnonymous]
        public async Task<ActionResult> EnviarCodigo(string returnUrl, bool rememberMe)
        {
            try
            {
                var userFactors = await _accountClient.ObterAutenticacaoDoisFatores();

                var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();

                return View(new SendCodeModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                throw;
            }
        }

        // POST: /Account/EnviarCodigo
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnviarCodigo(SendCodeModel model)
        {
            try
            {
                await _accountClient.EnviarCodigoDoisFatores(model);

                return RedirectToAction("VerificarCodigo", new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                throw;
            }
        }

        // GET: /Account/VerificarCodigo
        [AllowAnonymous]
        public ActionResult VerificarCodigo(string provider, string returnUrl, bool rememberMe)
        {
            try
            {
                if (!Request.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                    throw new Exception("Não autorizado!");

                return View(new VerifyCodeModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                throw;
            }
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerificarCodigo(VerifyCodeModel model)
        {
            var result = await _accountClient.VerificarCodigoDoisFatores(model);

            return RedirectToLocal(result.ReturnUrl);
        }

        // GET: /Account/Registrar
        [HttpGet]
        public ActionResult Registrar()
        {
            try
            {
                var model = new RegistrarModel();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                throw;
            }
        }

        // POST: /Account/Registrar
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registrar(RegistrarModel model)
        {
            _logger.Debug("Iniciando");
            try
            {
                if (!model.AcceptTerms)
                {
                    _logger.Error("Você deve aceitar os termos de uso!");
                    ModelState.AddModelError(string.Empty, "Você deve aceitar os termos de uso!");
                }

                await _accountClient.Registrar(model);

                return RedirectToAction("Account", "Login");

                // Se chegamos até aqui e houver alguma falha, exiba novamente o formulário
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                throw;
            }
        }

        //GET: /Account/EsqueciMinhaSenha
        public ActionResult EsqueciMinhaSenha()
        {
            var model = new ForgotPasswordModel();

            return View(model);
        }

        //
        // POST: /Account/EsqueciMinhaSenha
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EsqueciMinhaSenha(ForgotPasswordModel model)
        {
            try
            {
                model.CallBackUrl = Url.Action("RecuperarSenha", "Account", new { userId = "{0}", code = "{1}" }, protocol: Request.Url.Scheme);

                await _accountClient.EsqueciMinhaSenha(model);

                return View(model);
            }
            catch(ApplicationException ex)
            {
                _logger.Error(ex, ex.Message);
                return RedirectToAction("ConfirmacaoRecuperacaoSenha", "Account");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                throw;
            }
        }

        //
        // GET: /Account/ConfirmacaoRecuperacaoSenha
        [AllowAnonymous]
        public ActionResult ConfirmacaoRecuperacaoSenha()
        {
            return View();
        }

        //
        // GET: /Account/RecuperarSenha
        [AllowAnonymous]
        public ActionResult RecuperarSenha(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/RecuperarSenha
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecuperarSenha(ResetPasswordModel model)
        {
            try
            {
                var result = await _accountClient.RecuperarSenha(model);

                if (result.Succeeded)
                {
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ConfirmacaoRecuperacaoSenha", "Account");
                }
            }
            catch (ApplicationException ex)
            {
                _logger.Error(ex, ex.Message);
                return RedirectToAction("ConfirmacaoRecuperacaoSenha", "Account");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                throw;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}