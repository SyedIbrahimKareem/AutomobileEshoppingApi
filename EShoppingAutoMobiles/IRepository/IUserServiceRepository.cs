using EShoppingAutoMobilesBusinessLibrary.Token;
using EShoppingAutoMobilesBusinessLibrary.UserModels;

namespace EShoppingAutoMobiles.IRepository
{
    public interface IUserServiceRepository
    {
        public ResponseModel<UserMaster> AddUser(UserMaster userDetails);
        public UserMaster UpdateUser(UserMaster userDetails);
        public bool DeleteUser(string userName);
        public ResponseModel<UserMaster> GetUserDetails(string username);
    }
}
