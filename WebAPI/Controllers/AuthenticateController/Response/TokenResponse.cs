using System;

namespace WebAPI.Controllers.AuthenticateController.Response
{
    public class TokenResponse
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}
