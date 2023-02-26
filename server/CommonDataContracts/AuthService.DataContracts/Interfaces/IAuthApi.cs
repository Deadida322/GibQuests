using AuthService.DataContracts.User;
using CommonInfrastructure.Http;
using Refit;

namespace AuthService.DataContracts.Interfaces
{
    public interface IAuthApi
    {
        #region Auth
        [Post("/Auth/Register")]
        Task<CommonHttpResponse<UserViewModel>> RegisterAsync([Body]CreateUserContract createContract);

        [Patch("/Auth/UpdateUser")]
        Task<CommonHttpResponse<UserViewModel>> UpdateUserAsync([Body] UpdateUserContract contract);

        [Patch("/Auth/RestoreUser")]
        Task<CommonHttpResponse<bool>> RestoreUserAsync([Body] DeleteUserContract contract);

        [Delete("/Auth/DeleteUser")]
        Task<CommonHttpResponse<bool>> DeleteUserAsync([Body] DeleteUserContract contract);

        [Post("/Auth/GetUserInfo")]
        Task<CommonHttpResponse<UserViewModel>> GetUserInfoAsync([Body] LoginTokenContract contract);

        [Post("/Auth/Login")]
        Task<CommonHttpResponse<UserViewModel>> LoginAsync([Body] LoginContract contract);

        [Patch("/Auth/ChangeUserPassword")]
        Task<CommonHttpResponse<UserViewModel>> ChangeUserPasswordAsync(
            [Body] ChangePasswordContract contract);

        [Post("/Auth/LoginByRefresh")]
        Task<CommonHttpResponse<UserViewModel>> LoginByRefreshAsync([Body] LoginTokenContract contract);

        [Get("/Auth/GetUserById/{id}")]
        Task<CommonHttpResponse<ShortUserViewModel>> GetUserByIdAsync([AliasAs("id")] int id);

        [Post("/Auth/GetFilteredUsers")]
        public Task<CommonHttpResponse<IList<ShortUserViewModel>>> GetFilteredUsersAsync([Body] GetFilteredUsersContract contract);

        #endregion
    }
}
