using CommonInfrastructure.Http;

namespace AuthService.DataContracts.User
{
    public class ChangePasswordContract : CommonHttpRequest
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Старый пароль пользователя
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// Новый пароль пользователя
        /// </summary>
        public string NewPassword { get; set; }
    }
}
