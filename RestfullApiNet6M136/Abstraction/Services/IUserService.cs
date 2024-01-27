using Azure;
using RestfullApiNet6M136.DTOs.UserDTOs;
using RestfullApiNet6M136.Entities.Identity;
using RestfullApiNet6M136.Models;

namespace RestfullApiNet6M136.Abstraction.Services
{
    public interface IUserService
    {
        Task<GenericResponseModel<CreateUserResponseDTO>> CreateAsync(CreateUserDto model);
        Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate);
        public Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);

        public Task<GenericResponseModel<List<UserGetDTO>>> GetAllUsersAsync();
        public Task<GenericResponseModel<bool>> AssignRoleToUserAsnyc(string userId, string[] roles);
        public Task<GenericResponseModel<string[]>> GetRolesToUserAsync(string userIdOrName);
        public Task<GenericResponseModel<bool>> DeleteUserAsync(string userIdOrName);
        public Task<GenericResponseModel<bool>> UpdateUserAsync(UserUpdateDTO model);


    }
}
