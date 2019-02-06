namespace Antiguera.Administrador.Models
{
    public class ResponseLoginModel
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }
    }

    public class ResponseErrorLogin
    {
        public string error { get; set; }

        public string error_description { get; set; }
    }
}