using AuthService.DataContracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessQuestService.Core.HelperModels
{
    public class CheckQuestModel
    {
        public ShortUserViewModel User { get; set; }

        public ProgressUserModel Progress { get; set; }
    }
}
