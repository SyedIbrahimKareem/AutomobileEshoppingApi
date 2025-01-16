using EShoppingAutoMobilesBusinessLibrary.Token;
using EShoppingAutoMobilesBusinessLibrary.UserModels;

namespace EShoppingAutoMobiles.IRepository
{
    public interface IIdentityService
    {
        Task<ResponseModel<TokenModel>> LoginAsync(LoginModel login);
    }
}
