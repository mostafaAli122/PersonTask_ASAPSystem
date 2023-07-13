using Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Extensions
{
    public class GeneratedTokenService
    {
        private readonly RequestDelegate _next;

        public GeneratedTokenService(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IUnitOfWork _uow)
        {

            long _tokenId;
            long.TryParse(httpContext.User.Claims.FirstOrDefault(s => s.Type == "tokenId")?.Value, out _tokenId);

            long _userId;
            long.TryParse(httpContext.User.Claims.FirstOrDefault(s => s.Type == "MotoriUserId")?.Value, out _userId);


            var user = _uow.Users.GetById((int)_userId);

            if (httpContext.User.Identity == null  || !httpContext.User.Identity.IsAuthenticated )
            {
               await _next(httpContext);
            }
            else if(_tokenId > 0 )
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            else if (_tokenId > 0 )
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            else
            {
                await _next(httpContext);
            }
          
        }



    }

    public static class GeneratedTokenExtensions
    {
        public static IApplicationBuilder UseGeneratedTokenCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GeneratedTokenService>();
        }
    }
}
