using EShoppingAutoMobilesBusinessLibrary.Token;
using EShoppingAutoMobilesBusinessLibrary.UserModels;

namespace EShoppingAutoMobiles.IRepository
{
    public interface IUserServiceRepository
    {
        public ResponseModel<UserRegisteration> AddUser(UserRegisteration userDetails);
        public UserRegisteration UpdateUser(UserRegisteration userDetails);
        public bool DeleteUser(string userName);
        public ResponseModel<UserRegisteration> GetUserDetails(string username);
    }
}
