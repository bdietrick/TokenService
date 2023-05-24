using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Validations.Rules;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using TokenService.Models;

namespace TokenService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public TokenController() {}

        /// <summary>
        /// Returns a JWT if valid credentials are provided.
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns>JWT (string)</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/TokenService/GetToken
        ///     {
        ///        "username": "user",
        ///        "password": "123"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">JWT returned</response>
        /// <response code="401">Credentials failed</response>
        [HttpPost("GetToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetTokenAsync([FromBody] GetTokenRequestModel requestModel)
        {   
            if(!requestModel.Username.Equals("user") || !requestModel.Password.Equals("123")) 
            {
                return Forbid();
            }

            var token = await GenerateTokenAsync();

            return Ok(token);
        }

        private async Task<string> GenerateTokenAsync()
        {
            var secret = "MyVerySuperSecureSecretSharedKey";
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            var issuer = "http://www.uc.edu/IT3047C";
            var audience = "WebServerApplicationDevelopment";

            /***************************************************
             *  Uncomment code below to use custom claims
             *  ************************************************/
            //var claims = new Dictionary<string,object>();
            //claims.Add("claim-name", "claim-value");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(secretKey,
                                                            SecurityAlgorithms.HmacSha256Signature),
                //Claims = claims
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return await Task.FromResult(token);

        }
    }
}
