using DavidKuehn.Portfolio.Core.Extensions;
using DavidKuehn.Portfolio.WebApi.Models.IdentityProvider;
using DavidKuehn.Portfolio.WebApi.Services.IdentityProvider.Core_RBS_Tokens.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DavidKuehn.Portfolio.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ILogger<SecurityController> _logger;
        private readonly SecurityServices _securityServices;
        public SecurityController(ILogger<SecurityController> logger, SecurityServices securityServices)
        {
            _logger = logger;
            _securityServices = securityServices;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromBody]UserCredentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.UserName) || string.IsNullOrEmpty(credentials.Password))
            {
                return BadRequest("User name and password are required.");
            }

            var user = new LoginUser
            {
                UserName = credentials.UserName,
                Password = credentials.Password.ToSecureString()
            };

            try
            {
                var securityResponse = await _securityServices.AuthenticateUser(user);

                switch (securityResponse.StatusCode)
                {
                    case 200:
                        return Ok(securityResponse);
                    case 403:
                        return Forbid();
                    case 404:
                        return NotFound(securityResponse);
                    case 500:
                        return StatusCode(500, securityResponse);
                    case 400:
                    default:
                        return BadRequest(securityResponse);
                }
            }
            finally
            {
                user.Password?.Dispose();
            }
        }
    }
}
