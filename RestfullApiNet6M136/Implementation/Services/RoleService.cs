using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestfullApiNet6M136.Abstraction.Services;
using RestfullApiNet6M136.Entities.Identity;
using RestfullApiNet6M136.Models;

namespace RestfullApiNet6M136.Implementation.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<GenericResponseModel<bool>> CreateRole(string name)
        {
            GenericResponseModel<bool> responseModel = new GenericResponseModel<bool>();

            IdentityResult result = await _roleManager.CreateAsync(new() { Id = Guid.NewGuid().ToString(), Name = name });
            if (result.Succeeded)
            {
                //responseModel.Data = result.Succeeded ? true : false;

                responseModel.Data = result.Succeeded;
                responseModel.StatusCode = 201;
                return responseModel;
            }
            else
            {
                responseModel.Data = result.Succeeded;
                responseModel.StatusCode = 400;
                return responseModel;
            }

        }

        public async Task<GenericResponseModel<bool>> DeleteRole(string id)
        {
            GenericResponseModel<bool> responseModel = new GenericResponseModel<bool>();

            AppRole appRole = await _roleManager.FindByIdAsync(id);
            IdentityResult result = await _roleManager.DeleteAsync(appRole);

            if (result.Succeeded)
            {
                //responseModel.Data = result.Succeeded ? true : false;

                responseModel.Data = result.Succeeded;
                responseModel.StatusCode = 200;
                return responseModel;
            }
            else
            {
                responseModel.Data = result.Succeeded;
                responseModel.StatusCode = 400;
                return responseModel;
            }
        }

        public async Task<GenericResponseModel<object>> GetAllRoles()
        {
            GenericResponseModel<object> responseModel = new GenericResponseModel<object>();

            var data = await _roleManager.Roles.ToListAsync();
            if (data == null)
            {
                responseModel.Data = null;
                responseModel.StatusCode = 400;
                return responseModel;
            }

            responseModel.Data = data;
            responseModel.StatusCode = 200;

            return responseModel;
        }

        public async Task<GenericResponseModel<object>> GetRoleById(string id)
        {
            GenericResponseModel<object> responseModel = new GenericResponseModel<object>();

            var data = await _roleManager.FindByIdAsync(id);


            if (data == null)
            {
                responseModel.Data = null;
                responseModel.StatusCode = 400;
                return responseModel;
            }

            responseModel.Data = data;
            responseModel.StatusCode = 200;

            return responseModel;
        }

        public async Task<GenericResponseModel<bool>> UpdateRole(string id, string name)
        {
            GenericResponseModel<bool> responseModel = new GenericResponseModel<bool>();

            AppRole role = await _roleManager.FindByIdAsync(id);
            role.Name = name;
            IdentityResult result = await _roleManager.UpdateAsync(role);


            if (result.Succeeded)
            {
                //responseModel.Data = result.Succeeded ? true : false;

                responseModel.Data = result.Succeeded;
                responseModel.StatusCode = 200;
                return responseModel;
            }
            else
            {
                responseModel.Data = result.Succeeded;
                responseModel.StatusCode = 400;
                return responseModel;
            }
        }
    }
}
