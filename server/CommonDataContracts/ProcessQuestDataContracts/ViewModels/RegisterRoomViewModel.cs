using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessQuestDataContracts.ViewModels
{
    public class RegisterRoomViewModel
    {

        /// <summary>
        /// Ключ комнаты открывшегося прохождения
        /// </summary>
        public Guid Key { get; set; }

        public QuestProcessViewModel Quest { get; set; }
    }
}
