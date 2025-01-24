using EShoppingAPI.IRepository;
using EShoppingBusinessLibrary.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace EShoppingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IIdentityService identityService, ILogger<AuthController> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel loginModel)
        {
            try
            {

                _logger.LogInformation("Creating Refersh token at" + "" + DateTime.Now.ToString());
                var result = await _identityService.LoginAsync(loginModel);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                _logger.LogWarning("Error thrown message :", ex.Message);
                return BadRequest(ex.Message);
            }
            
        }
    }
}
