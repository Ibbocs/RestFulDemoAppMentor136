using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using RestfullApiNet6M136.Abstraction.Services;
using RestfullApiNet6M136.DTOs.UserDTOs;
using RestfullApiNet6M136.Entities.Identity;
using RestfullApiNet6M136.Models;
using Serilog;
using System.Text;

namespace RestfullApiNet6M136.Implementation.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser> userManager;
        readonly IMapper _mapper;

        public UserService(UserManager<AppUser> _userManager, IMapper mapper)
        {
            userManager = _userManager;
            _mapper = mapper;
        }

        //user yaratma
        public async Task<GenericResponseModel<CreateUserResponseDTO>> CreateAsync(CreateUserDto model)
        {
            var response = new GenericResponseModel<CreateUserResponseDTO>();

            var id = Guid.NewGuid().ToString(); //randomdan reqem alib da yazmaq olar ya da elle id vermeliyik.
            IdentityResult result = await userManager.CreateAsync(new()
            {
                Id = id,
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            }, model.Password);

            response.Data = new CreateUserResponseDTO { Succeeded = result.Succeeded };
            response.StatusCode = result.Succeeded ? 200 : 400;


            if (!result.Succeeded)
            {
                response.Data.Message = string.Join(" \n ", result.Errors.Select(error => $"{error.Code} - {error.Description}"));
            }

            //burdan sorasi default olaraq rol vermekdi usere, bunu admin ile de eletdirmek olar ya da yri method icinde.

            AppUser user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
                user = await userManager.FindByEmailAsync(id);
            if (user != null)
                await userManager.AddToRoleAsync(user, "User");

            return response;
        }

        public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime accessTokenDate)
        {
            //bu method rft update elemek ucunnde conntrollerlerde de vere bilerem amma vermmeisem
            //AppUser user = await userManager.FindByIdAsync(user.Id); //todo app user cekilede biler cekilmeyede ehtiyaca gore
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndTime = accessTokenDate.AddMinutes(10);
                await userManager.UpdateAsync(user);
            }
            //else
            //    //throw new NotFoundUserException();
            //    return;

            //throw new UpdateUserException(updateResult.Errors);
        }

        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            //AppUser user = await userManager.FindByIdAsync(userId);
            //if (user != null)
            //{
            //    //resetToken = resetToken.UrlDecode();
            //    byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToken);
            //    resetToken = Encoding.UTF8.GetString(tokenBytes);
            //    IdentityResult result = await userManager.ResetPasswordAsync(user, resetToken, newPassword);
            //    string a = "dcd";
            //    if (result.Succeeded)
            //        await userManager.UpdateSecurityStampAsync(user);
            //    else
            //        throw new SomeThingsWrongException();
            //}

            AppUser user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToken);
                resetToken = Encoding.UTF8.GetString(tokenBytes);

                //byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToken);
                //resetToken = Encoding.UTF8.GetString(tokenBytes);

                IdentityResult result = await userManager.ResetPasswordAsync(user, resetToken, newPassword);

                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        public async Task<GenericResponseModel<List<UserGetDTO>>> GetAllUsersAsync()
        {
            var users = await userManager.Users.ToListAsync();
            GenericResponseModel<List<UserGetDTO>> resModel = new GenericResponseModel<List<UserGetDTO>>() { Data = null, StatusCode = 400 };

            try
            {
                if (users != null && users.Count > 0)
                {
                    var data = _mapper.Map<List<UserGetDTO>>(users);

                    resModel.Data = data;
                    resModel.StatusCode = 200;
                    return resModel;

                }
                else
                {
                    resModel.Data = null;
                    resModel.StatusCode = 400;
                    return resModel;
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error GetAllUser");
                Log.Error(ex.Message + ex.InnerException);
                return resModel;
            }
        }

        public async Task<GenericResponseModel<bool>> AssignRoleToUserAsnyc(string userId, string[] roles)
        {
            AppUser user = await userManager.FindByIdAsync(userId);

            GenericResponseModel<bool> resModel = new GenericResponseModel<bool>() { Data = false, StatusCode = 400 };

            try
            {
                if (user != null)
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    await userManager.RemoveFromRolesAsync(user, userRoles);
                    await userManager.AddToRolesAsync(user, roles);

                    resModel.Data = true;
                    resModel.StatusCode = 200;

                    return resModel;
                }
                else
                {
                    return resModel;
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: Assign Role To User");
                Log.Error(ex.Message + ex.InnerException);
                return resModel;
            }
        }

        public async Task<GenericResponseModel<string[]>> GetRolesToUserAsync(string userIdOrName)
        {
            GenericResponseModel<string[]> resModel = new GenericResponseModel<string[]>() { StatusCode=400, Data=null };

            AppUser user = await userManager.FindByIdAsync(userIdOrName);

            if (user == null)
                user = await userManager.FindByNameAsync(userIdOrName);
            try
            {
                if (user != null)
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    resModel.Data = userRoles.ToArray();
                    resModel.StatusCode=200;
                    return resModel;
                }
                return resModel;
            }
            catch (Exception ex)
            {

                await Console.Out.WriteLineAsync("Error: Delet User Asyn");
                Log.Error(ex.Message + ex.InnerException);
                return resModel;
            }
        }

        public async Task<GenericResponseModel<bool>> DeleteUserAsync(string userIdOrName)
        {
            AppUser user = await userManager.FindByIdAsync(userIdOrName);
            GenericResponseModel<bool> resModel = new GenericResponseModel<bool>() { Data = false, StatusCode = 400 };

            if (user == null)
                user = await userManager.FindByNameAsync(userIdOrName);

            if (user == null)
                throw new ArgumentNullException(nameof(user));//todo bilmirem isleyer bele ya yo

            try
            {
                var data = await userManager.DeleteAsync(user);
                if (data.Succeeded)
                {
                    resModel.Data = true;
                    resModel.StatusCode = 200;
                    return resModel;
                }
                else
                {
                    return resModel;
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: Delet User Asyn");
                Log.Error(ex.Message + ex.InnerException);
                return resModel;
            }
        }

        public async Task<GenericResponseModel<bool>> UpdateUserAsync(UserUpdateDTO model)
        {
            AppUser user = await userManager.FindByIdAsync(model.UserId);

            GenericResponseModel<bool> resModel = new GenericResponseModel<bool>() { Data = false, StatusCode = 400 };

            if (user == null)
                user = await userManager.FindByNameAsync(model.UserName); //name update elese bele id verecek mecbur onda

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                user.UserName = model.UserName;
                user.BirthDate = model.BirthDate;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var data = await userManager.UpdateAsync(user);

                if (data.Succeeded)
                {
                    resModel.Data = true;
                    resModel.StatusCode = 200;

                    return resModel;
                }
                else
                {
                    return resModel;
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: Update User Asyn");
                Log.Error(ex.Message + ex.InnerException);
                return resModel;
            }
        }
    }
}
