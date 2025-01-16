using EShoppingAutoMobiles.DataAccess;
using EShoppingAutoMobiles.IRepository;
using EShoppingAutoMobilesBusinessLibrary.UserModels;
using EShoppingAutoMobilesBusinessLibrary.Token;
using Microsoft.AspNetCore.Mvc;

namespace EShoppingAutoMobiles.Repository
{
    public class UserServiceRepository : IUserServiceRepository
    {
        private readonly DbContextClass _dbContextClass;
        public UserServiceRepository(DbContextClass dbContextClass)
        {
            _dbContextClass = dbContextClass;
        }
        public ResponseModel<UserMaster> AddUser(UserMaster userDetails)
        {
            ResponseModel<UserMaster> response = new ResponseModel<UserMaster>();
            userDetails.CreatedOn= DateTime.Now.ToString();
            userDetails.UpdatedOn = DateTime.Now.ToString();
            var checkuserExist=_dbContextClass.UserMaster.ToList();
            var ischeckuserExist = checkuserExist.Where(x => x.UserName == userDetails.UserName);
            if (ischeckuserExist.Count() != 0)
            {
                response.IsSuccess = false;
                response.Message = "Username Already Exists";
                return response;
            }
            var result = _dbContextClass.UserMaster.Add(userDetails);
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
        public UserMaster UpdateUser(UserMaster userDetails)
        {
            var result = _dbContextClass.UserMaster.Update(userDetails);
            _dbContextClass.SaveChanges();
            return result.Entity;
        }
        public bool DeleteUser(string userName)
        {
            var filteredData = _dbContextClass.UserMaster.Where(x => x.UserName == userName).FirstOrDefault();
            var result = _dbContextClass.Remove(filteredData);
            _dbContextClass.SaveChanges();
            return result != null ? true : false;
        }
        public ResponseModel<UserMaster> GetUserDetails(string username)
        {
            ResponseModel<UserMaster> responseModel = new ResponseModel<UserMaster>();
            var data = _dbContextClass.UserMaster.Where(x => x.UserName == username).FirstOrDefault();
            if (data !=null)
            {
                responseModel.Data = data;
                responseModel.Message = "User Details of " + data.FirstName + " " + data.LastName;
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
