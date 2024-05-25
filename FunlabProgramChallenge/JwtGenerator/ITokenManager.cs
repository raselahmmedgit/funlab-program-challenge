using System.Security.Claims;
using System.Threading.Tasks;

namespace FunlabProgramChallenge.JwtGenerator
{
    public interface ITokenManager
    {
        Task<TokenModel> CreateTokenAsync(string loginId);
        RefreshTokenModel GenerateRefreshToken();
        ClaimsPrincipal GetClaimsPrincipalByToken(string token);
        bool IsValidateToken(string token);
    }
}
