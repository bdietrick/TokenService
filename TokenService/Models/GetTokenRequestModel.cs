using System.Security.Cryptography;

namespace TokenService.Models
{
    public class GetTokenRequestModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
