using AuthService.DataContracts.Interfaces;
using AuthService.DataContracts.User;
using CommonInfrastructure.Http;
using CommonInfrastructure.Http.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuestCore.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [AddUserInRequest]
    [ModelStateValidationActionFilter]
    public class AuthController : ControllerBase
    {
        private IAuthApi _authApi;
        public AuthController(IAuthApi authApi)
        {              
            _authApi = authApi;
        }

        [HttpPost]
        public Task<CommonHttpResponse<UserViewModel>> Register(CreateUserContract createContract)
        {
            return _authApi.RegisterAsync(createContract);
        }

        [HttpPatch]
        [Authorize]
        public Task<CommonHttpResponse<UserViewModel>> UpdateUser(UpdateUserContract contract)
        {
            return _authApi.UpdateUserAsync(contract);
        }

        [HttpDelete]
        [Authorize]
        public Task<CommonHttpResponse<bool>> DeleteUser(DeleteUserContract contract)
        {
            return _authApi.DeleteUserAsync(contract);
        }

        [HttpPatch]
        [Authorize]
        public Task<CommonHttpResponse<bool>> RestoreUser(DeleteUserContract contract)
        {
            return _authApi.RestoreUserAsync(contract);
        }

        [HttpPatch]
        [Authorize]
        public Task<CommonHttpResponse<UserViewModel>> ChangeUserPassword(ChangePasswordContract contract)
        {
            return _authApi.ChangeUserPasswordAsync(contract);
        }

        [HttpPost]
        public Task<CommonHttpResponse<UserViewModel>> Login(LoginContract contract)
        {
            return _authApi.LoginAsync(contract);
        }

        [HttpPost]
        public Task<CommonHttpResponse<UserViewModel>> GetUserInfo(LoginTokenContract contract)
        {
           return _authApi.GetUserInfoAsync(contract);
        }

        [HttpPost]
        public Task<CommonHttpResponse<UserViewModel>> LoginByRefresh(LoginTokenContract contract)
        {
            return _authApi.LoginByRefreshAsync(contract);
        }
    }
}
