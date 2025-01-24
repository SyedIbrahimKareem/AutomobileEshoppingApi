using EShoppingBusinessLibrary.Token;
using EShoppingBusinessLibrary.UserModels;

namespace EShoppingAPI.IRepository
{
    public interface IIdentityService
    {
        Task<ResponseModel<TokenModel>> LoginAsync(LoginModel login);
    }
}
