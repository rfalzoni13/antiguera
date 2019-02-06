using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace Antiguera.Administrador.Models
{
    public class ConfigModel
    {
        [DisplayName("Cor da barra principal")]
        public int IdCorHeader { get; set; }

        [DisplayName("Cor da barra lateral")]
        public int IdCorBarra { get; set; }
    }
}