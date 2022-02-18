using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Antiguera.Administrador.ViewModels
{
    public class VerifyCodeViewModel
    {
        public string Provider { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
        public string Code { get; set; }
        public bool RememberBrowser { get; set; }
    }

    public class SendCodeViewModel
    {
        public List<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
        public string SelectedProvider { get; set; }
    }
    public class ExternalLoginConfirmationViewModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string IdProvider { get; set; }
        public string ProviderName { get; set; }
        public bool AcceptTerms { get; set; }
    }

    public class RegistrarViewModel
    {
        public bool RememberMe { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Password { get; set; }
    }

}