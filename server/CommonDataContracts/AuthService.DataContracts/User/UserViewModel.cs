namespace AuthService.DataContracts.User
{
    public class UserViewModel
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Аватар пользователя
        /// </summary>
        public string Img { get; set; }

        /// <summary>
        /// Токен пользователя
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Refresh Токен пользователя
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
