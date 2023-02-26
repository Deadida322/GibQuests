namespace AuthService.DataContracts.User
{
    /// <summary>
    /// моделя для отображения в карточках
    /// </summary>
    public class ShortUserViewModel
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string UserName { get; set; }
    }
}
