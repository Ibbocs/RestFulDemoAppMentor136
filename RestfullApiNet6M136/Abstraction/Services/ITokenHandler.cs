using RestfullApiNet6M136.DTOs;
using RestfullApiNet6M136.Entities.Identity;

namespace RestfullApiNet6M136.Abstraction.Services
{
    public interface ITokenHandler
    {
        Task<TokenDTO> CreateAccessToken(AppUser user);
        string CreateRefreshToken();
    }
}
