using EShoppingAutoMobiles.IRepository;
using EShoppingAutoMobilesBusinessLibrary.UserModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using EShoppingAutoMobilesBusinessLibrary.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EShoppingAutoMobiles.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServiceRepository _userService;
        public UserController(IUserServiceRepository userService)
        {
            _userService = userService;
        }
        //Create user information or profile
        [HttpPost]
        
        public ActionResult<ResponseModel<UserMaster>> AddUserDetails([FromBody] UserRegisteration userDetails)
        {
            
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
