using CommonInfrastructure.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessQuestDataContracts.DataContracts
{
    public class RegisterRoomContract : CommonHttpRequest
    {
        public int QuestId { get; set; }

        public int? MaxDuration {  get; set; }

        public Guid? Key { get; set; }
    }
}
