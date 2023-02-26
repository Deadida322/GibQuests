using GenerateQuestsService.DataContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateQuestsService.DataContracts.Models
{
    public class QuestPolicy
    {
        public PolicyType PolicyType { get; set; }

        public MemberType MemberType { get; set; }
    }
}
