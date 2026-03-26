using System.Security.Claims;
using DocDes.Core.Model;

namespace DocDes.Core.Security;

public interface ITokenService
{
    AccessToken CreateAccessToken(ApplicationUser user);
    AccessToken CreateAccessToken(string userName, int userId, string userCredential);
    RefreshToken CreateRefreshToken(int userId);
    ClaimsPrincipal? ValidateToken(string token);


}