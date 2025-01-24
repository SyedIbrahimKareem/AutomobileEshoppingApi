using EShoppingAPI.IRepository;
using EShoppingBusinessLibrary.UserModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using EShoppingBusinessLibrary.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EShoppingAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServiceRepository _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserServiceRepository userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        //Create user information or profile
        [HttpPost]
        
        public ActionResult<ResponseModel<UserMaster>> AddUserDetails([FromBody] UserRegisteration userDetails)
        {
            _logger.LogInformation("Registering the User Details");
            var result = _userService.AddUser(userDetails);
            return Ok(result);
        }
        // Get user details or information
        [HttpGet]
        public ActionResult<ResponseModel<UserRegisteration>> GetUserDetails(string userName)
        {
            var result = _userService.GetUserDetails(userName);
            return result;
        }
        // Remove user account or profile
        [HttpDelete]
        public ActionResult<bool> DeleteUser(string userName)
        {
            var result = _userService.DeleteUser(userName);
            return result;
        }
    }
}
