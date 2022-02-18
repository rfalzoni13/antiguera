using Antiguera.Administrador.Controllers.Base;
using Antiguera.Administrador.Utils;
using Antiguera.Administrador.ViewModels;
using Antiguera.Dominio.Interfaces.Servicos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Antiguera.Administrador.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(IAcessoServico acessoServico, IEmuladorServico emuladorServico,
            IHistoricoServico historicoServico, IJogoServico jogoServico,
            IProgramaServico programaServico, IRomServico romServico,
            IUsuarioServico usuarioServico)
            :base(acessoServico, emuladorServico, historicoServico, jogoServico, programaServico,
                 romServico, usuarioServico)
        {
        }

        public ActionResult Index()
        {
            try
            {
                return View();
            }

            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: ", ex);
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            try
            {
                var model = new LoginViewModel();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex);
                return View("Error");
            }
        }

        //POST: /Home/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            _logger.Debug("Iniciando");
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // Isso não conta falhas de login em relação ao bloqueio de conta
                // Para permitir que falhas de senha acionem o bloqueio da conta, altere para shouldLockout: true
                _logger.Info("Fazendo login na base Identity");

                var user = await UserManager().FindAsync(model.UserName, model.Password);
                if(user == null)
                {
                    user = await UserManager().FindByEmailAsync(model.UserName);
                    model.UserName = !string.IsNullOrEmpty(user?.UserName) ? user?.UserName : model.UserName;
                }

                var result = await SignInManager().PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        _logger.Info("Obtendo Token de acesso!");

                        string id = SignInManager().AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();

                        var usuario = BuscarUsuarioPorIdentityId(user.Id);
                        if(usuario != null)
                        {
                            await SignInManager().UserManager.AddClaimAsync(id, new Claim("Nome", usuario.Nome));
                            await SignInManager().UserManager.AddClaimAsync(id, new Claim("UsuarioId", usuario.Id.ToString()));
                            if(!string.IsNullOrEmpty(usuario.Foto))
                            {
                                await SignInManager().UserManager.AddClaimAsync(id, new Claim("Foto", usuario.Foto));
                            }
                            var roles = await SignInManager().UserManager.GetRolesAsync(user.Id);
                            if (roles != null && roles.Count > 0)
                            {
                                foreach (var role in roles)
                                {
                                    await SignInManager().UserManager.AddClaimAsync(id, new Claim(ClaimTypes.Role, role));
                                }
                            }
                        }
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        _logger.Error("Conta bloqueada");
                        ModelState.AddModelError(string.Empty, "Conta bloqueada, entre em contato com o administrador!");
                        return View(model);
                    case SignInStatus.RequiresVerification:
                        _logger.Info("Enviando para página de confirmação");
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        _logger.Error("Login ou senha incorretos.");
                        ModelState.AddModelError(string.Empty, "Login ou senha incorretos.");
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Ocorreu um erro: " + ex);
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                ModelState.AddModelError(string.Empty, "Ocorreu um erro, consulte o arquivo de log para mais detalhes!");
                return View(model);
            }
        }

        // GET: /Home/VerificarCodigo
        [AllowAnonymous]
        public async Task<ActionResult> VerificarCodigo(string provider, string returnUrl, bool rememberMe)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index");
                }

                // Exija que o usuário efetue login via nome de usuário/senha ou login externo
                if (!await SignInManager().HasBeenVerifiedAsync())
                {
                    return View("Error");
                }
                return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
            }
            catch(Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        // POST: /Home/VerificarCodigo
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerificarCodigo(VerifyCodeViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // O código a seguir protege de ataques de força bruta em relação aos códigos de dois fatores. 
                // Se um usuário inserir códigos incorretos para uma quantidade especificada de tempo, então a conta de usuário 
                // será bloqueado por um período especificado de tempo. 
                // Você pode configurar os ajustes de bloqueio da conta em IdentityConfig
                var result = await SignInManager().TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(model.ReturnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError(string.Empty, "Código inválido.");
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        // GET: /Home/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            try
            {
                if (userId == null || code == null)
                {
                    return View("Error");
                }
                var result = await userManager.ConfirmEmailAsync(userId, code);
                return View(result.Succeeded ? "ConfirmEmail" : "Error");
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        //
        // GET: /Home/EnviarCodigo
        [AllowAnonymous]
        public async Task<ActionResult> EnviarCodigo(string returnUrl, bool rememberMe)
        {
            try
            {
                var userId = await SignInManager().GetVerifiedUserIdAsync();
                if (userId == null)
                {
                    return View("Error");
                }
                var userFactors = await userManager.GetValidTwoFactorProvidersAsync(userId);
                var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
                return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        //
        // POST: /Home/EnviarCodigo
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnviarCodigo(SendCodeViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                // Gerar o token e enviá-lo
                if (!await SignInManager().SendTwoFactorCodeAsync(model.SelectedProvider))
                {
                    return View("Error");
                }
                return RedirectToAction("VerificarCodigo", new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Erro fatal!");
                return View("Error");
            }
        }

        // POST: /Home/Registrar
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Registrar(RegistrarViewModel model)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    _logger.Debug("Iniciando");
        //    try
        //    {
        //        if (!model.RememberMe)
        //        {
        //            _logger.Error("Você deve aceitar os termos de uso!");
        //            ModelState.AddModelError(string.Empty, "Você deve aceitar os termos de uso!");
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            var user = new ApplicationUser
        //            {
        //                UserName = model.Email,
        //                Email = model.Email,
        //                EmailConfirmed = true,
        //                FirstName = model.Nome.Split(' ').FirstOrDefault(),
        //                LastName = model.Nome.Split(' ').LastOrDefault()
        //            };

        //            var result = await userManager.CreateAsync(user, model.Password);
        //            if (result.Succeeded)
        //            {
        //                await SignInManager().SignInAsync(user, isPersistent: false, rememberBrowser: false);

        //                // Para obter mais informações sobre como habilitar a confirmação da conta e redefinição de senha, visite https://go.microsoft.com/fwlink/?LinkID=320771
        //                // Enviar um email com este link
        //                // string code = await UserManager().GenerateEmailConfirmationTokenAsync(user.Id);
        //                // var callbackUrl = Url.Action("ConfirmEmail", "Home", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //                // await UserManager().SendEmailAsync(user.Id, "Confirmar sua conta", "Confirme sua conta clicando <a href=\"" + callbackUrl + "\">aqui</a>");

        //                return RedirectToAction("Index", "Home");
        //            }
        //            AddErrors(result);
        //        }

        //        // Se chegamos até aqui e houver alguma falha, exiba novamente o formulário
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Fatal("Ocorreu um erro: " + ex.Message);
        //        var user = await userManager.FindByEmailAsync(model.Email);
        //        if (user != null)
        //        {
        //            await userManager.DeleteAsync(user);
        //        }
        //        ModelState.AddModelError(string.Empty, ex.Message);
        //        return View(model);
        //    }
        //}

        //
        // GET: /Home/RedirectCallBack
        [AllowAnonymous]
        public async Task<ActionResult> RedirectCallBack(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Faça logon do usuário com este provedor de logon externo se o usuário já tiver um logon
            var result = await SignInManager().ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return RedirectToAction("Login");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Se o usuário não tiver uma conta, solicite que o usuário crie uma conta
                    ViewBag.ReturnUrl = returnUrl;

                    return View("ConfirmacaoConta", new ExternalLoginConfirmationViewModel
                    {
                        Email = loginInfo.Email,
                        UserName = loginInfo.DefaultUserName,
                        FullName = loginInfo.ExternalIdentity.Name,
                        IdProvider = loginInfo.Login.LoginProvider == "Facebook" ? "fb_" + loginInfo.Login.ProviderKey : "gg_" + loginInfo.Login.ProviderKey,
                        ProviderName = loginInfo.Login.LoginProvider
                    });
            }
        }

        //
        // POST: /Home/ConfirmacaoConta
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmacaoConta(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            _logger.Debug("Iniciando");

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            try
            {
                if (!model.AcceptTerms)
                {
                    _logger.Error("Você deve aceitar os termos de uso!");
                    ModelState.AddModelError(string.Empty, "Você deve aceitar os termos de uso!");
                }
                if (User.Identity.IsAuthenticated)
                {
                    _logger.Info("Usuário já autenticado!");
                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {
                    _logger.Info("Obtendo as informações sobre o usuário do provedor de logon externo");
                    // Obter as informações sobre o usuário do provedor de logon externo
                    var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                    if (info == null)
                    {
                        _logger.Error("Ocorreu uma falha ao realizar o login externo");
                        return View("FalhaLoginExterno");
                    }

                    _logger.Info("Criando o usuário na base Identity");
                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        FirstName = model.FullName.Split(' ').FirstOrDefault(),
                        LastName = model.FullName.Split(' ').LastOrDefault()
                    };

                    var result = await userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        _logger.Info("Adicinando informações do login de provedor");
                        result = await userManager.AddLoginAsync(user.Id, info.Login);
                        if (result.Succeeded)
                        {
                            await SignInManager().SignInAsync(user, isPersistent: false, rememberBrowser: false);

                            _logger.Info("Sucesso!");
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    AddErrors(result);
                }

                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Fatal("Ocorreu um erro: " + ex.Message);
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    await userManager.DeleteAsync(user);
                }
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        //
        // POST: /Home/LogOff
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }

        //
        // GET: /Home/FalhaLoginExterno
        [AllowAnonymous]
        public ActionResult FalhaLoginExterno()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (userManager != null)
                {
                    userManager.Dispose();
                    userManager = null;
                }

                if (SignInManager() != null)
                {
                    SignInManager().Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}