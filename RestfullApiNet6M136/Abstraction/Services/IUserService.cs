using Azure;
using RestfullApiNet6M136.DTOs.UserDTOs;
using RestfullApiNet6M136.Entities.Identity;
using RestfullApiNet6M136.Models;

namespace RestfullApiNet6M136.Abstraction.Services;

public interface IUserService
{
    Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate);
    //public Task ForgetPasswordAsync(string userId, string resetToken, string newPassword)
    public Task<GenericResponseModel<bool>> AssignRoleToUserAsnyc(string userId, string[] roles);

    Task<GenericResponseModel<CreateUserResponseDTO>> CreateAsync(CreateUserDto model);

    public Task<GenericResponseModel<List<UserGetDTO>>> GetAllUsersAsync();

    public Task<GenericResponseModel<string[]>> GetRolesToUserAsync(string userIdOrName); //userin rollarin getirmek

    public Task<GenericResponseModel<bool>> DeleteUserAsync(string userIdOrName);

    public Task<GenericResponseModel<bool>> UpdateUserAsync(UserUpdateDTO model);
}
