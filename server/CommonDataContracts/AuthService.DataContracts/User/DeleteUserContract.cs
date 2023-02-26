using CommonInfrastructure.Http;

namespace AuthService.DataContracts.User
{
    public class DeleteUserContract : CommonHttpRequest
    {

        public int Id { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}
