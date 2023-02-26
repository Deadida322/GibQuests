using System.ComponentModel.DataAnnotations;

namespace AuthService.DataContracts.User
{
    public class CreateUserContract
    {
        /// <summary>
        /// имя пользователя
        /// </summary>
        [Required(ErrorMessage = "Имя обязательно к заполнению"), 
         MaxLength(100, ErrorMessage = "Максимальная длина имени 100 символов")]
        public string Name { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [Required(ErrorMessage = "Фамилия обязательна к заполнению"),
         MaxLength(100, ErrorMessage = "Максимальная длина фамилии 100 символов")]
        public string Surname { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        [Required(ErrorMessage = "Логин обязателен к заполнению"),
         MaxLength(50, ErrorMessage = "Максимальная длина логина 50 символов")]
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [Required(ErrorMessage = "Пароль обязателен к заполнению"),
         MaxLength(256, ErrorMessage = "Максимальная длина пароля 256 символов")]
        public string Password { get; set; }

        /// <summary>
        /// Аватар пользователя в Base64
        /// </summary>
        public string Img { get; set; }
    }
}
