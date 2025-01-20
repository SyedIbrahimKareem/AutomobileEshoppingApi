using EShoppingAutoMobiles.DataAccess;
using EShoppingAutoMobiles.IRepository;
using EShoppingAutoMobilesBusinessLibrary.UserModels;
using EShoppingAutoMobilesBusinessLibrary.Token;
using Microsoft.AspNetCore.Mvc;
using EShoppingAutoMobiles.Helpers;

namespace EShoppingAutoMobiles.Repository
{
    public class UserServiceRepository : IUserServiceRepository
    {
        private readonly DbContextClass _dbContextClass;
        public UserServiceRepository(DbContextClass dbContextClass)
        {
            _dbContextClass = dbContextClass;
        }
        public ResponseModel<UserRegisteration> AddUser(UserRegisteration userDetails)
        {
            ResponseModel<UserRegisteration> response = new ResponseModel<UserRegisteration>();
            string md5Password = MD5HashEncryption.GetMd5Hash(userDetails.password);
            userDetails.password = md5Password;
            var checkuserExist=_dbContextClass.UserRegisteration.ToList();
            var ischeckuserExist = checkuserExist.Where(x => x.userName == userDetails.userName);
            if (ischeckuserExist.Count() != 0)
            {
                response.IsSuccess = false;
                response.Message = "Username Already Exists";
                return response;
            }
            var result = _dbContextClass.UserRegisteration.Add(userDetails);
            _dbContextClass.SaveChanges();
            if (result.Entity == null) {

                response.Data = userDetails;
                response.Message = "Something went wrong!";
                response.IsSuccess = false;
                return response;
            }

            response.Data = userDetails;
            response.Message = "Successfully created Account!";
            response.IsSuccess = true;
            return response;
        }
        public UserRegisteration UpdateUser(UserRegisteration userDetails)
        {
            var result = _dbContextClass.UserRegisteration.Update(userDetails);
            _dbContextClass.SaveChanges();
            return result.Entity;
        }
        public bool DeleteUser(string userName)
        {
            var filteredData = _dbContextClass.UserRegisteration.Where(x => x.userName == userName).FirstOrDefault();
            var result = _dbContextClass.Remove(filteredData);
            _dbContextClass.SaveChanges();
            return result != null ? true : false;
        }
        public ResponseModel<UserRegisteration> GetUserDetails(string username)
        {
            ResponseModel<UserRegisteration> responseModel = new ResponseModel<UserRegisteration>();
            var data = _dbContextClass.UserRegisteration.Where(x => x.userName == username).FirstOrDefault();
            if (data !=null)
            {
                responseModel.Data = data;
                responseModel.IsSuccess = true;
                return responseModel;
            }
            else if (data== null)
            {
                responseModel.Message = "User Details of" + username +"Not found";
                responseModel.IsSuccess = false;
                return responseModel;
            }
            return responseModel;
        }

    }
}
