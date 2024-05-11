using Azure;
using RestfullApiNet6M136.DTOs;
using RestfullApiNet6M136.Models;

namespace RestfullApiNet6M136.Abstraction.Services
{
    public interface IAuthoService
    {
        Task<GenericResponseModel<TokenDTO>> LoginAsync(string userNameOrEmail, string password);
        Task<GenericResponseModel<TokenDTO>> LoginWithRefreshTokenAsync(string refreshToken);
        Task<GenericResponseModel<bool>> LogOut(string userNameOrEmail);
        public Task<GenericResponseModel<bool>> PasswordResetAsnyc(string email, string currentPas, string newPas);

        //public Task<string> PasswordResetAsnyc(string email);
        //public Task<bool> VerifyResetTokenAsync(string resetToken, string userId);
    }
}
