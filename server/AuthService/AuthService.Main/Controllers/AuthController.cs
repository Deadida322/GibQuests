using AuthService.Core.BusinessLogic;
using AuthService.DataContracts.Interfaces;
using AuthService.DataContracts.User;
using CommonInfrastructure.Http;
using CommonInfrastructure.Http.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Main.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [ModelStateValidationActionFilter]
    public class AuthController : Controller, IAuthApi
    {
        private UserLogic _userLogic;
        public AuthController(UserLogic userLogic)
        {                         
            _userLogic = userLogic;
        }

        [HttpPost]
        public Task<CommonHttpResponse<UserViewModel>> RegisterAsync(CreateUserContract createContract)
        {
            return _userLogic.RegisterAsync(createContract);
        }

        [HttpPatch]
        public Task<CommonHttpResponse<UserViewModel>> UpdateUserAsync(UpdateUserContract contract)
        {
            return _userLogic.UpdateUserAsync(contract);
        }

        [HttpDelete]
        public Task<CommonHttpResponse<bool>> DeleteUserAsync(DeleteUserContract contract)
        {
            return _userLogic.DeleteUserAsync(contract);
        }

        [HttpPatch]
        public Task<CommonHttpResponse<bool>> RestoreUserAsync(DeleteUserContract contract)
        {
            return _userLogic.RestoreUserAsync(contract);
        }

        [HttpPost]
        public Task<CommonHttpResponse<UserViewModel>> LoginAsync(LoginContract contract)
        {
            return _userLogic.LoginAsync(contract);
        }

        [HttpPatch]
        public Task<CommonHttpResponse<UserViewModel>> ChangeUserPasswordAsync(ChangePasswordContract contract)
        {
            return _userLogic.ChangeUserPasswordAsync(contract);
        }

        [HttpPost]
        public Task<CommonHttpResponse<UserViewModel>> GetUserInfoAsync(LoginTokenContract contract)
        {
            return _userLogic.GetUserInfoAsync(contract);
        }

        [HttpPost]
        public Task<CommonHttpResponse<UserViewModel>> LoginByRefreshAsync(LoginTokenContract contract)
        {
            return _userLogic.LoginByRefreshAsync(contract);
        }

        [HttpGet("{id}")]
        public Task<CommonHttpResponse<ShortUserViewModel>> GetUserByIdAsync(int id)
        {
            return _userLogic.GetUserByIdAsync(id);
        }

        [HttpPost]
        public Task<CommonHttpResponse<IList<ShortUserViewModel>>> GetFilteredUsersAsync(GetFilteredUsersContract contract)
        {
            return _userLogic.GetFilteredUsersAsync(contract);
        }


    }
}
