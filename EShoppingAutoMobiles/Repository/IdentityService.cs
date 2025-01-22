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
        private readonly ILogger<IdentityService> _logger;
        public IdentityService(DbContextClass context,
            IOptions<ServiceConfiguration> settings,
            TokenValidationParameters tokenValidationParameters, RefreshTokenDBContext refreshTokenDBContext, ILogger<IdentityService> logger)
        {
            _context = context;
            _appSettings = settings.Value;
            _tokenValidationParameters = tokenValidationParameters;
            _refreshTokenDBContext = refreshTokenDBContext;
            _logger = logger;
        }


        public async Task<ResponseModel<TokenModel>> LoginAsync(LoginModel login)
        {
            ResponseModel<TokenModel> response = new ResponseModel<TokenModel>();
            try
            {
                string md5Password = MD5HashEncryption.GetMd5Hash(login.Password);
                UserRegisteration loginUser = _context.UserRegisteration.FirstOrDefault(c => c.userName == login.UserName && c.password == md5Password);
                _logger.LogInformation("Encryption of password");
                if (loginUser == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid Username And Password";
                    _logger.LogInformation("Invalid Username And Password");
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
                _logger.LogInformation("Returning of Refresh token from identity services");
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
                _logger.LogInformation("User roles fetching happening...");
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
                _logger.LogInformation("Returning of user roles");
                return lst;
            }
            catch (Exception)
            {
                return new List<RoleMaster>();
            }
        }

        public async Task<AuthenticationResult> AuthenticateAsync(UserRegisteration user)
        {
            // authentication successful so generate jwt token
            AuthenticationResult authenticationResult = new AuthenticationResult();
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                _logger.LogInformation("Generating the keys of secret");
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret);

                ClaimsIdentity Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserEmail",user.userEmail==null?"":user.userEmail),
                    new Claim("UserName",user.userName==null?"":user.userName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                });
                _logger.LogInformation("Adding the claims of user roles");
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
                _logger.LogInformation("Storing the token in database");
                await _refreshTokenDBContext.reFreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
                authenticationResult.RefreshToken = refreshToken.Token;
                authenticationResult.Success = true;
                _logger.LogInformation("returning of tokens to loginAsync Method");
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
        
