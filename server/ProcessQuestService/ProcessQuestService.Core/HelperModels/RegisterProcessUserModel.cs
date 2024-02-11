using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessQuestService.Core.HelperModels
{
    /// <summary>
    /// Модель для регистрации пользователя в прохождении квеста
    /// </summary>
    public class RegisterProcessUserModel
    {
        /// <summary>
        /// Ключ комнаты для доступа
        /// </summary>
        public string Room { get; set;}

        /// <summary>
        /// Токен доступа для пользователя
        /// </summary>
        public string Token { get; set;}
    }
}
