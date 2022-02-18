using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace Antiguera.Administrador.ViewModels
{
    public class ConfigViewModel
    {
        [DisplayName("Cor da barra principal")]
        public int IdCorHeader { get; set; }

        [DisplayName("Cor da barra lateral")]
        public int IdCorBarra { get; set; }
    }
}