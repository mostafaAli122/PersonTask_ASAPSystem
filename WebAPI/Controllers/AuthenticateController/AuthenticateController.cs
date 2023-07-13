using DataAccessEF.Services.Authentication;
using DataAccessEF.Services.LogFile;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers.AuthenticateController.Request;
using WebAPI.Controllers.AuthenticateController.Response;
using Domain.Shared;

namespace WebAPI.Controllers.AuthenticateController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : TaskBaseController
    {
        private readonly AuthConfiguration AuthConfig;
        private readonly IUnitOfWork UOW;
        private readonly UserManager<IdentityUser> userManager;


        public AuthenticateController(UserManager<IdentityUser> userManager,AuthConfiguration _AuthConfig, LogFileService logger, IUnitOfWork _uow):base(logger)
        {
            this.userManager = userManager;
            this.AuthConfig = _AuthConfig;
            this.UOW = _uow;
        }
        [HttpPost]
        [Route("login")]
        public async Task<DescriptiveResponse<TokenResponse>> Login([FromBody] LoginRequest model)
        {
            return await this.TryCatchLogAscync(async () =>
            {

                var users = this.UOW.Users.Find(x => x.UserName == model.Username)
                    ?? throw new TaskException(TaskValidationKeysEnum.WrongUserDetails);

                var user = users.FirstOrDefault();
                Func<Task<(long, string, DateTime)>> generateTokenFun = async () =>
                {
                    var userRoles = await userManager.GetRolesAsync(user);

                    var tokenId = UOW.Users.GetUserGeneratedTokenKey();

                    var authClaims = new List<Claim>
                            {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim("tokenId", (tokenId).ToString()),
                            new Claim("UserId", user.Id.ToString()),

                            };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.AuthConfig.JwtSecretKey));
                    var encryptKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.AuthConfig.JwtEncryptionKey));
                    var creds = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
                    var encryptingCredentials = new EncryptingCredentials(encryptKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);

                    var jwtSecurityToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
                        this.AuthConfig.JwtValidIssuer,
                        this.AuthConfig.JwtValidAudience,
                        new ClaimsIdentity(authClaims),
                        null,
                        expires: DateTime.Now.AddHours(this.AuthConfig.JwtExpireInHours),
                        null,
                        signingCredentials: creds,
                        encryptingCredentials: encryptingCredentials
                        );

                    return (tokenId, new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), jwtSecurityToken.ValidTo);
                };

               
                if(await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var tokenTupple= await generateTokenFun();
                     //await userManager.SetAuthenticationTokenAsync(user, "PersonTask", "RefreshToken", tokenTupple.Item2);

                    return new TokenResponse
                    {
                        token = tokenTupple.Item2,
                        expiration = tokenTupple.Item3
                    };

                }
                else
                {
                    throw new TaskException(TaskValidationKeysEnum.WrongUserNameOrPassword);
                }


            });
        }

    }
}
