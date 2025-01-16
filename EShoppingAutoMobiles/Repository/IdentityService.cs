using EShoppingAutoMobilesBusinessLibrary.Token;
using EShoppingAutoMobilesBusinessLibrary.UserModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EShoppingAutoMobilesBusinessLibrary.Token;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using EShoppingAutoMobiles.DataAccess;
using EShoppingAutoMobiles.Helpers;
using EShoppingAutoMobiles.IRepository;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;


namespace EShoppingAutoMobiles.Repository
{
    public class IdentityService : IIdentityService
    {
        private readonly DbContextClass _context;
        private readonly RefreshTokenDBContext _refreshTokenDBContext;
        private readonly ServiceConfiguration _appSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public IdentityService(DbContextClass context,
            IOptions<ServiceConfiguration> settings,
            TokenValidationParameters tokenValidationParameters, RefreshTokenDBContext refreshTokenDBContext)
        {
            _context = context;
            _appSettings = settings.Value;
            _tokenValidationParameters = tokenValidationParameters;
            _refreshTokenDBContext = refreshTokenDBContext;
        }


        public async Task<ResponseModel<TokenModel>> LoginAsync(LoginModel login)
        {
            ResponseModel<TokenModel> response = new ResponseModel<TokenModel>();
            try
            {
                string md5Password = MD5HashEncryption.GetMd5Hash(login.Password);
                UserMaster loginUser = _context.UserMaster.FirstOrDefault(c => c.UserName == login.UserName && c.Password == md5Password);

                if (loginUser == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid Username And Password";
                    return response;
                }

                AuthenticationResult authenticationResult = await AuthenticateAsync(loginUser);
                if (authenticationResult.Error != "" && authenticationResult.Success)
                {
                    response.Data = new TokenModel() { Token = authenticationResult.Token, RefreshToken = authenticationResult.RefreshToken };
                }
                else
                {
                    response.Message = authenticationResult.Error;
                    response.IsSuccess = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<RoleMaster> GetUserRole(long UserId)
        {
            try
            {
                List<RoleMaster> lst= new List<RoleMaster> ();
                DataTable table = new DataTable("Table");
                using (SqlConnection connection = new SqlConnection("server=I25002; database=ElectronicShopping; Integrated Security=true; Encrypt=false"))
                {

                    //Create the SqlCommand object by passing the stored procedure name and connection object as parameters
                    SqlCommand cmd = new SqlCommand("GetUserRolesInfo", connection)
                    {
                        //Specify the command type as Stored Procedure
                        CommandType = CommandType.StoredProcedure

                    };
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    //Open the Connection
                    connection.Open();
                    //Execute the command i.e. Executing the Stored Procedure using ExecuteReader method
                    //SqlDataReader requires an active and open connection
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(table);
                    foreach (DataRow row in table.Rows)
                    {
                        RoleMaster instance = new RoleMaster();
                        instance.RoleId = Convert.ToInt32(row["RoleId"]);
                        instance.RoleName = row["RoleName"].ToString();
                        lst.Add(instance);
                    }

                }
                return lst;
            }
            catch (Exception)
            {
                return new List<RoleMaster>();
            }
        }

        public async Task<AuthenticationResult> AuthenticateAsync(UserMaster user)
        {
            // authentication successful so generate jwt token
            AuthenticationResult authenticationResult = new AuthenticationResult();
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret);

                ClaimsIdentity Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName",user.LastName),
                    new Claim("UserEmail",user.UserEmail==null?"":user.UserEmail),
                    new Claim("UserName",user.UserName==null?"":user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                });
                foreach (var item in GetUserRole(user.UserId))
                {
                    Subject.AddClaim(new Claim(ClaimTypes.Role, item.RoleName));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = Subject,
                    Expires = DateTime.UtcNow.Add(_appSettings.JwtSettings.TokenLifetime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                authenticationResult.Token = tokenHandler.WriteToken(token);
                var refreshToken = new ReFreshToken
                {
                    Token = Guid.NewGuid().ToString(),
                    JwtId = token.Id,
                    UserId = user.UserId,
                    CreationDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddMonths(6)
                };
                await _refreshTokenDBContext.reFreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
                authenticationResult.RefreshToken = refreshToken.Token;
                authenticationResult.Success = true;
                return authenticationResult;
            }
            catch (Exception ex)
            {
                authenticationResult.Error = ex.Message.ToString();
                authenticationResult.Success = false;
                return authenticationResult;
            }

        }
    }
}
        
