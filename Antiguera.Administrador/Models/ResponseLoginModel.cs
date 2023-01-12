using Newtonsoft.Json;
using System;

namespace Antiguera.Administrador.Models
{
    public class ResponseLoginModel
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public string refresh_token { get; set; }

        public int expires_in { get; set; }
    }

    public class ResponseErrorLogin
    {
        public string error { get; set; }

        public string error_description { get; set; }
    }

    public class TokenModel
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(".expires")]
        public DateTime? Expire { get; set; }

        [JsonProperty(".issued")]
        public DateTime? Issue { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("roleId")]
        public string RoleId { get; set; }

        public bool? Novo { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}