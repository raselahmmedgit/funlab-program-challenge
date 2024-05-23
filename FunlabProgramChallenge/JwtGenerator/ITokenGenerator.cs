using System.Security.Claims;
using System.Threading.Tasks;

namespace FunlabProgramChallenge.JwtGenerator
{
    public interface ITokenGenerator
    {
        Task<TokenModel> CreateTokenAsync(string loginId);
        RefreshTokenModel GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
