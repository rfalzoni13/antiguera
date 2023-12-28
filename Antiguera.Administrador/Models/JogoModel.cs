using Antiguera.Utils.Helpers;
using Newtonsoft.Json;
using System;
using System.Web;

namespace Antiguera.Administrador.Models
{
    public class JogoModel
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public DateTime Lancamento { get; set; }

        public string Publisher { get; set; }

        public string Developer { get; set; }

        public string Genero { get; set; }

        public string Arquivo { get; set; }

        public string Capa { get; set; }

        public string Jogo64
        {
            get
            {
                if(ArquivoJogo != null)
                    return FileHelper.ConvertStreamToBase64String(ArquivoJogo);
                
                return null;
            }
            set { }
        }

        public string Capa64
        {
            get
            {
                if(ArquivoCapa != null)
                    return FileHelper.ConvertStreamToBase64String(ArquivoCapa);

                return null;
            }
            set { }
        }

        public string Tipo { get; set; }

        public string Plataforma { get; set; }

        public bool? Novo { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        [JsonIgnore]
        public HttpPostedFileBase ArquivoJogo { get; set; }
        [JsonIgnore]
        public HttpPostedFileBase ArquivoCapa { get; set; }
    }
}