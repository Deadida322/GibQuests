namespace AuthService.DataContracts.User
{
    public class LoginTokenContract
    {
        /// <summary>
        /// Токен пользователя
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Refresh Токен пользователя
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
